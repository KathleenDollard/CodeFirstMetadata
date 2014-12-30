using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Tracing;
using System.Reflection;
using CodeFirst.Common;

namespace CodeFirstMetadataTest.SemanticLog
{
    // TODO: Generate the base class on this pattern
    public abstract class CodeFirstEventBase : CodeFirstMetadataMethod<CodeFirstEvent>
    {
        public virtual int EventId { get; set; }
        public virtual EventKeywords Keywords { get; set; }
        public virtual string[] CustomKeywords { get; set; }
        public virtual EventLevel Level { get; set; }
        public virtual EventOpcode Opcode { get; set; }
        public virtual string CustomOpcode { get; set; }
        public virtual EventTask Task { get; set; }
        public virtual string CustomTask { get; set; }
        public virtual byte Version { get; set; }
        public IEnumerable<CodeFirstEventParam> Parameters { get; private set; }

    }

}
