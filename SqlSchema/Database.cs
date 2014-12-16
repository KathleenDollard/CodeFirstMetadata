using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSchema
{
    public class Database
    {
       public string Name { get; internal set; }

       public IEnumerable<Table> Tables { get; internal set; }
       public IEnumerable<View> Views { get; internal set; }
    }
}
