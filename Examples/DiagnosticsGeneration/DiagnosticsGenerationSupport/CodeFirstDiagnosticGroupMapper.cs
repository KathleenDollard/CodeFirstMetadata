using CodeFirst;
using CodeFirst.Common;
using RoslynDom.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstMetadataTest.Diagnostic
{
   public class CodeFirstDiagnosticGroupMapper : CodeFirstMapper, IMapper<CodeFirstDiagnosticGroup >
   {
      public override IEnumerable<Type> SupportedTypes
      { get { return new[] { typeof(CodeFirstDiagnosticGroup) }; } }
   }
}
