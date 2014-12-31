using CodeFirst.Common;
using Microsoft.CodeAnalysis;
using RoslynDom.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RoslynDom;
using CodeFirst.Provider;
using Microsoft.CodeAnalysis.MSBuild;

namespace CodeFirst.TemplateSupport
{
   public abstract class TemplateRunnerBase : ITemplateRunner
   {
      public abstract string CreateString<TMetadata, TTemplate>(TMetadata metadata, TTemplate template)
               where TMetadata : CodeFirstMetadata<TMetadata>;

      private ICodeFirstServiceProvider serviceProvider;
      private IFactoryAccess factoryAccess;

      protected TemplateRunnerBase(ICodeFirstServiceProvider serviceProvider)
      {
         if (serviceProvider == null)
         { serviceProvider = new ServiceProvider(); }
         this.serviceProvider = serviceProvider;
         serviceProvider.LoadIntoContainer<IFactoryAccess>();
      }

      private IEnumerable<TemplateMap> MakeTemplateMaps()
      {
         var ret = new List<TemplateMap>();
         var entryPoints = serviceProvider.GetServices<ICodeFirstEntry>();
         var templates = serviceProvider.GetServices<ITemplate>();

         foreach (var entryPoint in entryPoints)
         {
            var entryPointType = entryPoint.GetType();
            var metadataLoader = serviceProvider.GetMetadataLoader(entryPoint.GetType().GetTypeInfo());
            var childProperty = GetChildProperty(entryPointType);
            foreach (var template in templates)
            {
               var interfaceType = template
                              .GetType()
                              .GetInterface("ITemplate`1");
               var metadataType = interfaceType == null
                                    ? null
                                    : interfaceType.GenericTypeArguments.FirstOrDefault();
               if (metadataType != null)
               {
                  var isDirectMatch = false;
                  PropertyInfo childMatch = null;
                  if (metadataType.IsAssignableFrom(entryPoint.GetType()))
                  { isDirectMatch = true; }
                  else if (childProperty != null
                              && metadataType.IsAssignableFrom(childProperty.PropertyType.GenericTypeArguments.First()))
                  { childMatch = childProperty; }
                  if (isDirectMatch)
                  { ret.Add(new TemplateMap(entryPoint.AttributeId, entryPointType, template, metadataLoader)); }
                  else if (childMatch != null)
                  { ret.Add(new TemplateMap(entryPoint.AttributeId, entryPointType, template, metadataLoader, childMatch)); }
               }
            }
         }
         return ret;
      }


      private PropertyInfo GetChildProperty(Type entryPointType)
      {
         var properties = entryPointType.GetProperties()
                           .Where(x => x.PropertyType.IsConstructedGenericType)
                           .Where(x => x.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>));
         foreach (var prop in properties)
         {
            if (typeof(CodeFirstMetadata).IsAssignableFrom(prop.PropertyType.GenericTypeArguments.First()))
            { return prop; }
         }
         return null;
      }

      protected IFactoryAccess FactoryAccess
      {
         get
         {
            if (factoryAccess == null)
            { factoryAccess = serviceProvider.GetService<IFactoryAccess>(); }
            return factoryAccess;
         }
      }

      public IDictionary<string, string> CreateOutputStringsFromFiles(string inputRootDirectory, string outputRootDirectory, bool noRecurse = false, bool whatIf = false)
      {

         var searchOption = noRecurse
                              ? SearchOption.TopDirectoryOnly
                              : SearchOption.AllDirectories;
         var rootGroup = FactoryAccess.LoadFromFiles(Directory.GetFiles(inputRootDirectory, "*.cs", searchOption));
         return CreateOutputStrings(rootGroup, outputRootDirectory);

      }

      public IDictionary<string, string> CreateOutputStringsFromProject(string relativePath, string outputRootDirectory, bool whatIf = false)
      {
         var startDirectory = Path.Combine(FileSupport.ProjectPath(AppDomain.CurrentDomain.BaseDirectory), relativePath);
         startDirectory = Path.GetFullPath(startDirectory);
         var ws = MSBuildWorkspace.Create();
         var projectPath = FileSupport.GetNearestCSharpProject(startDirectory);
         // For testing: wait for the result
         var project = ws.OpenProjectAsync(projectPath).Result;
         return CreateOutputStringsFromProject(project, outputRootDirectory, whatIf);
      }
      
      public IDictionary<string, string> CreateOutputStringsFromProject(Project project, string outputRootDirectory, bool whatIf = false)
      {
         var rootGroup = FactoryAccess.LoadGroup(project);
         return CreateOutputStrings(rootGroup, outputRootDirectory);
      }

