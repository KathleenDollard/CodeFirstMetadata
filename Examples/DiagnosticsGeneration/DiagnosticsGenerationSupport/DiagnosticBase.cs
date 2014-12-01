using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CodeFirstMetadataTest.Diagnostic
{
   [DiagnosticAndCodeFix]
   public class DiagnosticAndCodeFixBase
   {
      public string Id { get; set; }
      public string Description { get; set; }
      public string MessageFormat { get; set; }
      public string Category { get; set; }
      protected static Microsoft.CodeAnalysis.Diagnostic Report(Location location, params string[] messageArgs) { return null; }
      protected static Microsoft.CodeAnalysis.Diagnostic Report(SyntaxToken token, params string[] messageArgs) { return null; }

      public IList<CodeFirstAnalyzer> Analyzers { get; set; }

      public void AddAnalyzer<TSyntaxNode>(SyntaxKind syntaxKind, Func<TSyntaxNode, bool> condition, Func<TSyntaxNode, Location> getLocation,
          string messageArg)
         where TSyntaxNode : SyntaxNode 
      { }
      public void AddCodeFix<TSyntaxNode>(Func<TSyntaxNode, SyntaxNode> makeNewNode, bool skipFormatting = false, params string[] messageArgs)
         where TSyntaxNode : SyntaxNode
      { }
      //protected void AddAnalyzer<T>(Func<T, bool> condition, Func<T, Location> location, string messageArg) { }

      //protected void SimpleFix<T>(Func<T, SyntaxNode> newNode, bool skipFormatting = false, params string[] messageArgs) { }
      //protected void SimpleFix<T>(Func<T, SyntaxNode> newNode, bool skipFormatting = false, string messageArg = "") { }

      //public abstract class KindAnalayzer<T>
      //   where T : CSharpSyntaxNode
      //{
      //   public List<string> MessageArgs { get; private set; }
      //   public Location Location { get; protected set; }
      //   public virtual void AnalyzeNode(T node) { }
      //   public virtual void AnalyzeNode(T node, SemanticModel semanticModel) { }
      //   public virtual void AnalyzeNode(T node, SemanticModel semanticModel,
      //      Action<Diagnostic> addDiagnostic, AnalyzerOptions options,
      //      CancellationToken cancellationToken)
      //   { }
      //}
      //public abstract class KindFixer<T>
      //  where T : CSharpSyntaxNode
      //{
      //   public abstract IEnumerable<CodeAction> GetFixesAsync(Document document,
      //      TextSpan span, IEnumerable<Diagnostic> diagnostics,
      //      CancellationToken cancellationToken);
      //}
   }

   public class FluentDiagnosticBuilder
   {
      public FluentDiagnosticBuilder WithId(string Id) { return this; }
      public FluentDiagnosticBuilder WithDescription(string description) { return this; }
      public FluentDiagnosticBuilder WithMessageFormat(string messageFormat) { return this; }
      public FluentDiagnosticBuilder WithCategory(string Category) { return this; }

      public FluentDiagnosticBuilder WithAnalyzer<T>(FluentAnalyzerBuilder<T> analyzer)
            where T : SyntaxNode
      { return this; }
      public FluentDiagnosticBuilder WithCodeFix<T>(FluentCodeFixBuilder<T> fixer)
            where T : SyntaxNode
      { return this; }
   }

   public class FluentAnalyzerBuilder<T>
      where T : SyntaxNode
   {
      public FluentAnalyzerBuilder<T> WithCondition(Func<T, bool> condition) { return this; }
      public FluentAnalyzerBuilder<T> WithLocationGetter(Func<T, Location> getLocation) { return this; }
      public FluentAnalyzerBuilder<T> WithMessageArg(string arg) { return this; }
   }

   public class FluentCodeFixBuilder<T>
      where T : SyntaxNode
   {
      public FluentCodeFixBuilder<T> SkipFormatting() { return this; }
      public FluentCodeFixBuilder<T> DoFormatting() { return this; }
      public FluentCodeFixBuilder<T> WithNewNodeMaker(Func<T, SyntaxNode> makeNewNode) { return this; }
   }
}
