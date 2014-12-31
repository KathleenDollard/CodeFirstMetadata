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

   }
}
