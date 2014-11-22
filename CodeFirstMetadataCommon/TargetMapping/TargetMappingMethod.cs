using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RoslynDom.Common;

namespace CodeFirst.Common
{
     public class TargetMethodMapping : TargetChildMapping
    {
        internal TargetMethodMapping(string targetName, string prefix, TypeInfo typeInfo)
            : base(targetName, prefix, typeInfo, TargetLevel.Method)
        {    }
        internal override bool CanHaveChildren
        {
            get { return true; }
        }

        public override IEnumerable<string> GetNamedProperties()
        {

            return base.GetNamedProperties().Union(new string[] { "Comments", "ScopeAccess" });
        }
    }

    public class TargetMethodMapping<T> : TargetMethodMapping
    {
        internal TargetMethodMapping(string targetName, string prefix, TypeInfo typeInfo)
        : base(targetName, prefix, typeInfo)
        { }
    }

}
