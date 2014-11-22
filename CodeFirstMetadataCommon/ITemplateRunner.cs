using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.Common
{
    public interface ITemplateRunner
    {
        // TODO: Major mechanism should probably return a syntax tree
        string CreateString<TMetadata, TTemplate>(TMetadata metadata, TTemplate template)
                        where TMetadata : CodeFirstMetadata<TMetadata>;

    }
}
