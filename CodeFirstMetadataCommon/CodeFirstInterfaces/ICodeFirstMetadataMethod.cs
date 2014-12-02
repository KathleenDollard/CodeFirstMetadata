using System.Collections.Generic;
using RoslynDom.Common;

namespace CodeFirst.Common
{
   public interface ICodeFirstMetadataMethod : ICodeFirstMetadata
   {
      string Name { get; set; }
      ScopeAccess ScopeAccess { get; set; }
      IEnumerable<IStatement> Statements { get; set; }
   }

}
