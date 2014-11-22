﻿using CodeFirstAnalyzer;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace KathleensAnalyzer
{
   public class IfElseBraceDiagnosticCodeFirst : DiagnosticAndCodeFixBase
   {
      public IfElseBraceDiagnosticCodeFirst()
      {
         base.
         Id = "KADGEN1001";
         Description = "Needs braces";
         MessageFormat = "{0} needs braces";
         Category = "Style";
         AddAnalyzer<IfStatementSyntax>(
            condition: x => !x.Statement.IsKind(SyntaxKind.Block),
            getLocation: x => x.IfKeyword.GetLocation(),
            messageArgs: "if statement");
         AddCodeFix<IfStatementSyntax>(
            makeNewNode: x => x.WithStatement(
                     SyntaxFactory.Block(x.Statement)));
         AddAnalyzer<ElseClauseSyntax>(
           condition: x => !x.Statement.IsKind(SyntaxKind.Block)
                           && !x.Statement.IsKind(SyntaxKind.IfStatement),
           getLocation: x => x.ElseKeyword.GetLocation(),
           messageArgs: "else statement");
         AddCodeFix<ElseClauseSyntax>(
           makeNewNode: GetNewElseNode);
      }

      private SyntaxNode GetNewElseNode(ElseClauseSyntax  elseClause)
      {
         return elseClause.WithStatement(SyntaxFactory.Block(elseClause.Statement));
      }
   }
}
