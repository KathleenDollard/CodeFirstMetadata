using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirst.Common;
using RoslynDom;
using RoslynDom.Common;
using System.Reflection;

namespace CodeFirstMetadataTest.Roslyn
{
    // T = CodeFirstEventSource
    // class -> EventSource
    // method -> Event
    // parameter -> EventParameter
    public class RoslynMetadataParser<T> : IParser<T>
             where T : CodeFirstMetadata<T>
    {
        public IEnumerable<T> Parse(string str)
        {
            var ret = new List<T>();
            var root = RDomFactory.GetRootFromString(str);
            // TODO: Add optional namespace layer
            var classes = root.Classes;
            foreach (var cl in classes)
            {
                var item = ParseClass<T>(cl);
                if (item != null) ret.Add(item);
            }
            return ret;
        }

        private TTarget ParseClass<TTarget>(IClass cl)
        {
            // Properties of EventSource are attributes of cfcs
            // Methods of EventSource are Events (nested)
            // Properties of EventSource and nested types would be alternte child sets
            // TODO: Determine if base classes shoudl be searched
            var targetMethods = typeof(T).GetTypeInfo().DeclaredMethods;
            var sourceMethods = cl.Methods;
            foreach (var target in targetMethods)
            {
                //var item = ParseMethod<T>
            }
            throw new NotImplementedException();
        }
    }
}
