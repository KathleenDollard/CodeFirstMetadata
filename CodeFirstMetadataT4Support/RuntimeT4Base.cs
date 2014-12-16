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

   public abstract class RuntimeT4Base : RuntimeT4PatternBase
   {
      protected int IndentSize = 3;
      protected int MethodContinueIndent = 12;

      public virtual string FilePathHint { get { return ""; } }
      public virtual string Name { get { return ""; } }

      protected virtual string MakeFieldName(string name)
      { return "_" + name.Substring(0, 1).ToLower() + name.Substring(1); }
      protected virtual string MakePublicName(string name)
      { return name.Substring(0, 1).ToUpper() + name.Substring(1); }
      protected virtual string MakeLocalName(string name)
      { return name.Substring(0, 1).ToLower() + name.Substring(1); }

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

      protected string PropertyDeclaration(string name,
           string propertyTypeNam)
      { return PropertyDeclaration(name, ScopeAccess.Public, false, propertyTypeNam); }

      protected string PropertyDeclaration(string name,
                  ScopeAccess scope,
                  string propertyTypeNam)
      { return PropertyDeclaration(name, scope, false, propertyTypeNam); }

      protected abstract string PropertyDeclaration(string name,
                  ScopeAccess scope,
                  bool isStatic,
                  string propertyTypeNam);

      protected string FieldDeclaration(string name,
               string fieldTypeName)
      { return FieldDeclaration(name, ScopeAccess.Private, false, fieldTypeName); }

      protected string FieldDeclaration(string name,
          ScopeAccess scope,
          string fieldTypeName)
      { return FieldDeclaration(name, scope, false, fieldTypeName); }

      protected abstract string FieldDeclaration(string name,
                  ScopeAccess scope,
                  bool isStatic,
                  string fieldTypeName);



      /// <summary>
      /// Create the template output
      /// </summary>
      public virtual string TransformText()
      {
         return this.GenerationEnvironment.ToString();
      }

   }


}