      private IDictionary<string, string> CreateOutputStrings(IRootGroup rootGroup, string outputRootDirectory)
      {
         var ret = new Dictionary<string, string>();
         // TODO: Allow filtering to limit the breadth of generation
         var templateMaps = MakeTemplateMaps();
         var distinct = templateMaps
                              .Select(x => new
                              {
                                 x.EntryPointType,
                                 x.MetadataLoader,
                                 childProperty = x.ChildProperty,
                                 childType = x.ChildProperty == null
                                             ? null
                                             : x.ChildProperty.PropertyType.GenericTypeArguments.First(),
                                 x.AttributeIdentifier
                              })
                              .Distinct()
                              .ToList();
         foreach (var set in distinct)
         {
            var newDict = ReflectionHelpers.InvokeGenericMethod(typeof(TemplateRunnerBase).GetTypeInfo(), "CreateOutputStringsCore",
                                       set.EntryPointType, this,
                                       rootGroup, outputRootDirectory, set.MetadataLoader, set.childProperty, set.childType, set.AttributeIdentifier, templateMaps)
                                    as IDictionary<string, string>;
            foreach (var pair in newDict)
            { ret.Add(pair.Key, pair.Value); }
         }
         return ret;
      }

      private IDictionary<string, string> CreateOutputStringsCore<TLocal>(IRootGroup rootGroup, string outputRootDirectory,
                        IMetadataLoader<TLocal> metadataLoader, PropertyInfo childProperty, Type childPropertyType, string attributeIdentifier, IEnumerable<TemplateMap> templateMaps)
         where TLocal : CodeFirstMetadata<TLocal>
      {
         var newDict = new Dictionary<string, string>();
         var metadataList = GetMetadata(attributeIdentifier, metadataLoader, rootGroup.Roots);

         IEnumerable<CodeFirstMetadata> candidates;
         if (childPropertyType != null)
         {
            candidates = metadataList
                           .Where(x => x != null)
                           .SelectMany(x => (childProperty.GetValue(x) as IEnumerable<CodeFirstMetadata>))
                           .ToList();
         }
         else
         {
            candidates = metadataList.Where(x => x.GetType().FullName == typeof(TLocal).FullName);
         }
         if (candidates.Any())
         {
            foreach (var candidate in candidates)
            {
               var candidateMaps = templateMaps.ToList();
               //.Where(x => x.EntryPointType == typeof(T));
               foreach (var candidateMap in candidateMaps)
               {
                  var firstNamespace = candidate
                                       .AncestorsAndSelf
                                       .OfType<ICodeFirstMetadataNamespace>()
                                       .FirstOrDefault();
                  var inFileName = firstNamespace == null
                                      ? null
                                      : firstNamespace.FilePath;
                  var outFileName = GetFileName(candidateMap.Template.FilePathHint, inFileName, "");
                  var genericChildPropertyType = candidateMap.ChildProperty == null
                                                 ? null
                                                 : candidateMap.ChildProperty.PropertyType.GenericTypeArguments.FirstOrDefault();
                  var templateType = genericChildPropertyType != null
                                        ? genericChildPropertyType
                                        : candidateMap.EntryPointType;
                  var outText = ReflectionHelpers.InvokeGenericMethod(typeof(TemplateRunnerBase).GetTypeInfo(), "CreateOutputString",
                                       templateType, this,
                                       candidateMap.Template, candidate)
                                    as string;
                  newDict.Add(outFileName, outText);
               }
            }
            //newDict = ReflectionHelpers.InvokeGenericMethod(this.GetType().GetTypeInfo(), "CreateOutputStringsGeneric",
            //                          typeof(TLocal), this,
            //                          candidates, templateMaps,metadataLoader, outputRootDirectory)
            //                       as Dictionary<string, string>;
         }
         return newDict;
      }

      /// <summary>
      /// A template for creation of a file path. The following values are supported and
      /// should be included in curly braces. 
      /// 
      /// - MetadataPath
      /// - MetadataFileName
      /// - TemplatePath
      /// - TemplateFileName
      /// - ExecutionPath
      /// 
      /// </summary>
      private string GetFileName(string filePathHint, string metadataFilePath, string templateFilePath)
      {
         var executionFilePath = Environment.CurrentDirectory;
         var hint = filePathHint
                        .Replace("{MetadataPath}", @"{0}")
                        .Replace("{MetadataFileName}", @"{1}")
                        .Replace("{TemplatePath}", @"{2}")
                        .Replace("{TemplateFileName}", @"{3}")
                        .Replace("{ExecutionPath}", @"{4}");
         var outFileName = string.Format(hint,
                     Path.GetDirectoryName(metadataFilePath),
                     Path.GetFileNameWithoutExtension(metadataFilePath),
                     "",
                     "",
                     executionFilePath);
         return Path.GetFullPath(outFileName);
      }

      protected string CreateOutputString<TLocal>(ITemplate<TLocal> template, TLocal candidate)
         where TLocal : CodeFirstMetadata<TLocal>
      {
         var outText = template.GetOutput(candidate);
         return outText;
      }

      protected IEnumerable<T> GetMetadata<T>(
                         string attributeIdentifier,
                        IMetadataLoader<T> metadataLoader,
                        IEnumerable<IRoot> roots)
                         where T : CodeFirstMetadata<T>
      {
         var ret = new List<T>();
         foreach (var root in roots)
         {
            var metadata = metadataLoader.LoadFrom(root, attributeIdentifier);
            ret.Add(metadata);
         }
         return ret;
      }
   }
}
