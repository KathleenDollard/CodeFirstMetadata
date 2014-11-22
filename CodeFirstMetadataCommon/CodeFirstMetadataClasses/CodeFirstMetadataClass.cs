using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;
using System.Xml.Linq;

namespace CodeFirst.Common
{
   public class CodeFirstMetadataClass<T> : CodeFirstMetadata<T> where T : CodeFirstMetadata<T>
   {
      public CodeFirstMetadataClass()
      {
         ImplementedInterfaces = new List<string>();
      }
      public virtual string Comments { get; set; }
      public virtual string Name { get; set; }
      public virtual string Namespace { get; set; }
      public virtual ScopeAccess ScopeAccess { get; set; }
      public IEnumerable<string> ImplementedInterfaces { get; private set; }

      public IEnumerable<string> customMethods { get; private set; }
      public IEnumerable<string> customProperties { get; private set; }
   }
}
