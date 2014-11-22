// This file was generated, if you change it your changes are toast
// Generation was last done on 11/13/2014 12:00:00 AM using template DiagnosticTemplate

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.Formatting;

namespace KathleensAnalyzer
{
   [ExportCodeFixProvider(DiagnosticAnalyzer.DiagnosticId, LanguageNames.CSharp)]
   public class CodeFixProvider : ICodeFixProvider
   {
      public IEnumerable<string> GetFixableDiagnosticIds()
      {
         return new[] { DiagnosticAnalyzer.DiagnosticId };
      }

      public async Task<IEnumerable<CodeAction>> GetFixesAsync(Document document, 
         TextSpan span, 
         IEnumerable<Diagnostic> diagnostics, 
         CancellationToken cancellationToken)
      {
         try {
            var root = await document.GetSyntaxRootAsync(cancellationToken)
                           .ConfigureAwait(false);
            var token = root.FindToken(span.Start);
            var ifStatement = token.Parent as IfStatementSyntax;
            var newIfStatement = ifStatement.WithStatement(
                           SyntaxFactory.Block(ifStatement.Statement))
                           .WithAdditionalAnnotations(Formatter.Annotation);
            var newRoot = root.ReplaceNode(ifStatement, newIfStatement);
            var newDoc = document.WithSyntaxRoot(newRoot);
            return new[] { CodeAction.Create("Add braces", newDoc) };
         }
         catch
         {
            throw;
         }
      }
   }
}