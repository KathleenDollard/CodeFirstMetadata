using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TextTemplating.VSHost;
using System.Reflection;
using CodeFirst.Common;
using RoslynDom.Common;

namespace KadGen.Common
{

    public  class RuntimeT4CSharpBase : RuntimeT4Base
    {
        public virtual void Initialize() { }

        protected void PushBracket()
        { PushBracket(""); }

        protected void PushBracket(string name)
        {
            if (!string.IsNullOrWhiteSpace(name)) this.WriteLine(name);
            this.Write("{");
            this.PushIndent();
        }
        protected void PopBracket()
        {
            this.PopIndent();
            this.Write("}\r\n");
        }

        protected override string MethodDeclaration(string name,
                    ScopeAccess scope,
                    bool isStatic,
                    string returnTypeName,
                    IEnumerable<IHasParameterInfo> parameters)
        {
            var indent = this.CurrentIndent + new string(' ', MethodContinueIndent);
            return ToStringHelper.ToStringWithCulture(scope) + " " +
                (isStatic ? "static " : "") +
                returnTypeName + " " + name + "(" +
                ParameterDeclarations(parameters) + ")";
            //(parameters.Count() == 0 ? "" : "\r\n" + indent) +
        }

        protected string ParameterDeclarations(IEnumerable<IHasParameterInfo> parameters)
        {
            var indent = this.CurrentIndent + new string(' ', MethodContinueIndent);
            var separator = ", \r\n" + indent;

            return ConcatenateWith(separator,
                parameters.Select(x => x.TypeName + " " + x.Name));
        }
    }

    public abstract class RuntimeT4Base : RuntimeT4PatternBase
    {
        protected int IndentSize = 3;
        protected int MethodContinueIndent = 12;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attr"></param>
        /// <returns></returns>
        /// <remarks>
        /// Internal scope is for the purposes of unit testing
        /// </remarks>
        protected internal static string GetAttributeConstructor(CodeFirstAttribute attr)
        {
            var ret = attr.Name + "(";
            var propList = attr.Properties
                           .OrderBy(x => string.IsNullOrWhiteSpace(x.Name) ? 0 : 1)
                           .ThenBy(x => x.Ordinal);
            foreach (var prop in propList)
            {
                if (!prop.Equals(propList.First()))
                { ret += ", "; }
                if (string.IsNullOrWhiteSpace(prop.Name))
                { ret += prop.Value; }
                else
                { ret += prop.Name + " = " + prop.Value; }
            }
            return ret + ")";
        }

        protected void WriteLine()
        { this.WriteLine(""); }

        protected virtual void OutputGenerationWarning()
        { // no op
        }
        protected void PushIndent()
        {
            this.PushIndent(new string(' ', IndentSize));
        }

        protected string ParametersFrom(
                IEnumerable<IHasParameterInfo> parameters,
                bool includeLeadingComma = false)
        {
            if (parameters.Count() == 0) return "";
            return (includeLeadingComma ? ", " : "") +
                ConcatenateWith(", ", parameters.Select(x => x.Name));
        }

        protected string IfNotNull<T>(T item, params object[] outputItems) where T : class
        {
            if (item == null) return "";
            return (ConcatenateWithSpace(outputItems));
        }

        protected string IfNotDefault<T>(T item, params object[] outputItems)
        {
            if (item.Equals(default(T))) return "";
            return (ConcatenateWithSpace(outputItems));
        }

        protected string ConcatenateWithSpace(IEnumerable<object> outputItems)
        {
            return ConcatenateWith(" ", outputItems);
        }

        protected string ConcatenateWith(string separator, IEnumerable<object> outputItems)
        {
            // String builder not used because lists are expected to be short
            var ret = "";
            foreach (var output in outputItems)
            {
                ret += separator + ToStringHelper.ToStringWithCulture(output);
            }
            if (ret.Length == 0) return ret;
            return ret.Substring(separator.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scope"></param>
        /// <param name="parameters"></param>
        /// <returns>
        /// VB overrides must understand that a Sub is a void return.
        /// Users will never see this, so it's a useful hack.
        /// </returns>
        protected string MethodDeclaration(string name,
                    ScopeAccess scope,
                    IEnumerable<IHasParameterInfo> parameters)
        { return MethodDeclaration(name, scope, false, "void", parameters); }

        protected string MethodDeclaration(string name,
                    ScopeAccess scope,
                    string returnTypeName,
                    IEnumerable<IHasParameterInfo> parameters)
        { return MethodDeclaration(name, scope, false, returnTypeName, parameters); }

        protected abstract string MethodDeclaration(string name,
                    ScopeAccess scope,
                    bool isStatic,
                    string returnTypeName,
                    IEnumerable<IHasParameterInfo> parameters);




        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            return this.GenerationEnvironment.ToString();
        }

    }

    /// <summary>
    /// </summary>


    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
    public class RuntimeT4PatternBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0)
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
