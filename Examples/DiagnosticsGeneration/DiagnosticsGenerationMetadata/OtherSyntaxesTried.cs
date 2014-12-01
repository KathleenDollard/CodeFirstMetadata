using CodeFirstMetadataTest.Diagnostic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace KathleensAnalyzer
{
   // I selected the other syntax becuase I felt it did the best job of guiding you through the process

   // *** Creating the class without the base class appraoch
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

   // Methods used by the lambdas in the constructor appear here, or 
   // methods that can be directly used as delegates,

   //*** Attempt at fluent syntax  
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
