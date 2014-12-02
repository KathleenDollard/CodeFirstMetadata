using System.Collections.Generic;
using RoslynDom.Common;

namespace CodeFirst.Common
{

   public interface ICodeFirstMetadataProperty : ICodeFirstMetadata
   {
      string Name { get; set; }
      ScopeAccess ScopeAccess { get; set; }
      IReferencedType PropertyType { get; set; } // probably need more type info here

      bool CanGet { get; set; }
      bool CanSet { get; set; }

     IEnumerable<IStatement> GetStatements { get; set; }
     IEnumerable<IStatement> SetStatements { get; set; }
   }



}
