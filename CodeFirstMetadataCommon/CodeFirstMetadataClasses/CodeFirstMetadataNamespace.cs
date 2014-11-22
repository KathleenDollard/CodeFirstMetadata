using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;
using System.Xml.Linq;

namespace CodeFirst.Common
{
    public class CodeFirstMetadataNamespace<T> : CodeFirstMetadata<T> where T : CodeFirstMetadata<T>
    {
 
    }

}
