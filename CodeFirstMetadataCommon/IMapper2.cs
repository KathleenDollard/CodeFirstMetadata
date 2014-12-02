using RoslynDom.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.Common
{
   public interface IMapper2
   {
      IEnumerable<Type> SupportedTypes { get; }
   }

   public interface IMapper2<out T> : IMapper2
      where T : CodeFirstMetadata
   {
      T Map(TargetChildMapping mapping, IDom source, CodeFirstMetadata parent);
      IEnumerable<T> MapList(TargetChildMapping mapping, IEnumerable<IDom> sourceList, CodeFirstMetadata parent);
      IEnumerable<T> MapFromConstructor(TargetChildMapping mapping, IDom source, CodeFirstMetadata parent);
   }
}
