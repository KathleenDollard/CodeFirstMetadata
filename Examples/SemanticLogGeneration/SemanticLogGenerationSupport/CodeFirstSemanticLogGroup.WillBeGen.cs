using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// TODO: Attempt to remove this line after generating base class
using System.Diagnostics.Tracing;
using CodeFirst.Common;

namespace CodeFirstMetadataTest.SemanticLog
{
    // TODO: Generate this base class based on expected attributes
    public abstract class CodeFirstSemanticLogGroupBase : CodeFirstMetadataNamespace<CodeFirstSemanticLogGroup>
    {
        public CodeFirstSemanticLogGroupBase()
        {
            this.SemanticLogs = new List<CodeFirstSemanticLog>();
        }

        public virtual string Name { get; set; }

        public IEnumerable<CodeFirstSemanticLog> SemanticLogs { get; private set; }

    }

}
