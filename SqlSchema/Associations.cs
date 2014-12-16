using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSchema
{
   public class Associations
   {
      public Table PrimaryKeyTable { get; internal set; }
      public Table ForeignKeyTable { get; internal set; }
      public IEnumerable<Column> PrimaryKeyColumns { get; internal set; }
      public IEnumerable<Column> ForeignKeyColumns { get; internal set; }
   }
}
