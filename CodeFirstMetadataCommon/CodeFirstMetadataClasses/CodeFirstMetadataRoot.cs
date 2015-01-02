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
   public class CodeFirstMetadataNamespace<T> : CodeFirstMetadata<T>, ICodeFirstMetadataNamespace, ICodeFirstEntry
         where T : CodeFirstMetadata<T>
   {
      // By convention, one namespace per file
      private string filePath;
      public string FilePath
      {
         get { return filePath; }
         set { filePath = value; }
      }

      private string _attributeId;
      public string AttributeId
      {
         get
         { return _attributeId; }
         protected set
         { _attributeId = value; }
      }
   }
}
