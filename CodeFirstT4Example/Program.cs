using System;
using System.Collections.Generic;
using System.Linq;
using CodeFirst;
using CodeFirst.Common;
using CodeFirstMetadataT4Support;
using CodeFirstMetadataTest.SemanticLog;
using CodeFirstMetadataTest.PropertyChanged;
using CodeFirstT4Example;
using RoslynDom.Common;

namespace ConsoleRunT4Example
{
   class Program
   {
      static void Main(string[] args)
      {
         string[] outFiles;
         var templateMaps = new List<TemplateMap>();

         templateMaps.Add(new TemplateMap<CodeFirstSemanticLogGroup, EventSourceTemplate>
            ("SemanticLog",
             new CodeFirstMetadataLoader<CodeFirstSemanticLogGroup>(),
             new T4TemplateRunner(),
             new EventSourceTemplate()));

         templateMaps.Add(new TemplateMap<CodeFirstClassGroup, PropertyChangedTemplate>
            ("NotifyPropertyChanged",
             new CodeFirstMetadataLoader<CodeFirstClassGroup>(),
             new T4TemplateRunner(),
             new PropertyChangedTemplate()));

         //templateMaps.Add(new TemplateMap<CodeFirstDiagnosticGroup, DiagnosticTemplate>
         //   ("DiagnosticAndCodeFix",
         //    new CodeFirstMetadataLoader<CodeFirstDiagnosticGroup>(),
         //    new T4TemplateRunner(),
         //    new DiagnosticTemplate()));

         foreach (var templateMap in templateMaps)
         {
            var startDirectory = FileSupport.ProjectPath(AppDomain.CurrentDomain.BaseDirectory);
            var sources = templateMap.GetFileNames(startDirectory);
            var output = templateMap.OutputToFiles(out outFiles, sources.ToArray());
         }

         Console.Write("You're done. Adjacent to your cfcs file you'll find a generated file to include in your project");
         Console.Read();
      }
   }
}
