using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

      IMetadataLoader GetMetadataLoader(TypeInfo metadataTypeInfo);

      IMapper2<T> GetMapper2<T>()
              where T : CodeFirstMetadata;

      void LoadIntoContainer<T>();
   }
}
