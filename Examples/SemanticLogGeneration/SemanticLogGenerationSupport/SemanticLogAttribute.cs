using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.SemanticLog
{
   public class UniqueNameAttribute : Attribute
   {
      public UniqueNameAttribute(string uniqueName)
      {
         UniqueName = uniqueName;
      }

      public string UniqueName { get; private set; }
   }
}
