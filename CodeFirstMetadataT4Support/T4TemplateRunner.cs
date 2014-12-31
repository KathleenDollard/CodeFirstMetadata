using System;
using CodeFirst.Common;
using CodeFirst.TemplateSupport;

namespace CodeFirstMetadataT4Support
{
   public class T4TemplateRunner : TemplateRunnerBase
   {

      public T4TemplateRunner(ICodeFirstServiceProvider serviceProvider ) : base(serviceProvider) { }

      public T4TemplateRunner() : base(null) { }

      public override string CreateString<TMetadata, TTemplate>(TMetadata metadata, TTemplate template)
      {
         // TODO: Loosen the typing here as much as practical, interface?
         var typedTemplate = template as CodeFirstT4CSharpBase<TMetadata>;
         if (typedTemplate == null) throw new InvalidOperationException("Template must be of type CodeFirstT4CSharpBase<TMetadata>");
         var templateRunner = new RunPreprocessedT4<TMetadata>(metadata, typedTemplate);
         string output = templateRunner.Run();
         return output;
      }
   }

}