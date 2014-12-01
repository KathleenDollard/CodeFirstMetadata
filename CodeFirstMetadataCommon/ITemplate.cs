using System;

namespace CodeFirst.Common
{
   public interface ITemplate
   {
      Type MetadataType { get; }
   }

   public interface ITemplate<T> : ITemplate 
      where T : CodeFirstMetadata<T>
   {
      string GetOutput(T metadata);
   }
}