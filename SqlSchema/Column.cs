using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSchema
{
    public class Column
    {
       public string Name { get; internal set; }
       public int Ordinal { get; internal set; }
       public bool AllowNulls { get; internal set; }
       public string SqlDataType { get; internal set; }
       public Type Type { get; internal set; }
       public int MaxLength { get; internal set; }
       public int NumericPrecision { get; internal set; }
       public int NumericScale { get; internal set; }
       public string TableName { get; internal set; }
       public string SchemaName { get; internal set; }
       public bool IsPrimaryKey { get; internal set; }

       public string DatabaseName { get; internal set; }
    }
}
