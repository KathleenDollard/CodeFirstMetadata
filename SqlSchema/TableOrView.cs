using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSchema
{
    public class TableOrView
    {
        public string DatabaseName { get; internal set; }
       public string SchemaName { get; internal set; }
       public string Name { get; internal set; }

       public IEnumerable<Column> Columns { get; internal set; }
    }
}
