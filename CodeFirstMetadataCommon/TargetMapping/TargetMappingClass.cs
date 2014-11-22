using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RoslynDom.Common;

namespace CodeFirst.Common
{
 
    public abstract class TargetClassMapping : TargetChildMapping
    {
        internal TargetClassMapping(string targetName, string prefix, TypeInfo typeInfo)
            : base(targetName, prefix, typeInfo, TargetLevel.Class)
        {        }

        internal override bool CanHaveChildren
        {
            get { return true; }
        }

          public override IEnumerable<string> GetNamedProperties()
        {
            
            return base.GetNamedProperties().Union(new string[]  {
                    "ClassName", "Namespace",
                    "Comments", "ScopeAccess",
                    "ImplementedInterfaces" });
        }
    }

    public class TargetClassMapping<T> : TargetClassMapping
    {
        internal TargetClassMapping(string targetName, string prefix, TypeInfo typeInfo)
        : base(targetName, prefix, typeInfo)
        { }
    }


}
