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
using CodeFirstMetadataTest.Diagnostic;
using System.IO;
using RoslynDom.CSharp;
using Microsoft.CodeAnalysis.MSBuild;
using DiagnosticsGenerationTemplates;
using CodeFirst.Provider;

namespace ConsoleRunT4Example
{
   class Program
   {
      static void Main(string[] args)
      {
         var provider = new Provider();
         var runner = new T4TemplateRunner(provider);
         var startDirectory = Path.Combine(FileSupport.ProjectPath(AppDomain.CurrentDomain.BaseDirectory), "..\\DiagnosticsGeneration\\DiagnosticsGenerationMetadata");
         startDirectory = Path.GetFullPath(startDirectory);
         var ws = MSBuildWorkspace.Create();
         var projectPath = FileSupport.GetNearestCSharpProject(startDirectory);
         // For now: wait for the result
         var project = ws.OpenProjectAsync(projectPath).Result;
         var dict = runner.CreateOutputStringsFromProject(project,"..\\Output");
         foreach (var pair in dict)
         {
            File.WriteAllText(pair.Key, pair.Value);
         }

         //string[] outFiles;
         //var templateMaps = new List<TemplateMap>();
         //var provider = new Provider();
         //var entryPoints = provider.GetServices<ICodeFirstEntry>();

         //foreach (var entryPoint in entryPoints)
         //{
         //   var metadataLoader = provider.GetService<IMetadataLoader>(x=>x.ReturnType == entryPoint.GetType());
         //   templateMaps.Add(entryPoint.AttributeId, new CodeFirstMetadataLoader<CodeFirstDiagnosticGroup>(),
         //    new T4TemplateRunner(),
         //    new DiagnosticTemplate(),
         //    metadataDirectory: @"DiagnosticsGeneration\DiagnosticsGenerationMetadata",
         //    inputExtension: ".cs"))
         //}



         //templateMaps.Add(new TemplateMap<CodeFirstSemanticLogGroup, EventSourceTemplate>
         //   ("SemanticLog",
         //    new CodeFirstMetadataLoader<CodeFirstSemanticLogGroup>(),
         //    new T4TemplateRunner(),
         //    new EventSourceTemplate()));

         //templateMaps.Add(new TemplateMap<CodeFirstClassGroup, PropertyChangedTemplate>
         //   ("NotifyPropertyChanged",
         //    new CodeFirstMetadataLoader<CodeFirstClassGroup>(),
         //    new T4TemplateRunner(),
         //    new PropertyChangedTemplate()));

         //templateMaps.Add(new TemplateMap<CodeFirstDiagnosticGroup, DiagnosticTemplate>
         //   ("DiagnosticAndCodeFix",
         //    new CodeFirstMetadataLoader<CodeFirstDiagnosticGroup>(),
         //    new T4TemplateRunner(),
         //    new DiagnosticTemplate(),
         //    metadataDirectory: @"DiagnosticsGeneration\DiagnosticsGenerationMetadata",
         //    inputExtension: ".cs"));

         //templateMaps.Add(new TemplateMap<CodeFirstDiagnosticGroup, DiagnosticTemplate>
         //   (
         //    new CodeFirstMetadataLoader<CodeFirstDiagnosticGroup>(),
         //    new T4TemplateRunner(),
         //    new DiagnosticTemplate(),
         //    metadataDirectory: @"DiagnosticsGeneration\DiagnosticsGenerationMetadata",
         //    inputExtension: ".cs"));

         //var startDirectory = FileSupport.ProjectPath(AppDomain.CurrentDomain.BaseDirectory);
         //var ws = MSBuildWorkspace.Create();
         //foreach (var templateMap in templateMaps)
         //{
         //   var sourceDirectory = Path.Combine(startDirectory, "..");
         //   var sources = templateMap.GetFileNames(sourceDirectory);
         //   var firstSource = sources.FirstOrDefault();
         //   if (firstSource != null)
         //   {
         //      var projectPath = FileSupport.GetNearestCSharpProject(firstSource);
         //      // For now: wait for the result
         //      var project = ws.OpenProjectAsync(projectPath).Result;
         //      var rootGroup = RDom.CSharp.LoadGroup(project);
         //      var output = templateMap.OutputToFiles(out outFiles, rootGroup, sources.ToArray());
         //   }
         //}

         Console.Write("You're done. Adjacent to your generated file (temp location) you'll find a generated file to include in your project");
         Console.Read();
      }
   }
}
