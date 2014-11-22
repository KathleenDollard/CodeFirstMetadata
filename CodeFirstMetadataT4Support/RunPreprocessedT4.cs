using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.Common
{
    public class RunPreprocessedT4<T>
            where T : CodeFirstMetadata<T>
    {
        private CodeFirstT4CSharpBase<T> _template;

        public RunPreprocessedT4(T source, CodeFirstT4CSharpBase<T> template)
        {
            _template = template;
            template.Meta = source;
        }

        public string Run()
        {
            var output = _template.TransformText();
            return output;
        }
    }
}
