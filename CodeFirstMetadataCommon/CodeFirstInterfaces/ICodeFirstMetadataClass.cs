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
   public interface ICodeFirstMetadataClass : ICodeFirstMetadata
   {
      string Comments { get; set; }
      string Name { get; set; }
      string Namespace { get; set; }
      ScopeAccess ScopeAccess { get; set; }
      IEnumerable<string> ImplementedInterfaces { get; }
      IEnumerable<string> customMethods { get;  }
      IEnumerable<string> customProperties { get; }
   }
}
