// This file was generated, if you change it your changes are toast
// Generation was last done on 12/1/2014 12:00:00 AM using template DiagnosticTemplate

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace KathleensAnalyzer
{
   // TODO: Consider implementing other interfaces that implement IDiagnosticAnalyzer instead of or in addition to ISymbolAnalyzer

   [DiagnosticAnalyzer(LanguageNames.CSharp)]
   public class IfElseBraceDiagnosticCodeFirst : DiagnosticAnalyzer
   {
      public const string DiagnosticId = "KADGEN1001";
      internal const string Description = "Needs braces";
      internal const string MessageFormat = "{0} needs braces";
      internal const string Category = "Style";

      internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Description, MessageFormat, Category, DiagnosticSeverity.Error, true);

      public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

      public override void Initialize(AnalysisContext context)
      {
         context.RegisterSyntaxNodeAction(AnalyzeNodes, SyntaxKind.IfStatement, SyntaxKind.IfStatement);
      }

      private void AnalyzeNodes(SyntaxNodeAnalysisContext context)
      {
      
         var ifStatementSyntax = context.Node as  IfStatementSyntax;
         if (ifStatementSyntax != null
               && ( !ifStatementSyntax.Statement.IsKind(SyntaxKind.Block)))
         {
            Location loc =  ifStatementSyntax.IfKeyword.GetLocation();
            Diagnostic diagnostic = Diagnostic.Create(Rule, loc, "if statement");
            context.ReportDiagnostic(diagnostic);
         }

      
         var elseClauseSyntax = context.Node as  ElseClauseSyntax;
         if (elseClauseSyntax != null
               && ( !elseClauseSyntax.Statement.IsKind(SyntaxKind.Block)
                           && !elseClauseSyntax.Statement.IsKind(SyntaxKind.IfStatement)))
         {
            Location loc =  elseClauseSyntax.ElseKeyword.GetLocation();
            Diagnostic diagnostic = Diagnostic.Create(Rule, loc, "else statement");
            context.ReportDiagnostic(diagnostic);
         }

            }
   }
}