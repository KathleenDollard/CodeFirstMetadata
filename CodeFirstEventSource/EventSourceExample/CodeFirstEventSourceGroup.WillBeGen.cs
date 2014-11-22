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
    public abstract class CodeFirstEventSourceGroupBase : CodeFirstMetadataNamespace<CodeFirstEventSourceGroup>
    {
        public CodeFirstEventSourceGroupBase()
        {
            this.EventSources = new List<CodeFirstEventSource>();
            this.AddDefiningAttribute(typeof(EventSourceAttribute));
        }

        public virtual string Name { get; set; }

        public IEnumerable<CodeFirstEventSource> EventSources { get; private set; }

    }

}
