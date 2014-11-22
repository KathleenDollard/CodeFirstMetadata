using CodeFirstAnalyzer;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Threading;

namespace KathleensAnalyzer
{
   //public class Anything
   //{
   //   public void MakeDiagnostics()
   //   {
   //      var ifElseBraces = new DiagnosticBase();
   //      ifElseBraces.Id = "KADGEN1001";
   //      ifElseBraces.Description = "Needs braces";
   //      ifElseBraces.MessageFormat = "{0} needs braces";
   //      ifElseBraces.Category = "Style";
   //      ifElseBraces.AddAnalyzer<IfStatementSyntax>(
   //         condition: x => !x.Statement.IsKind(SyntaxKind.Block),
   //         getLocation: x => x.IfKeyword.GetLocation(),
   //         messageArgs: "if statement");
   //      ifElseBraces.AddCodeFix<IfStatementSyntax>(
   //         makeNewNode: x => x.WithStatement(
   //                  SyntaxFactory.Block(x.Statement)));
   //      ifElseBraces.AddAnalyzer<ElseClauseSyntax>(
   //         condition: x => !x.Statement.IsKind(SyntaxKind.Block)
   //                        && !x.Statement.IsKind(SyntaxKind.IfStatement),
   //         getLocation: x => x.ElseKeyword.GetLocation(),
   //         messageArgs: "else statement");
   //      ifElseBraces.AddCodeFix<IfStatementSyntax>(
   //         makeNewNode: x => x.WithStatement(
   //                  SyntaxFactory.Block(x.Statement)));
   //   }
   //}
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

      // Methods used by the lambdas in the constructor appear here, or 
      // methods that can be directly used as delegates,

      //public IfElseBraceDiagnosticCodeFirst(bool dummy)
      //{
      //   var builder = new FluentDiagnosticBuilder()
      //      .WithId("KADGEN1001")
      //      .WithDescription("Needs braces")
      //      .WithMessageFormat("{0} needs braces")
      //      .WithCategory("Style")
      //      .WithAnalyzer(new FluentAnalyzerBuilder<IfStatementSyntax>()
      //            .WithCondition(x => !x.Statement.IsKind(SyntaxKind.Block))
      //            .WithLocationGetter(x => x.IfKeyword.GetLocation())
      //            .WithMessageArg("if statement"))
      //      .WithCodeFix(new FluentCodeFixBuilder<IfStatementSyntax>()
      //            .WithNewNodeMaker(x => x.WithStatement(
      //               SyntaxFactory.Block(x.Statement))))
      //      .WithAnalyzer(new FluentAnalyzerBuilder<ElseClauseSyntax>()
      //            .WithCondition(x => !x.Statement.IsKind(SyntaxKind.Block))
      //            .WithLocationGetter(x => x.ElseKeyword.GetLocation())
      //            .WithMessageArg("else clause"))
      //      .WithCodeFix(new FluentCodeFixBuilder<ElseClauseSyntax>()
      //            .WithNewNodeMaker(x => x.WithStatement(
      //               SyntaxFactory.Block(x.Statement))));
      //}
      ///// <summary>
      ///// [[ This sample is to show the approach to a more complex analyzer, not clear that it's needed, but I wanted to demo alternative ]]
      ///// </summary>
      //public class ElseBraceAnalyzer : KindAnalayzer<ElseClauseSyntax>
      //{
      //   public override void AnalyzeNode(ElseClauseSyntax node)
      //   {
      //      if (!node.Statement.IsKind(SyntaxKind.Block))
      //      {
      //         Report(node.ElseKeyword, "else clause");
      //      }
      //   }
      //}

      // Fixerisn't worked out yet
   }
}
