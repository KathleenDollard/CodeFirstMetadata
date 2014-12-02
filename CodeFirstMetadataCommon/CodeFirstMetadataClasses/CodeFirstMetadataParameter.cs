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
      public class CodeFirstMetadataParameter<T> : CodeFirstMetadata<T>, ICodeFirstMetadataParameter
            where T : CodeFirstMetadata<T>
    {
      public string TypeName { get; set; }
        public string Name { get; set; }
    }

}
