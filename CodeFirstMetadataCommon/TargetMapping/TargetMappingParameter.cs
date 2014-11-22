using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RoslynDom.Common;

namespace CodeFirst.Common
{
    public class TargetParameterMapping : TargetChildMapping
    {
        internal TargetParameterMapping(string targetName, string prefix, TypeInfo typeInfo)
            : base(targetName, prefix, typeInfo, TargetLevel.Parameter)
        { }

        internal override bool CanHaveChildren
        {
            get { return false; }
        }

        public override IEnumerable<string> GetNamedProperties()
        {
            
            return base.GetNamedProperties().Union(new string[]  { "TypeName" });
        }

    }

    public class TargetParameterMapping<T> : TargetParameterMapping
    {
        internal TargetParameterMapping(string targetName, string prefix, TypeInfo typeInfo)
        : base(targetName, prefix, typeInfo)
        { }
    }

}
