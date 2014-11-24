using System.Collections.Generic;
using CodeFirst.Common;

namespace CodeFirstMetadataTest.SemanticLog
{
    // TODO: Generate this base class based on expected attributes
    public abstract class CodeFirstSemanticLogBase : CodeFirstMetadataClass<CodeFirstSemanticLog>
    {
        public CodeFirstSemanticLogBase()
        {
            Events = new List<CodeFirstLogEvent>();
        }

        public virtual string UniqueName { get; set; }
        public virtual string LocalizationResources { get; set; }

        public IEnumerable<CodeFirstLogEvent> Events { get; private set; }

    }

}
