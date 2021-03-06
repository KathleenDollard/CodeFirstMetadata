﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 14.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace SemanticLogGenerationTemplates
{
    using System.Linq;
    using CodeFirst.Common;
    using CodeFirstMetadataTest.SemanticLog;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "14.0.0.0")]
    public partial class EventSourceTemplate : CodeFirstT4CSharpBase<CodeFirstSemanticLog>
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            
            #line 9 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceTemplate.tt"
 
	OutputGenerationWarning();
  	var apiClassName = Meta.Name;
  	var eventSourceClassName = Meta.Name;
    var eventSourceMeta = Meta as CodeFirstSemanticLog; 
    var loggerHandled = false; 
    var includeInnerLogger = false;
    var eventSourceScope = "public";
 
            
            #line default
            #line hidden
            this.Write("using System;\r\nusing System.Diagnostics.Tracing;\r\n\r\n// UniqueName: ");
            
            #line 21 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.UniqueName));
            
            #line default
            #line hidden
            this.Write("\r\n\r\n");
            
            #line 23 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceTemplate.tt"
 OutputNamespaceOpen(); 
            
            #line default
            #line hidden
            
            #line 24 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceTemplate.tt"
 if (Meta.IncludesInterface) { 
            
            #line default
            #line hidden
            
            #line 1 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceWithInterface.t4"
      OutputXmlComments(Meta); 
            
            #line default
            #line hidden
            this.Write("public partial class ");
            
            #line 2 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceWithInterface.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(apiClassName));
            
            #line default
            #line hidden
            this.Write(" : ");
            
            #line 2 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceWithInterface.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.ImplementedInterfaces.First()));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 3 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceWithInterface.t4"
 
this.PushBracket(); 
includeInnerLogger = true;
eventSourceScope = "private";
eventSourceClassName = "Nested" + apiClassName;

            
            #line default
            #line hidden
            this.Write("\r\n");
            this.Write("#region Standard class stuff\r\n// Private constructor blocks direct instantiation " +
                    "of class\r\nprivate ");
            
            #line 4 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write("() {}\r\n\r\n// Readonly access to cached, lazily created singleton instance\r\nprivate" +
                    " static readonly Lazy<");
            
            #line 7 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write("> _lazyLog = \r\n        new Lazy<");
            
            #line 8 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write(">(() => new ");
            
            #line 8 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write("()); \r\npublic static ");
            
            #line 9 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write(" Log\r\n{\r\n\tget { return _lazyLog.Value; }\r\n}\r\n// Readonly access to  private cache" +
                    "d, lazily created singleton inner class instance\r\nprivate static readonly Lazy<");
            
            #line 14 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write("> _lazyInnerlog = \r\n        new Lazy<");
            
            #line 15 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write(">(() => new ");
            
            #line 15 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write("());\r\nprivate static ");
            
            #line 16 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write(" innerLog\r\n{\r\n\tget { return _lazyInnerlog.Value; }\r\n}\r\n#endregion\r\n");
            this.Write("\r\n\r\n");
            
            #line 12 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceWithInterface.t4"

loggerHandled = true;
foreach(var evt in Meta.Events)
{
    OutputXmlComments(evt);

            
            #line default
            #line hidden
            
            #line 18 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceWithInterface.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(MethodDeclaration(evt.Name, evt.ScopeAccess, evt.Parameters)));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 21 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceWithInterface.t4"
  this.PushBracket(); 
            
            #line default
            #line hidden
            this.Write("  \r\ninnerLog.");
            
            #line 22 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceWithInterface.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(evt.Name));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 22 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceWithInterface.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(ParametersFrom(evt.Parameters)));
            
            #line default
            #line hidden
            this.Write(");\r\n");
            
            #line 23 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceWithInterface.t4"
 this.PopBracket();
} 


            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 1 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
  OutputClassPrefix(); 
            
            #line default
            #line hidden
            this.Write("[EventSource(Name = \"");
            
            #line 2 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.UniqueName));
            
            #line default
            #line hidden
            this.Write("\")]\r\n");
            
            #line 3 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(eventSourceScope));
            
            #line default
            #line hidden
            this.Write(" sealed partial class ");
            
            #line 3 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(eventSourceClassName));
            
            #line default
            #line hidden
            this.Write(" : EventSource\r\n");
            
            #line 4 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
 this.PushBracket(); 
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 6 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
 if (!loggerHandled) { 
            
            #line default
            #line hidden
            this.Write("#region Standard class stuff\r\n// Private constructor blocks direct instantiation " +
                    "of class\r\nprivate ");
            
            #line 4 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write("() {}\r\n\r\n// Readonly access to cached, lazily created singleton instance\r\nprivate" +
                    " static readonly Lazy<");
            
            #line 7 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write("> _lazyLog = \r\n        new Lazy<");
            
            #line 8 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write(">(() => new ");
            
            #line 8 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write("()); \r\npublic static ");
            
            #line 9 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write(" Log\r\n{\r\n\tget { return _lazyLog.Value; }\r\n}\r\n// Readonly access to  private cache" +
                    "d, lazily created singleton inner class instance\r\nprivate static readonly Lazy<");
            
            #line 14 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write("> _lazyInnerlog = \r\n        new Lazy<");
            
            #line 15 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write(">(() => new ");
            
            #line 15 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write("());\r\nprivate static ");
            
            #line 16 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write(" innerLog\r\n{\r\n\tget { return _lazyInnerlog.Value; }\r\n}\r\n#endregion\r\n");
            this.Write("\r\n");
            
            #line 8 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
 } 
            
            #line default
            #line hidden
            this.Write("\r\n#region Your trace event methods\r\n\r\n");
            
            #line 12 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"

