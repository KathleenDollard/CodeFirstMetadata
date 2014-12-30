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
    public class CodeFirstClassGroupBase : CodeFirstMetadataNamespace<CodeFirstClassGroup>
    {
        public CodeFirstClassGroupBase()
        {
            this.Classes = new List<CodeFirstClass>();
            this.AddDefiningAttribute(typeof(NotifyParentPropertyAttribute ));
        }

        public virtual string Name { get; set; }

        public IEnumerable<CodeFirstClass> Classes { get; private set; }


    }

}
