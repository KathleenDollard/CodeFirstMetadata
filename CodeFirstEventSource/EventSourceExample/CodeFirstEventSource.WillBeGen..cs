using System.Collections.Generic;
using System.Diagnostics.Tracing;
using CodeFirst.Common;

namespace CodeFirstMetadataTest.SemanticLog
{
    // TODO: Generate this base class based on expected attributes
    public abstract class CodeFirstEventSourceBase : CodeFirstMetadataClass<CodeFirstEventSource>
    {
        public CodeFirstEventSourceBase()
        {
            this.Events = new List<CodeFirstEvent>();
            this.AddDefiningAttribute(typeof(EventSourceAttribute));
        }

        public virtual string Name { get; set; }
        public virtual string LocalizationResources { get; set; }

        public IEnumerable<CodeFirstEvent> Events { get; private set; }

    }

}
