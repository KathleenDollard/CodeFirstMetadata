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
 
    public class CodeFirstEventSource : CodeFirstEventSourceBase
    {

        private string _name;
        public override string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_name))
                { return Namespace.Replace(".", "-") + "-" + ClassName; }
                return _name;
            }
            set
            { _name = value; }
        }

        public string EventSourceName
        {
            get
            {
                var tempName = this.Name.SubstringAfterLast("-");
                return tempName;
            }
        }

        public bool IncludesInterface
        {  get {  return this.ImplementedInterfaces.Count() > 0; } }

        public bool IsLocalized
        {  get {  return !string.IsNullOrWhiteSpace(this.LocalizationResources); } }

        public override bool ValidateAndUpdateCore()
        {
            var isOk = base.ValidateAndUpdateCore();
            if (isOk)
            { return CheckAndUpdateEventIds(); }
            return false;
        }

        /// <summary>
        /// This is a weird algorithm because it numbers implicit events from 
        /// the top, regardless of whether other events have event IDs. But
        /// while I wouldn't have chosen this, I think it's important to match
        /// EventSource implicit behavior exactly. 
        /// </summary>
        private bool CheckAndUpdateEventIds()
        {
            var i = 0;
            foreach (var evt in this.Events)
            {
                i++;
                if (evt.EventId == 0) evt.EventId = i;
            }
            // PERF: The following is an O<n2> algorithm, probably a better way
            var dupes = this.Events
                .Where(x => this.Events
                    .Any(y => (y != x) && x.EventId == y.EventId));
            return (dupes.Count() == 0);

        }
    }


}
