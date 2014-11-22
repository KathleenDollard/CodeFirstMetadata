// This file was generated, if you change it your changes are toast
// Generation was last done on 11/22/2014 12:00:00 AM using template DiagnosticTemplate

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
   public class DiagnosticAnalyzer : ISyntaxNodeAnalyzer<SyntaxKind>
   {
      public const string DiagnosticId = "";
      internal const string Description = "";
      internal const string MessageFormat = "";
      internal const string Category = "";

      internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Description, MessageFormat, Category, DiagnosticSeverity.Error, true);

      public ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

      public ImmutableArray<SyntaxKind> SyntaxKindsOfInterest
      {
         get
         {
            return ImmutableArray.Create(SyntaxKind.IfStatement, SyntaxKind.ElseClause);
         }
      }

      public void AnalyzeNode(SyntaxNode node, 
         SemanticModel semanticModel, 
         Action<Diagnostic> addDiagnostic, 
         AnalyzerOptions options, 
         CancellationToken cancellationToken)
      {
            }

      private void Report(Location location, string v, Action<Diagnostic> addDiagnostic)
      {
         var diagnostic = Diagnostic.Create(Rule, location, v);
         addDiagnostic(diagnostic);
      }
   }
}