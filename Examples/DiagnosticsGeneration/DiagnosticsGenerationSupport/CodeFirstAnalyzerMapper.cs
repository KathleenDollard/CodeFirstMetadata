using CodeFirst;
using CodeFirst.Common;
using RoslynDom.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstMetadataTest.Diagnostic
{
   public class CodeFirstAnalyzerMapper : CodeFirstMapper, IMapper<CodeFirstAnalyzer>
   {
      public override IEnumerable<Type> SupportedTypes
      { get { return new[] { typeof(CodeFirstAnalyzer) }; } }

      protected override void SetNewItemProperties<TLocal>(TLocal newItem, IInvocationStatement invocation)
      {
         var item = newItem as CodeFirstAnalyzer;
         var conditionArg = invocation.Arguments.Where(x => RoslynDom.Common.StringUtilities.PascalCase(x.Name) == nameof(CodeFirstAnalyzer.Condition)).FirstOrDefault();
         item.Condition = conditionArg.ValueExpression as ILambdaExpression;
         var getLocationArg = invocation.Arguments.Where(x => RoslynDom.Common.StringUtilities.PascalCase(x.Name) == nameof(CodeFirstAnalyzer.GetLocation)).FirstOrDefault();
         item.GetLocation = getLocationArg.ValueExpression as ILambdaExpression;
         var messageArgs = invocation.Arguments.Where(x => RoslynDom.Common.StringUtilities.PascalCase(x.Name) == nameof(CodeFirstAnalyzer.MessageArg)).FirstOrDefault();
         //  item.MessageArgs = messageArgs.ValueExpression as ILambdaExpression;
         var syntaxKindArg = invocation.Arguments.Where(x => RoslynDom.Common.StringUtilities.PascalCase(x.Name) == nameof(CodeFirstAnalyzer.SyntaxKind)).FirstOrDefault();
         //item.SyntaxKind = (int)syntaxKindArg.ValueExpression.ToString() ;
      }
   }
}
