using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Tracing;
using System.Reflection;
using CodeFirst.Common;

namespace CodeFirstMetadataTest.Diagnostic
{
    // TODO: Generate the base class on this pattern
    public class CodeFirstFixBase : CodeFirstMetadataProperty<CodeFirstFix>
    {
      public virtual string Message { get; set; }
   }

}
