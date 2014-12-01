using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RoslynDom.Common;
using Microsoft.CodeAnalysis;

namespace CodeFirst.Common
{
   public class TemplateMap

   {
      public TemplateMap(string attributeIdentifier,
                  Type entryPointType,
                  ITemplate template,
                  IMetadataLoader metadataLoader,
                  PropertyInfo childProperty = null)
      {
         AttributeIdentifier = attributeIdentifier;
         Template = template;
         MetadataLoader = metadataLoader;
         ChildProperty = childProperty;
         EntryPointType = entryPointType;
      }
      public string AttributeIdentifier { get; private set; }
      public ITemplate Template { get; private set; }
      public IMetadataLoader MetadataLoader { get; private set; }
      public PropertyInfo ChildProperty { get; private set; }
      public Type EntryPointType { get; private set; }

      //public IEnumerable<string> GetFileNames(string startDirectory)
      //{
      //   startDirectory = Path.Combine(startDirectory, MetadataDirectory);
      //   var sources = FileSupport.GetMatchingFiles("*." + InputExtension, startDirectory, true)
      //                        .Where(x => !x.Contains("\\TemporaryGeneratedFile_"))
      //                        .Where(x => !x.Contains("\\AssemblyInfo.cs"))
      //                        .Where(x => !x.EndsWith(".g.cs"));
      //   return sources;
      //}

      //public abstract bool OutputToFiles(out string[] outputFiles, params string[] sources);

      //public abstract bool OutputToFiles(out string[] outputFiles, IRootGroup rootGroup, params string[] sources);

      //private static string RemovePeriod(string fileExtension)
      //{
      //   if (fileExtension.StartsWith(".")) return fileExtension.Substring(1);
      //   return fileExtension;
      //}
   }

   //public class TemplateMap<TMetadata, TTemplate>
   //     : TemplateMap
   //     where TMetadata : CodeFirstMetadata<TMetadata>
   //{
   //   public TemplateMap
   //           (string attributeId,
   //             IMetadataLoader<TMetadata> metadataLoader,
   //             ITemplateRunner templateRunner,
   //             TTemplate template,
   //             string metadataDirectory,
   //             string inputExtension = ".cfcs",
   //             string outputFormat = "{0}.g.cs",
   //             string outputDirectory = null)
   //     : base(attributeId, templateRunner, template, metadataDirectory, inputExtension, outputFormat, outputDirectory)
   //   {
   //      MetadataLoader = metadataLoader;
   //   }

   //   public IMetadataLoader<TMetadata> MetadataLoader { get; private set; }

   //   //public override bool OutputToFiles(
   //   //   out string[] outputFiles,
   //   //   params string[] sources)
   //   //{
   //   //   var outputs = new List<string>();
   //   //   foreach (var inFileName in sources)
   //   //   {
   //   //      // Switch to regex, but for now use extension
   //   //      if (Path.GetExtension(inFileName).Equals("." + InputExtension, StringComparison.OrdinalIgnoreCase))
   //   //      {
   //   //         string outFileName = GetOutputFileName(inFileName);
   //   //         TMetadata metadata = MetadataLoader.LoadFromFile(inFileName, AttributeIdentifier);
   //   //         if (metadata != null)
   //   //         {
   //   //            // TODO: Have more sophisticated response to validation failrue that doesn't abort run
   //   //            if (!metadata.ValidateAndUpdate()) throw new InvalidOperationException("Metadata validation failed");
   //   //            Type templateExpectedType = GetTemplateExpectedType(Template);
   //   //            string output = "";
   //   //            if (typeof(TMetadata) == templateExpectedType)
   //   //            { output = TemplateRunner.CreateString(metadata, Template); }
   //   //            else
   //   //            {
   //   //               output = (string)ReflectionHelpers.InvokeGenericMethod(this.GetType().GetTypeInfo(),
   //   //                            "LoopOverItems", templateExpectedType, this, metadata, Template);
   //   //            }
   //   //            if (!string.IsNullOrWhiteSpace(output))
   //   //            {
   //   //               outputs.Add(outFileName);
   //   //               File.WriteAllText(outFileName, output);
   //   //            }
   //   //         }
   //   //      }
   //   //   }
   //   //   outputFiles = outputs.ToArray();
   //   //   return true;
   //   //}

