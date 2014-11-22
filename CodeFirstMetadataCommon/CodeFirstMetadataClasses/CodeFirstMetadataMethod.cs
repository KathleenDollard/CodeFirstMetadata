using System.Collections.Generic;
using RoslynDom.Common;

namespace CodeFirst.Common
{
   public class CodeFirstMetadataMethod<T> : CodeFirstMetadata<T> where T : CodeFirstMetadata<T>
   {
      public virtual string Name { get; set; }
      public virtual ScopeAccess ScopeAccess { get; set; }
      public IEnumerable<IStatement> Statements { get; set; }
   }

}
