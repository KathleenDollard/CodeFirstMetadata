using System;

namespace CodeFirst.Common
{
   public interface ITemplate
   {
      Type MetadataType { get; }

      /// <summary>
      /// A template for creation of a file path. The following values are supported and
      /// should be included in curly braces. 
      /// - MetadataPath
      /// - MetadataFileName
      /// - TemplatePath
      /// - TemplateFileName
      /// - ExecutionPath
      /// </summary>
      string FilePathHint { get; }
      string Name { get; }
   }

   public interface ITemplate<T> : ITemplate 
      where T : CodeFirstMetadata<T>
   {
      string GetOutput(T metadata);

   
   }
}