   //   //public override bool OutputToFiles(
   //   //   out string[] outputFiles,
   //   //   IRootGroup rootGroup,
   //   //   params string[] sources)
   //   //{
   //   //   sources = sources.Select(x => Path.GetFullPath(x)).ToArray();
   //   //   var outputs = new List<string>();
   //   //   foreach (var inFileName in sources)
   //   //   {
   //   //      // Switch to regex, but for now use extension
   //   //      if (Path.GetExtension(inFileName).Equals("." + InputExtension, StringComparison.OrdinalIgnoreCase))
   //   //      {
   //   //         var root = rootGroup.Roots
   //   //                        .Where(x => x.FilePath == inFileName)
   //   //                        .FirstOrDefault();
   //   //         if (root != null)
   //   //         {
   //   //            string outFileName = GetOutputFileName(inFileName);
   //   //            TMetadata metadata = MetadataLoader.LoadFrom(root, AttributeIdentifier);
   //   //            if (metadata != null)
   //   //            {
   //   //               // TODO: Have more sophisticated response to validation failrue that doesn't abort run
   //   //               if (!metadata.ValidateAndUpdate()) throw new InvalidOperationException("Metadata validation failed");
   //   //               Type templateExpectedType = GetTemplateExpectedType(Template);
   //   //               string output = "";
   //   //               if (typeof(TMetadata) == templateExpectedType)
   //   //               { output = TemplateRunner.CreateString(metadata, Template); }
   //   //               else
   //   //               {
   //   //                  output = (string)ReflectionHelpers.InvokeGenericMethod(this.GetType().GetTypeInfo(),
   //   //                               "LoopOverItems", templateExpectedType, this, metadata, Template);
   //   //               }
   //   //               if (!string.IsNullOrWhiteSpace(output))
   //   //               {
   //   //                  outputs.Add(outFileName);
   //   //                  File.WriteAllText(outFileName, output);
   //   //               }
   //   //            }
   //   //         }
   //   //      }
   //   //   }
   //   //   outputFiles = outputs.ToArray();
   //   //   return true;
   //   //}

   //   //private string GetOutputFileName(string inFileName)
   //   //{
   //   //   string fullPath = Path.GetDirectoryName(inFileName);
   //   //   string shortName = Path.GetFileNameWithoutExtension(inFileName);
   //   //   string fullName = Path.Combine(fullPath, shortName);
   //   //   string outFileName = string.Format(OutputFormat, fullName);
   //   //   return outFileName;
   //   //}

   //   //private Type GetTemplateExpectedType<T>(T template)
   //   //{
   //   //   var currentType = template.GetType();
   //   //   while (currentType != typeof(object))
   //   //   {
   //   //      if (currentType.IsConstructedGenericType)
   //   //      {
   //   //         var innerType = currentType.GetGenericArguments().First();
   //   //         return innerType;
   //   //      }
   //   //      else { currentType = currentType.BaseType; }
   //   //   }
   //   //   return null;
   //   //}

   //   //private string LoopOverItems<TExpectedType>(TMetadata metadata, TTemplate template)
   //   //    where TExpectedType : CodeFirstMetadata<TExpectedType>
   //   //{
   //   //   // TODO: Be more flexible in the medata location 
   //   //   var children = GetChildrenOfType<TExpectedType>(metadata);
   //   //   var output = "";
   //   //   foreach (TExpectedType child in children)
   //   //   { output += TemplateRunner.CreateString(child, template); }
   //   //   return output;
   //   //}

   //   //private IEnumerable<TExpectedType> GetChildrenOfType<TExpectedType>(TMetadata metadata)
   //   //    where TExpectedType : CodeFirstMetadata<TExpectedType>
   //   //{
   //   //   var expectedType = ReflectionHelpers.MakeGenericType(
   //   //               typeof(IEnumerable<>).GetTypeInfo(),
   //   //               typeof(TExpectedType).GetTypeInfo());
   //   //   var typeInfo = metadata.GetType().GetTypeInfo();
   //   //   var prop = typeInfo.GetAllProperties().Where(x => x.PropertyType == expectedType).FirstOrDefault();
   //   //   var listAsObject = prop.GetValue(metadata);
   //   //   var list = listAsObject as IEnumerable<TExpectedType>;
   //   //   return list;
   //   //}
   //}
}
