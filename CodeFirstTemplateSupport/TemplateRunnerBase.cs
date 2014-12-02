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
            var metadataLoader = ReflectionHelpers.InvokeGenericMethod(serviceProvider.GetType().GetTypeInfo(),
                        "GetMetadataLoader", entryPointType, serviceProvider)
                        as IMetadataLoader;
            var childProperty = GetChildProperty(entryPointType);
            foreach (var template in templates)
            {
               var metadataType = template
                              .GetType()
                              .GetInterface("ITemplate`1")
                              ?.GenericTypeArguments.FirstOrDefault();
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

      private IFactoryAccess FactoryAccess
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
                                 childType = x.ChildProperty?.PropertyType.GenericTypeArguments.First(),
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
                           .Where(x=>x!=null)
                           .SelectMany(x => (childProperty.GetValue(x) as IEnumerable<CodeFirstMetadata>)).ToList();
         }
         else
         {
            candidates = metadataList.Where(x => x.GetType().FullName == typeof(TLocal).FullName);
         }
         if (candidates.Any())
         {
            foreach (var candidate in candidates)
            {
               var candidateMaps = templateMaps;
               //.Where(x => x.EntryPointType == typeof(T));
               foreach (var candidateMap in candidateMaps)
               {
                  var inFileName = candidate
                                       .AncestorsAndSelf
                                       .OfType<ICodeFirstMetadataNamespace>()
                                       .FirstOrDefault()
                                       ?.FilePath;
                  // TODO: This is wrong so I can test the rest of the system
                  var outFileName = Path.Combine(Path.GetDirectoryName(inFileName), Path.GetFileNameWithoutExtension(inFileName)) + ".g.cs";
                  var templateType = candidateMap.ChildProperty != null
                                  ? candidateMap.ChildProperty?.PropertyType.GenericTypeArguments.First()
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

      private string CreateOutputString<TLocal>(ITemplate<TLocal> template, TLocal candidate)
         where TLocal : CodeFirstMetadata<TLocal>
      {
         var outText = template.GetOutput(candidate );
         return outText; 
      }

      private IEnumerable<T> GetMetadata<T>(
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
