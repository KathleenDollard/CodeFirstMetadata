using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics.Contracts;
using System.Collections;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using CodeFirstMetadata.Common;
using RoslynDom.Common;
using RoslynDom;

namespace CodeFirstMetadata.Roslyn
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// Internally, this is pretty gnarly code, not for the faint of heart
    /// </remarks>
    public class ParserForMetadata<T> : IParser<T>
        where T : CodeFirstMetadata<T>
    {
        public IEnumerable<T> Parse(string str)
        {
            var ret = new List<T>();
            var root = RDomFactory.GetRootFromString(str);
            var classes = root.Classes;
            foreach (var cl in classes)
            {
                var item = ParseClass<T>(cl);
                if (item != null) ret.Add(item);
            }
            return ret;
        }

        private TTargetClass ParseClass<TTargetClass>(IClass cl)
            where TTargetClass : CodeFirstMetadata.Common.CodeFirstMetadata 
        {
            return ParseLevel<TTargetClass, IClass>(cl,
                       GetParseMethods,
                       new LookupSet<IClass>("Interfaces", x=>x.AllImplementedInterfaces));
        }

        private IEnumerable GetParsedMethods<TDom>(TDom domItem, System.Reflection.TypeInfo propType)
        {
            var ret = new List<TMethodItem>();
            var concreteMethod = ReflectionSupport.MakeMethod(typeof(ParserForMetadata<T>), "ParseMethod", typeof(TMethodItem));
            var methodNodes = classNode.GetMethods();
            foreach (var methodNode in methodNodes)
            {
                ret.Add((TMethodItem)concreteMethod.Invoke(this, new object[] { methodNode }));
            }
            return ret;
            var concreteMethod = ReflectionSupport.MakeMethod(typeof(ParserForMetadata<T>), "GetParsedMethodsGeneric", propType);
            return (IEnumerable)concreteMethod.Invoke(this, new object[] { classNode });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TLevel"></typeparam>
        /// <param name="node"></param>
        /// <param name="nestedLevel"></param>
        /// <param name="assignments"></param>
        /// <returns>
        /// While the gnarly part of this class is partially in the reflection work, 
        /// the guts of the class is this method
        /// </returns>
        private TTarget ParseLevel<TTarget, TDom>(TDom domItem,
            IEnumerable<IDom> nextLevel,
            params LookupSet<TDom>[] assignments)
            where TTarget : CodeFirstMetadata.Common.CodeFirstMetadata
        {
            var usedAttributes = new List<AttributeSyntax>();
            var attributes = node.GetAttributes();
            var typeInfo = typeof(TLevel).GetTypeInfo();
            var ret = Activator.CreateInstance<TLevel>();
            ret.XmlCommentString = node.GetXmlComments();
            var props = ReflectionSupport.GetAllProperties(typeInfo, typeof(KadMetadata)).Where(x => x.CanWrite);
            foreach (var prop in props)
            {
                var assigned = false;
                foreach (var assignment in assignments)
                {
                    if (TrySetValue(prop, assignment.Item1, ret, assignment.Item2.Invoke(node)))
                    { assigned = true; break; }
                }
                if (!assigned)
                {
                    var matchingAttributes = attributes.Where(x => x.Name.ToString() == prop.Name);
                    if (IsPropertyList(prop))
                    {
                        var underlyingType = UnderlyingType(prop.PropertyType);
                        if (IsTypeComplex(underlyingType))
                        {
                            var list = nestedLevel.Invoke(node, underlyingType);
                            prop.SetValue(ret, list);
                        }
                        else
                        { AssignListProperty(ret, prop, matchingAttributes); }
                    }
                    else
                    {
                        AssignSimpleProperty(ret, prop, matchingAttributes);
                    }
                    usedAttributes.AddRange(matchingAttributes);
                }
            }
            var unusedAttributes = attributes.Except(usedAttributes);
            AddExplicitAttributes(ret, unusedAttributes);
            return ret;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMethod"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        /// <remarks>
        /// Don't be concerned by the lack of references, this is called via reflection
        /// </remarks>
        private TMethod ParseMethod<TMethod>(MethodDeclarationSyntax node)
            where TMethod : KadMetadata
        {
            return ParseLevel<TMethod>(node,
                       GetParsedParameters,
                       GetPair(node, "Name", GetMethodName),
                       GetPair(node, "Body", GetMethodBody));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMethod"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        /// <remarks>
        /// Don't be concerned by the lack of references, this is called via reflection
        /// </remarks>
        private TParam ParseParameter<TParam>(ParameterSyntax node)
            where TParam : KadMetadata
        {
            return ParseLevel<TParam>(node,
                       null,
                       GetPair(node, "Name", GetParameterName),
                       GetPair(node, "TypeName", GetParameterType));
        }

        private Tuple<string, Func<SyntaxNode, object>>
            GetPair<TNode>(TNode node, string key, Func<SyntaxNode, object> func)
            where TNode : SyntaxNode
        {
            return new Tuple<string, Func<SyntaxNode, object>>
                (key, func);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TLevel"></typeparam>
        /// <param name="node"></param>
        /// <param name="nestedLevel"></param>
        /// <param name="assignments"></param>
        /// <returns>
        /// While the gnarly part of this class is partially in the reflection work, 
        /// the guts of the class is this method
        /// </returns>
        private TLevel ParseLevel<TLevel>(SyntaxNode node,
            Func<SyntaxNode, Type, IEnumerable> nestedLevel,
            params Tuple<string, Func<SyntaxNode, object>>[] assignments)
            where TLevel : KadMetadata
        {
            var usedAttributes = new List<AttributeSyntax>();
            var attributes = node.GetAttributes();
            var typeInfo = typeof(TLevel).GetTypeInfo();
            var ret = Activator.CreateInstance<TLevel>();
            ret.XmlCommentString = node.GetXmlComments();
            var props = ReflectionSupport.GetAllProperties(typeInfo, typeof(KadMetadata)).Where(x => x.CanWrite);
            foreach (var prop in props)
            {
                var assigned = false;
                foreach (var assignment in assignments)
                {
                    if (TrySetValue(prop, assignment.Item1, ret, assignment.Item2.Invoke(node)))
                    { assigned = true; break; }
                }
                if (!assigned)
                {
                    var matchingAttributes = attributes.Where(x => x.Name.ToString() == prop.Name);
                    if (IsPropertyList(prop))
                    {
                        var underlyingType = UnderlyingType(prop.PropertyType);
                        if (IsTypeComplex(underlyingType))
                        {
                            var list = nestedLevel.Invoke(node, underlyingType);
                            prop.SetValue(ret, list);
                        }
                        else
                        { AssignListProperty(ret, prop, matchingAttributes); }
                    }
                    else
                    {
                        AssignSimpleProperty(ret, prop, matchingAttributes);
                    }
                    usedAttributes.AddRange(matchingAttributes);
                }
            }
            var unusedAttributes = attributes.Except(usedAttributes);
            AddExplicitAttributes(ret, unusedAttributes);
            return ret;
        }

        private void AddExplicitAttributes<TItem>(
            TItem item, IEnumerable<AttributeSyntax> explicitAttributes)
            where TItem : KadMetadata
        {
            foreach (var attr in explicitAttributes)
            {
                var kadAttr = new KadAttribute(attr.GetName());
                foreach (var argNode in attr.GetAttributeArguments())
                {
                    var pair = argNode.GetAttributeArgument();
                    kadAttr.Properties.Add(pair);
                }
                item.AdditionalAttributes.Add(kadAttr);
            }
        }

        private bool IsTypeComplex(Type type)
        {
            return type.IsClass && type != typeof(string);
        }

        private static bool IsPropertyList(PropertyInfo prop)
        {
            if (prop.PropertyType == typeof(string)) return false;
            return prop.PropertyType.GetTypeInfo()
                            .ImplementedInterfaces
                            .Where(x => x == typeof(IEnumerable)).Any();
        }

        private static Type UnderlyingType(Type propType)
        {
            if (propType.IsArray)
            { return propType.GetElementType(); }
            var typeArgs = propType.GetGenericArguments();
            if (typeArgs.Count() != 1)
            { throw new ArgumentException("Doesn't appear to be an IEnumerable"); }
            return typeArgs[0];
        }

        private static void AssignSimpleProperty<TItem>(TItem item, PropertyInfo prop, IEnumerable<AttributeSyntax> matchingAttributes)
        {
            switch (matchingAttributes.Count())
            {
                case 1:
                    var value = matchingAttributes.First().GetAttributeValue( prop.PropertyType);
                    prop.SetValue(item, value);
                    break;
                case 0:
                    break;

                default:
                    throw new ArgumentException("Multiple attributes where only one allowed");
            }
        }

        private static void AssignListProperty<TItem>(TItem item, PropertyInfo prop, IEnumerable<AttributeSyntax> matchingAttributes)
        {
            var propType = UnderlyingType(prop.PropertyType);
            var concreteMethod = ReflectionSupport.MakeMethod(typeof(ParserForMetadata<T>), "AssignListPropertyGeneric", typeof(TItem), propType);
            concreteMethod.Invoke(null, new object[] { item, prop, matchingAttributes });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMethod"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        /// <remarks>
        /// Don't be concerned by the lack of references, this is called via reflection
        /// </remarks>
        private static void AssignListPropertyGeneric<TItem, TListItem>(TItem item, PropertyInfo prop, IEnumerable<AttributeSyntax> matchingAttributes)
        {
            var values = matchingAttributes.Select(x => (TListItem)x.GetAttributeValue<TListItem>());
            if (prop.PropertyType.IsArray)
            { prop.SetValue(item, values.ToArray()); }
            else
            { prop.SetValue(item, values); }
        }

        private IEnumerable GetParsedMethods(SyntaxNode classNode, Type propType)
        {
            var concreteMethod = ReflectionSupport.MakeMethod(typeof(ParserForMetadata<T>), "GetParsedMethodsGeneric", propType);
            return (IEnumerable)concreteMethod.Invoke(this, new object[] { classNode });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMethod"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        /// <remarks>
        /// Don't be concerned by the lack of references, this is called via reflection
        /// </remarks>
        private IEnumerable<TMethodItem> GetParsedMethodsGeneric<TMethodItem>(ClassDeclarationSyntax classNode)
        {
            var ret = new List<TMethodItem>();
            var concreteMethod = ReflectionSupport.MakeMethod(typeof(ParserForMetadata<T>), "ParseMethod", typeof(TMethodItem));
            var methodNodes = classNode.GetMethods();
            foreach (var methodNode in methodNodes)
            {
                ret.Add((TMethodItem)concreteMethod.Invoke(this, new object[] { methodNode }));
            }
            return ret;
        }

        private IEnumerable GetParsedParameters(SyntaxNode methodNode, Type propType)
        {
            var concreteMethod = ReflectionSupport.MakeMethod(typeof(ParserForMetadata<T>), "GetParsedParametersGeneric", propType);
            return (IEnumerable)concreteMethod.Invoke(this, new object[] { methodNode });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMethod"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        /// <remarks>
        /// Don't be concerned by the lack of references, this is called via reflection
        /// </remarks>
        private IEnumerable<TParamItem> GetParsedParametersGeneric<TParamItem>(MethodDeclarationSyntax methodNode)
        {
            var ret = new List<TParamItem>();
            var concreteMethod = ReflectionSupport.MakeMethod(typeof(ParserForMetadata<T>), "ParseParameter", typeof(TParamItem));
            var parameterNodes = methodNode.GetParameters();
            foreach (var parameterNode in parameterNodes)
            {
                ret.Add((TParamItem)concreteMethod.Invoke(this, new object[] { parameterNode }));
            }
            return ret;
        }

        private static bool TrySetValue(PropertyInfo prop, string propName, object item, object value)
        {
            if (prop.Name != propName) return false;
            return ReflectionSupport.TrySetPropertyValue(item, propName, value);
        }

        #region Wrapper methods to improve main calls readability

        private object GetClassName(SyntaxNode node)
        { return ((ClassDeclarationSyntax)node).GetName(); }

        private object GetNamespaceName(SyntaxNode node)
        { return ((ClassDeclarationSyntax)node).GetNamespaceName(); }

        private object GetInterfaceNames(SyntaxNode node)
        { return ((ClassDeclarationSyntax)node).GetInterfaceNamesForClass(); }

        private object GetMethodName(SyntaxNode node)
        { return ((MethodDeclarationSyntax)node).GetName(); }

        private object GetMethodBody(SyntaxNode node)
        { return ((MethodDeclarationSyntax)node).GetMethodBody(); }

        private object GetParameterName(SyntaxNode node)
        { return ((ParameterSyntax)node).GetName(); }

        private object GetParameterType(SyntaxNode node)
        { return ((ParameterSyntax)node).GetParameterType(); }

        #endregion

    }
}
