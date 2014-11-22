using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.Common
{
    [LanguageSpecific]
    public enum ScopeAccess
    {
        Public = 0,
        Private,
        Protected,
        Internal,
        ProtectedInternal
    }

}
