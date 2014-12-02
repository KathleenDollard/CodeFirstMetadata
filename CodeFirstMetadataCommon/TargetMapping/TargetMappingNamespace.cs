using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RoslynDom.Common;

namespace CodeFirst.Common
{
   public abstract class TargetNamespaceMapping : TargetChildMapping
   {
      internal TargetNamespaceMapping(string targetName, string prefix, TypeInfo typeInfo)
          : base(targetName, prefix, typeInfo, TargetLevel.Namespace)
      { }

      internal override bool CanHaveChildren
      {
         get { return true; }
      }

      public override IEnumerable<string> GetNamedProperties()
      {

         return base.GetNamedProperties().Union(new string[] { "FilePath" });
      }
   }

   public class TargetNamespaceMapping<T> : TargetNamespaceMapping
   {
      internal TargetNamespaceMapping(string targetName, string prefix, TypeInfo typeInfo)
          : base(targetName, prefix, typeInfo)
      { }

   }

}
