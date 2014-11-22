using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirst.Common;

namespace CodeFirstMetadataTest.SemanticLog
{
    public abstract class CodeFirstEventParamBase : CodeFirstMetadataParameter<CodeFirstEventParam>, IHasParameterInfo
    {
        // Currently all the information required is held in the base classes (name and type)
    }
 
}
