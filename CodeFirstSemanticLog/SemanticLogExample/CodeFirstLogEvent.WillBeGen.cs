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
    public abstract class CodeFirstLogEventBase : CodeFirstMetadataMethod<CodeFirstLogEvent>
    {
        private int _eventId;
        public virtual int EventId { get { return _eventId; }  set {_eventId = value; } }
        public virtual string[] CustomKeywords { get; set; }
        public virtual EventLevel Level { get; set; }
        public virtual string CustomOpcode { get; set; }
        public virtual string CustomTask { get; set; }
        public virtual byte Version { get; set; }
        public IEnumerable<CodeFirstLogEventParam> Parameters { get; private set; }

    }

}
