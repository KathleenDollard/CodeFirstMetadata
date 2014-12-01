﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 14.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace DiagnosticsGenerationTemplates
{
    using System.Linq;
    using CodeFirst.Common;
    using CodeFirstMetadataTest.Diagnostic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\DiagnosticsGeneration\DiagnosticsGenerationTemplates\DiagnosticTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "14.0.0.0")]
    public partial class DiagnosticTemplate : CodeFirstT4CSharpBase<CodeFirstDiagnostic>
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            
            #line 9 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\DiagnosticsGeneration\DiagnosticsGenerationTemplates\DiagnosticTemplate.tt"
 
	OutputGenerationWarning();
    var classMeta = Meta as CodeFirstDiagnostic;
 
            
            #line default
            #line hidden
            this.Write(@"using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ");
            
            #line 22 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\DiagnosticsGeneration\DiagnosticsGenerationTemplates\DiagnosticTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Namespace));
            
            #line default
            #line hidden
            this.Write("\r\n{\r\n   // TODO: Consider implementing other interfaces that implement IDiagnosti" +
                    "cAnalyzer instead of or in addition to ISymbolAnalyzer\r\n\r\n   [DiagnosticAnalyzer" +
                    "(LanguageNames.CSharp)]\r\n   public class ");
            
            #line 27 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\DiagnosticsGeneration\DiagnosticsGenerationTemplates\DiagnosticTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write(" : DiagnosticAnalyzer\r\n   {\r\n      public const string DiagnosticId = ");
            
            #line 29 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\DiagnosticsGeneration\DiagnosticsGenerationTemplates\DiagnosticTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Id));
            
            #line default
            #line hidden
            this.Write(";\r\n      internal const string Description = ");
            
            #line 30 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\DiagnosticsGeneration\DiagnosticsGenerationTemplates\DiagnosticTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Description));
            
            #line default
            #line hidden
            this.Write(";\r\n      internal const string MessageFormat = ");
            
            #line 31 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\DiagnosticsGeneration\DiagnosticsGenerationTemplates\DiagnosticTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.MessageFormat));
            
            #line default
            #line hidden
            this.Write(";\r\n      internal const string Category = ");
            
            #line 32 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\DiagnosticsGeneration\DiagnosticsGenerationTemplates\DiagnosticTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Category));
            
            #line default
            #line hidden
            this.Write(@";

      internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Description, MessageFormat, Category, DiagnosticSeverity.Error, true);

      public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

      public override void Initialize(AnalysisContext context)
      {
         context.RegisterSyntaxNodeAction(AnalyzeNodes, ");
            
            #line 40 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\DiagnosticsGeneration\DiagnosticsGenerationTemplates\DiagnosticTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(String.Join(", ", Meta.Analyzers.Select(x=>x.SyntaxKind))));
            
            #line default
            #line hidden
            this.Write(");\r\n      }\r\n\r\n      private void AnalyzeNodes(SyntaxNodeAnalysisContext context)" +
                    "\r\n      {\r\n      ");
            
            #line 45 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\DiagnosticsGeneration\DiagnosticsGenerationTemplates\DiagnosticTemplate.tt"
 foreach(var analyzer in Meta.Analyzers) 
      {
      
            
            #line default
            #line hidden
            this.Write("\r\n         var ");
            
            #line 49 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\DiagnosticsGeneration\DiagnosticsGenerationTemplates\DiagnosticTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(analyzer.VariableName));
            
            #line default
            #line hidden
            this.Write(" = context.Node as  ");
            
            #line 49 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\DiagnosticsGeneration\DiagnosticsGenerationTemplates\DiagnosticTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(analyzer.PropertyType.Name));
            
            #line default
            #line hidden
            this.Write(";\r\n         if (");
            
            #line 50 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\DiagnosticsGeneration\DiagnosticsGenerationTemplates\DiagnosticTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(analyzer.VariableName));
            
            #line default
            #line hidden
            this.Write(" != null\r\n               && (");
            
            #line 51 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\DiagnosticsGeneration\DiagnosticsGenerationTemplates\DiagnosticTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(analyzer.ConditionString));
            
            #line default
            #line hidden
            this.Write("))\r\n         {\r\n            Location loc = ");
            
            #line 53 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\DiagnosticsGeneration\DiagnosticsGenerationTemplates\DiagnosticTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(analyzer.GetLocationString));
            
            #line default
            #line hidden
            this.Write(";\r\n            Diagnostic diagnostic = Diagnostic.Create(Rule, loc, ");
            
            #line 54 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\DiagnosticsGeneration\DiagnosticsGenerationTemplates\DiagnosticTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(analyzer.MessageArg));
            
            #line default
            #line hidden
            this.Write(");\r\n            context.ReportDiagnostic(diagnostic);\r\n         }\r\n\r\n      ");
            
            #line 58 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\DiagnosticsGeneration\DiagnosticsGenerationTemplates\DiagnosticTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("      }\r\n   }\r\n}");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
}
