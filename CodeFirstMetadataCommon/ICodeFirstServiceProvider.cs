using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.Common
{
   public interface ICodeFirstServiceProvider : IServiceProvider
   {
      T GetService<T>(Func<T, bool> selector = null);
      IEnumerable<T> GetServices<T>(Func<T, bool> selector = null);
      IMetadataLoader<T> GetMetadataLoader<T>()
               where T : CodeFirstMetadata;
      IMapper GetMapper<T>()
               where T : CodeFirstMetadata;
      IMapper GetMapper(Type targetType);
      IMapper2 GetMapper2(Type targetType);
      void LoadIntoContainer<T>();
   }
}
