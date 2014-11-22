using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Tracing;
using System.Reflection;
using CodeFirst.Common;
using Microsoft.CodeAnalysis;

namespace CodeFirstMetadataTest.Diagnostic
{
 

    public class CodeFirstAnalyzer<T> : CodeFirstAnalyzerBase<T>
      where T : SyntaxNode
    {
    }
}