foreach(var evt in Meta.Events)
{
    OutputXmlComments(evt);
	OutputAttributes(evt);

            
            #line default
            #line hidden
            this.Write("[Event(");
            
            #line 18 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(evt.EventId));
            
            #line default
            #line hidden
            this.Write(")]\r\n");
            
            #line 19 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(MethodDeclaration(evt.Name, evt.ScopeAccess, evt.Parameters)));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 22 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
  this.PushBracket(); 
            
            #line default
            #line hidden
            this.Write("   \r\nif (IsEnabled()) WriteEvent(");
            
            #line 23 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(evt.EventId));
            
            #line default
            #line hidden
            
            #line 23 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(ParametersFrom(evt.Parameters, true)));
            
            #line default
            #line hidden
            this.Write(");\r\n");
            
            #line 24 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
 this.PopBracket();
} 
            
            #line default
            #line hidden
            this.Write("#endregion\r\n\r\n");
            
            #line 28 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
 this.PopBracket(); 
            
            #line default
            #line hidden
            
            #line 29 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
 OutputClassSuffix(); 
            
            #line default
            #line hidden
            this.Write("\r\n");
            this.Write("\r\n");
            
            #line 29 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceWithInterface.t4"
 this.PopBracket(); 
            
            #line default
            #line hidden
            this.Write("\r\n\r\n");
            this.Write("\r\n");
            
            #line 26 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceTemplate.tt"
 }
   else 
   { 
            
            #line default
            #line hidden
            
            #line 1 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
  OutputClassPrefix(); 
            
            #line default
            #line hidden
            this.Write("[EventSource(Name = \"");
            
            #line 2 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.UniqueName));
            
            #line default
            #line hidden
            this.Write("\")]\r\n");
            
            #line 3 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(eventSourceScope));
            
            #line default
            #line hidden
            this.Write(" sealed partial class ");
            
            #line 3 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(eventSourceClassName));
            
            #line default
            #line hidden
            this.Write(" : EventSource\r\n");
            
            #line 4 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
 this.PushBracket(); 
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 6 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
 if (!loggerHandled) { 
            
            #line default
            #line hidden
            this.Write("#region Standard class stuff\r\n// Private constructor blocks direct instantiation " +
                    "of class\r\nprivate ");
            
            #line 4 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write("() {}\r\n\r\n// Readonly access to cached, lazily created singleton instance\r\nprivate" +
                    " static readonly Lazy<");
            
            #line 7 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write("> _lazyLog = \r\n        new Lazy<");
            
            #line 8 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write(">(() => new ");
            
            #line 8 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write("()); \r\npublic static ");
            
            #line 9 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write(" Log\r\n{\r\n\tget { return _lazyLog.Value; }\r\n}\r\n// Readonly access to  private cache" +
                    "d, lazily created singleton inner class instance\r\nprivate static readonly Lazy<");
            
            #line 14 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write("> _lazyInnerlog = \r\n        new Lazy<");
            
            #line 15 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write(">(() => new ");
            
            #line 15 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write("());\r\nprivate static ");
            
            #line 16 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceStandardLogger.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Meta.Name));
            
            #line default
            #line hidden
            this.Write(" innerLog\r\n{\r\n\tget { return _lazyInnerlog.Value; }\r\n}\r\n#endregion\r\n");
            this.Write("\r\n");
            
            #line 8 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
 } 
            
            #line default
            #line hidden
            this.Write("\r\n#region Your trace event methods\r\n\r\n");
            
            #line 12 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"

foreach(var evt in Meta.Events)
{
    OutputXmlComments(evt);
	OutputAttributes(evt);

            
            #line default
            #line hidden
            this.Write("[Event(");
            
            #line 18 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(evt.EventId));
            
            #line default
            #line hidden
            this.Write(")]\r\n");
            
            #line 19 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(MethodDeclaration(evt.Name, evt.ScopeAccess, evt.Parameters)));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 22 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
  this.PushBracket(); 
            
            #line default
            #line hidden
            this.Write("   \r\nif (IsEnabled()) WriteEvent(");
            
            #line 23 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(evt.EventId));
            
            #line default
            #line hidden
            
            #line 23 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(ParametersFrom(evt.Parameters, true)));
            
            #line default
            #line hidden
            this.Write(");\r\n");
            
            #line 24 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
 this.PopBracket();
} 
            
            #line default
            #line hidden
            this.Write("#endregion\r\n\r\n");
            
            #line 28 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
 this.PopBracket(); 
            
            #line default
            #line hidden
            
            #line 29 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceNormal.t4"
 OutputClassSuffix(); 
            
            #line default
            #line hidden
            this.Write("\r\n");
            this.Write("\r\n");
            
            #line 30 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceTemplate.tt"
   } 
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 32 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceTemplate.tt"
 OutputNamespaceClose(); 
            
            #line default
            #line hidden
            return this.GenerationEnvironment.ToString();
        }
        
        #line 33 "C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\Examples\SemanticLogGeneration\SemanticLogGenerationTemplates\EventSourceTemplate.tt"

public override string FilePathHint
{
   get
   { return @"{ExecutionPath}\..\..\..\DomainOutput\{MetadataFileName}2.g.cs";
   }
}

        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
}
