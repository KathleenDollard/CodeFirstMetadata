using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;
using System.Xml.Linq;
using RoslynDom.Common;

namespace CodeFirst.Common
{

   public class CodeFirstMetadataProperty<T> : CodeFirstMetadata<T> where T : CodeFirstMetadata<T>
   {
      public virtual string Name { get; set; }
      public virtual ScopeAccess ScopeAccess { get; set; }
      public virtual IReferencedType PropertyType { get; set; } // probably need more type info here

      public virtual bool CanGet { get; set; }
      public virtual bool CanSet { get; set; }

      public IEnumerable<IStatement> GetStatements { get; set; }
      public IEnumerable<IStatement> SetStatements { get; set; }
   }



}
