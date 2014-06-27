using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeFirst.Common;
using RoslynDom;
using RoslynDom.Common;

namespace CodeFirst
{
    public class CodeFirstMetadataLoader<T> : IMetadataLoader<T>
            where T : class
    {
        public T LoadFromFile(string fileName, string attributeIdentifier)
        {
            var root = RDomFactory.GetRootFromFile(fileName);
            if (ShouldRun(root, attributeIdentifier))
            {
                // TODO: We need to be more flexible and look for the desired element and then map from that level
                var cfNamespace = root.Namespaces.First();
                var returnType = typeof(T);
                var mapping = TargetMapping.DeriveMapping("root", "root", returnType.GetTypeInfo()) as TargetNamespaceMapping;
                var mapper = new CodeFirstMapper();
                var newObj = mapper.Map(mapping, cfNamespace);
                return newObj as T;
            }
            return null;
        }

        private bool ShouldRun(IRoot root, string attributeIdentifier)
        {
            var classes = (from x in root.RootClasses
                           from y in x.Attributes
                           where y.Name == attributeIdentifier
                           select true);
            return (classes.Count() > 0);
        }
    }
}
