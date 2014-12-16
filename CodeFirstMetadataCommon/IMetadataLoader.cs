using RoslynDom.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace CodeFirst.Common
{
   public interface IMetadataLoader
   {
      Type ReturnType { get; }
   }

   public interface IMetadataLoader<T> : IMetadataLoader
      where T : CodeFirstMetadata
   {
      T LoadFromFile(string fileName, string AttributeIdentifier);
      T LoadFromString(string input, string AttributeIdentifier);
      T LoadFrom(IRoot root, string AttributeIdentifier);
      T LoadFrom(Document document, string AttributeIdentifier);
   }
}
