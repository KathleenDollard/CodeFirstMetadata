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

    public abstract class RuntimeT4CSharpBase : RuntimeT4Base
    {

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
        }

        protected string ParameterDeclarations(IEnumerable<IHasParameterInfo> parameters)
        {
            var indent = this.CurrentIndent + new string(' ', MethodContinueIndent);
            var separator = ", \r\n" + indent;

            return ConcatenateWith(separator,
                parameters.Select(x => x.TypeName + " " + x.Name));
        }

        protected override string PropertyDeclaration(string name,
            ScopeAccess scope, bool isStatic, string propertyTypeName)
        {
            return ToStringHelper.ToStringWithCulture(scope) + " " +
                (isStatic ? "static " : "") +
                propertyTypeName + " " + MakePublicName(name);
        }

        protected override string FieldDeclaration(string name, 
            ScopeAccess scope, bool isStatic, string fieldTypeName)
        {
            return ToStringHelper.ToStringWithCulture(scope) + " " +
                (isStatic ? "static " : "") +
                fieldTypeName + " " + MakeFieldName(name);
        }

    }

  }
