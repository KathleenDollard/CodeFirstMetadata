using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// TODO: Attempt to remove this line after generating base class
using System.Diagnostics.Tracing;
using CodeFirst.Common;
using System.ComponentModel;

namespace CodeFirstMetadataTest.PropertyChanged
{
    // TODO: Generate this base class based on expected attributes
    public class CodeFirstClassBase : CodeFirstMetadataClass<CodeFirstClass>
    {
        public CodeFirstClassBase()
        {
            this.Properties = new List<CodeFirstProperty>();
            this.AddDefiningAttribute(typeof(NotifyPropertyChangedAttribute));
        }

        public IEnumerable<CodeFirstProperty> Properties { get; private set; }

    }

}
