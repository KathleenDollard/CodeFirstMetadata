using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoslynDom.Common;

namespace CodeFirst.Common
{
   public abstract class TemplateMap2
   {
      protected TemplateMap2
               (string attributeId,
               ITemplateRunner templateRunner,
               object template,
               string inputExtension,
               string outputFormat)
      {
         AttributeIdentifier = attributeId;
         TemplateRunner = templateRunner;
         InputExtension = RemovePeriod(inputExtension);
         OutputFormat = outputFormat;
         Template = template;
      }
      public readonly string AttributeIdentifier; // Example of a public readonly field
      public ITemplateRunner TemplateRunner { get; private set; }
      public string InputExtension { get; private set; }
      public string OutputFormat { get; private set; }
      public object Template { get; private set; }

      public IEnumerable<string> GetFileNames(string startDirectory)
      {
         var sources = FileSupport.GetMatchingFiles("*." + InputExtension, startDirectory, true).ToArray();
         return sources;
      }
      private static string RemovePeriod(string fileExtension)
      {
         if (fileExtension.StartsWith(".")) return fileExtension.Substring(1);
         return fileExtension;
      }
   }

   public class TemplateMap2<T>
         : TemplateMap2
         where T : CodeFirstMetadata
   {
      public TemplateMap2
              (string attributeId,
               IMetadataLoader<T> metadataLoader,
               ITemplateRunner templateRunner,
               object template,
               string inputExtension = ".cfcs",
               string outputFormat = "{0}.g.cs")
          : base(attributeId, templateRunner, template, inputExtension, outputFormat)
      {
         MetadataLoader = metadataLoader;
      }

      public IMetadataLoader<T> MetadataLoader { get; private set; }

      public bool RunToFile(
         out string[] outputFiles,
         params string[] sources)
      {
         var outputs = new List<string>();
         outputFiles = outputs.ToArray();
         return true;
      }

   }
}
