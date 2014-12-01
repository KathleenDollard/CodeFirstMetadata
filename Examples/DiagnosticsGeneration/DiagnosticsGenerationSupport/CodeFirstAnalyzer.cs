using RoslynDom.Common;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeFirstMetadataTest.Diagnostic
{
   public class CodeFirstAnalyzer : CodeFirstAnalyzerBase
   {
      // Since the integer value is used, this data should not be saved across versions of Roslyn
      public string SyntaxKind { get; set; }
      public ILambdaExpression Condition { get; set; }
      public ILambdaExpression GetLocation { get; set; }
      public string MessageArg { get; set; }

      private string variableName;
      public string VariableName
      {
         get
         {
            return variableName == null
                      ? StringUtilities.CamelCase(PropertyType.Name)
                      : variableName;
         }
         set { variableName = value; }
      }

      public string ConditionString
      {
         get { return StringifyLambda(Condition, VariableName); }
      }

      public string GetLocationString
      {
         get { return StringifyLambda(GetLocation, VariableName); }
      }


      private string StringifyLambda(ILambdaExpression lambda, string paramReplace)
      {
         var singleLambda = lambda as ILambdaSingleExpression;
         if (singleLambda == null) { throw new NotImplementedException(); }
         var exp = singleLambda.InitialExpressionString.SubstringAfter("=>");
         var argName = singleLambda.Parameters.First().Name;
         // TODO: This should be at least a RegEx that will support word boundaries
         var regex = new Regex(@"\b" + argName + @"\b");
         var output = regex.Replace(exp, paramReplace);
         return output;
      }
   }
}
