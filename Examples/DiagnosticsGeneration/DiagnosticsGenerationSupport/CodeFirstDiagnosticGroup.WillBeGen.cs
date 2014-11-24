using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// TODO: Attempt to remove this line after generating base class
using System.Diagnostics.Tracing;
using CodeFirst.Common;
using System.ComponentModel;

namespace CodeFirstMetadataTest.Diagnostic
{
   // TODO: Generate this base class based on expected attributes
   public class CodeFirstDiagnosticGroupBase : CodeFirstMetadataNamespace<CodeFirstDiagnosticGroup>
    {
        public CodeFirstDiagnosticGroupBase()
        {
            this.Diagnostics = new List<CodeFirstDiagnostic>();
        }

        public virtual string Name { get; set; }

        public IEnumerable<CodeFirstDiagnostic> Diagnostics { get; private set; }


    }

}
