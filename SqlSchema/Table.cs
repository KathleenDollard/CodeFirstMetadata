using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSchema
{
    public class Table : TableOrView
    {
       public IEnumerable<Associations> ForiegnKeys { get; internal set; }
    }
}
