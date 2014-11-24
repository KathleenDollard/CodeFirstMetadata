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
   public class CodeFirstAnalyzer : CodeFirstAnalyzerBase
   {
   }

   public class CodeFirstAnalyzer<T> : CodeFirstAnalyzer
      where T : SyntaxNode
     {
      public Type SyntaxNodeType { get; set; }
      public Func<T, bool> condition { get; set; }
      public Func<T, Location> getLocation { get; set; }
      public Func<T, SyntaxNode> makeNewNode { get; set; }
   }
}
