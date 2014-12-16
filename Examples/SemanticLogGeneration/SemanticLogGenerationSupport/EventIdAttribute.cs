using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.SemanticLog
{
   public class EventIdAttribute : Attribute
   {
      public EventIdAttribute (int id)
      { Id = id; }

      public int Id { get; private set; }
   }
}
