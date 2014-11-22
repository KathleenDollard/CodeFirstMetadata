using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirst.Common;

namespace CodeFirstMetadataT4Support
{
    public class T4TemplateRunner : ITemplateRunner
    {
        public string CreateString<TMetadata, TTemplate>(TMetadata metadata, TTemplate template)
            where TMetadata : CodeFirstMetadata<TMetadata>
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