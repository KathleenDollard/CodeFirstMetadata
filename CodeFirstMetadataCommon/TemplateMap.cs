using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RoslynDom.Common;

namespace CodeFirst.Common
{
   public abstract class TemplateMap

   {
      public TemplateMap(string attributeIdentifier,
                  ITemplateRunner templateRunner,
                  object template,
                  string inputExtension,
                  string outputFormat)
      {
         AttributeIdentifier = attributeIdentifier;
         TemplateRunner = templateRunner;
         Template = template;
         InputExtension = RemovePeriod(inputExtension);
         OutputFormat = outputFormat;
      }
      public string AttributeIdentifier { get; private set; }
      public ITemplateRunner TemplateRunner { get; private set; }
      public object Template { get; private set; }
      public string InputExtension { get; private set; }
      public string OutputFormat { get; private set; }

      public IEnumerable<string> GetFileNames(string startDirectory)
      {
         var sources = FileSupport.GetMatchingFiles("*." + InputExtension, startDirectory, true)
                              .Where(x => !x.Contains("\\TemporaryGeneratedFile_"))
                              .Where(x => !x.Contains("\\AssemblyInfo.cs"));
         return sources;
      }

      public abstract bool OutputToFiles(out string[] outputFiles, params string[] sources);

      private static string RemovePeriod(string fileExtension)
      {
         if (fileExtension.StartsWith(".")) return fileExtension.Substring(1);
         return fileExtension;
      }
   }

   public class TemplateMap<TMetadata, TTemplate>
        : TemplateMap
        where TMetadata : CodeFirstMetadata<TMetadata>
   {
      public TemplateMap
              (string attributeId,
                IMetadataLoader<TMetadata> metadataLoader,
                ITemplateRunner templateRunner,
                TTemplate template,
                string inputExtension = ".cfcs",
                string outputFormat = "{0}.g.cs")
        : base(attributeId, templateRunner, template, inputExtension, outputFormat)
      {
         MetadataLoader = metadataLoader;
      }

      public IMetadataLoader<TMetadata> MetadataLoader { get; private set; }

      public override bool OutputToFiles(
         out string[] outputFiles,
         params string[] sources)
      {
         var outputs = new List<string>();
         foreach (var inFileName in sources)
         {
            // Switch to regex, but for now use extension
            if (Path.GetExtension(inFileName).Equals("." + InputExtension, StringComparison.OrdinalIgnoreCase))
            {
               string outFileName = GetOutputFileName(inFileName);
               TMetadata metadata = MetadataLoader.LoadFromFile(inFileName, AttributeIdentifier);
               if (metadata != null)
               {
                  // TODO: Have more sophisticated response to validation failrue that doesn't abort run
                  if (!metadata.ValidateAndUpdate()) throw new InvalidOperationException("Metadata validation failed");
                  Type templateExpectedType = GetTemplateExpectedType(Template);
                  string output = "";
                  if (typeof(TMetadata) == templateExpectedType)
                  { output = TemplateRunner.CreateString(metadata, Template); }
                  else
                  {
                     output = (string)ReflectionHelpers.InvokeGenericMethod(this.GetType().GetTypeInfo(),
                                  "LoopOverItems", templateExpectedType, this, metadata, Template);
                  }
                  if (!string.IsNullOrWhiteSpace(output))
                  {
                     outputs.Add(outFileName);
                     File.WriteAllText(outFileName, output);
                  }
               }
            }
         }
         outputFiles = outputs.ToArray();
         return true;
      }

      private string GetOutputFileName(string inFileName)
      {
         string fullPath = Path.GetDirectoryName(inFileName);
         string shortName = Path.GetFileNameWithoutExtension(inFileName);
         string fullName = Path.Combine(fullPath, shortName);
         string outFileName = string.Format(OutputFormat, fullName);
         return outFileName;
      }

      private Type GetTemplateExpectedType<T>(T template)
      {
         var currentType = template.GetType();
         while (currentType != typeof(object))
         {
            if (currentType.IsConstructedGenericType)
            {
               var innerType = currentType.GetGenericArguments().First();
               return innerType;
            }
            else { currentType = currentType.BaseType; }
         }
         return null;
      }

      private string LoopOverItems<TExpectedType>(TMetadata metadata, TTemplate template)
          where TExpectedType : CodeFirstMetadata<TExpectedType>
      {
         // TODO: Be more flexible in the medata location 
         var children = GetChildrenOfType<TExpectedType>(metadata);
         var output = "";
         foreach (TExpectedType child in children)
         { output += TemplateRunner.CreateString(child, template); }
         return output;
      }

      private IEnumerable<TExpectedType> GetChildrenOfType<TExpectedType>(TMetadata metadata)
          where TExpectedType : CodeFirstMetadata<TExpectedType>
      {
         var expectedType = ReflectionHelpers.MakeGenericType(
                     typeof(IEnumerable<>).GetTypeInfo(),
                     typeof(TExpectedType).GetTypeInfo());
         var typeInfo = metadata.GetType().GetTypeInfo();
         var prop = typeInfo.GetAllProperties().Where(x => x.PropertyType == expectedType).FirstOrDefault();
         var listAsObject = prop.GetValue(metadata);
         var list = listAsObject as IEnumerable<TExpectedType>;
         return list;
      }
   }
}
