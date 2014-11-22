using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RoslynDom.Common;

namespace CodeFirst.Common
{

    public abstract class TargetPropertyMapping : TargetChildMapping
    {
 
        internal TargetPropertyMapping(string targetName, string prefix, TypeInfo typeInfo)
            : base(targetName, prefix, typeInfo, TargetLevel.Class)
        {        }

        internal override bool CanHaveChildren
        {
            get { return true; }
        }

        public override IEnumerable<string> GetNamedProperties()
        {
            return base.GetNamedProperties().Union(new string[]
            { "ScopeAccess", "CanGet", "CanSet" , "PropertyType" });
        }
    }

    public class TargetPropertyMapping<T> : TargetPropertyMapping
    {
        internal TargetPropertyMapping(string targetName, string prefix, TypeInfo typeInfo)
        : base(targetName, prefix, typeInfo)
        { }
    }


}
