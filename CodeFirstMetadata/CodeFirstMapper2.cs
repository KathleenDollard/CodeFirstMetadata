using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeFirst.Common;
using RoslynDom.Common;
using System.Collections;
using System.Globalization;

namespace CodeFirst
{
   public class CodeFirstMapper2<T> : IMapper2<T>
      where T : CodeFirstMetadata<T>
   {
      private static readonly TypeInfo thisType = typeof(CodeFirstMapper2<T>).GetTypeInfo();
      //private static PluralizationService pluralizeService = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));
      private ICodeFirstServiceProvider serviceProvider;

      public CodeFirstMapper2(ICodeFirstServiceProvider serviceProvider)
      {
         this.serviceProvider = serviceProvider;
      }

      public virtual IEnumerable<Type> SupportedTypes
      { get { return new[] { typeof(CodeFirstMetadata) }; } }

      public virtual T Map(TargetChildMapping mapping, IDom source, CodeFirstMetadata parent)
      {
         // var codeFirstType = mapping.GetType().GetTypeInfo().GenericTypeArguments.First();
         var newItem = ReflectionHelpers.CreateInstanceOfType<T>();
         newItem.Parent = parent;
         AssignNamedProperties(source, newItem, mapping);
         var sourceWithAttributes = source as IHasAttributes;
         if (sourceWithAttributes != null)
         {
            AssignAttributesToProperties(sourceWithAttributes, newItem, mapping);
         }
         AssignChildren(source, newItem, mapping, newItem);
         return newItem;
      }

      public virtual IEnumerable<T> MapList(TargetChildMapping mapping, IEnumerable<IDom> sourceList, CodeFirstMetadata parent)
      {
         var ret = new List<T>();
         foreach (var source in sourceList)
         { ret.Add(Map(mapping, source, parent)); }
         return ret;
      }

      public virtual IEnumerable<T> MapFromConstructor(TargetChildMapping mapping, IDom source, CodeFirstMetadata parent)
      {
         var newItems = new List<T>();
         var sourceWithConstructor = source as IClassOrStructure;
         if (sourceWithConstructor != null)
         {
            foreach (var constructor in sourceWithConstructor.Constructors) // no clue what multiple constructors might mean
            {
               foreach (var statement in constructor.Statements)
               {
                  newItems.AddRange(MapInvocation(statement as IInvocationStatement, source as IClass, parent));
               }
            }
         }
         return newItems;
      }

      private void AssignNamedProperties(IDom source, T obj, TargetChildMapping mapping)
      {
         var sourceHasStructuredDocs = source as IHasStructuredDocumentation;
         if (sourceHasStructuredDocs != null)
         {
            var xmlDocs = sourceHasStructuredDocs.StructuredDocumentation;
            obj.XmlCommentString = xmlDocs.Document;
         }
         var namedProperties = mapping.GetNamedProperties().ToList();
         foreach (var namedProperty in namedProperties)
         {
            var value = source.RequestValue(namedProperty, true);
            //while (value == null) { value = source.Parent.RequestValue(namedProperty); }
            if (ReflectionUtilities.CanSetProperty(obj, namedProperty))
            {
               ReflectionHelpers.AssignPropertyValue(obj, namedProperty, value);
            }
         }
      }

      private void AssignAttributesToProperties(IHasAttributes source, T obj, TargetChildMapping mapping)
      {
         var usage = MakeValuePairList(source);

         foreach (var map in mapping.Attributes)
         {
            var valueUsage = usage.Where(x => x.Value.Key == map.TargetName).FirstOrDefault();
            if (valueUsage != null)
            {
               if (ReflectionUtilities.CanSetProperty(obj, map.TargetName))
               {
                  valueUsage.IncrementUse();
                  ReflectionHelpers.AssignPropertyValue(obj, map.TargetName, valueUsage.Value.Value);
               }
            }
         }
      }

      private void AssignChildren(IDom source, T obj, TargetChildMapping mapping, CodeFirstMetadata parent)
      {
         foreach (var childMap in mapping.Children)
         {
            var newItems = ReflectionHelpers.InvokeGenericMethod(thisType, "CreateChildren",
                           childMap.UnderlyingTypInfo, this, source, childMap, parent);
            ReflectionHelpers.AssignPropertyValue(obj, childMap.TargetName, newItems);
         }
      }

      private IEnumerable<TLocal> CreateChildren<TLocal>(IDom source, TargetChildMapping mapping, CodeFirstMetadata parent)
        where TLocal : CodeFirstMetadata
      {
         var mapper = serviceProvider.GetMapper2<TLocal>();
         var sourceChildren = GetSourceChildren(source, mapping);
         var items = new List<TLocal>();
         if (sourceChildren != null)
         {
            items.AddRange( mapper.MapList(mapping, sourceChildren, parent));
         }
         var newItems2 = mapper.MapFromConstructor(mapping, source, parent);
         items.AddRange(newItems2);
         return items;
      }

      private IEnumerable<IDom> GetSourceChildren(IDom source, TargetChildMapping mapping)
      {
         // TODO: Add class, properties, etc. maybe namespace, depending on abstract root 
         // yes, it's ugly, but the reflection here is worse, should have used DI
         if ((mapping as TargetClassMapping) != null)
         { return ((INestedContainer)source).Classes; }
         if ((mapping as TargetMethodMapping) != null)
         { return ((ITypeMemberContainer)source).Methods; }
         if ((mapping as TargetPropertyMapping) != null)
         { return ((ITypeMemberContainer)source).Properties; }
         if ((mapping as TargetParameterMapping) != null)
         { return ((IPropertyOrMethod)source).Parameters; }
         throw new NotImplementedException();
      }

       private IEnumerable<T> MapInvocation(IInvocationStatement invocation, IClass source, CodeFirstMetadata parent)
      {
         var newItems = new List<T>();
         if (invocation == null) { return newItems; }
         if (source == null) { return newItems; }

         var name = invocation.MethodName;
         // English language convention used here
         if (name.StartsWith("Add"))
         {
            name = name.SubstringAfter("Add");
            var plural = Pluralizer.Pluralize(name);
            var current = source;
            if (current != null)
            {
               var property = GetPropertyUseBase(plural, ref current);
               if (property != null)
               {
                  var newItem = Activator.CreateInstance<T>();
                  newItem.Parent = parent;
                  SetNewItemProperties(newItem, invocation);
                  newItems.Add(newItem);
               }
            }
         }
         return newItems;
      }

      protected virtual void SetNewItemProperties<TLocal>(TLocal newItem, IInvocationStatement invocation)
      {
         var newProperties = typeof(TLocal).GetProperties();
         var usedProperties = new List<PropertyInfo>();
         foreach (var arg in invocation.Arguments)
         {
            // TODO: Allow unnamed args by correlating with method parameters. 
            var argName = arg.Name.Substring(0, 1).ToUpperInvariant() + arg.Name.Substring(1);
            var propInfo = newProperties.Where(x => x.Name == argName).FirstOrDefault();
            if (propInfo != null)
            {
               object value = arg.ValueExpression;
               if (propInfo.PropertyType == typeof(string)) { value = arg.ValueExpression.InitialExpressionString; }
               propInfo.SetValue(newItem, value);
               usedProperties.Add(propInfo);
            }
         }
         if (invocation.TypeArguments.Any())
         {
            var propInfo = typeof(TLocal).GetProperties().Where(x => x.Name == "PropertyType").First();
            if (propInfo != null)
            {
               propInfo.SetValue(newItem, invocation.TypeArguments.First());
            }
         }
      }

      private static IProperty GetPropertyUseBase(string plural, ref IClass current)
      {
         IProperty property = null;
         while (property == null && current != null)
         {
            property = (current as ITypeMemberContainer).Properties
                              .Where(x => x.Name == plural)
                              .FirstOrDefault();
            current = current.BaseType == null
                        ? null
                        : current.BaseType.Type as IClass;
         }

         return property;
      }

      private class UsageTrackingEntry<TValue>
      {
         private TValue _value;
         private int _useCount;

         public UsageTrackingEntry(TValue value)
         {
            _value = value;
            _useCount = 0;
         }

         public void IncrementUse() { _useCount++; }

         public TValue Value { get { return _value; } }
      }

      private List<UsageTrackingEntry<KeyValuePair<string, object>>> MakeValuePairList(IHasAttributes source)
      {
         // TODO: Consider precendence on dupes. Currently order they appear, first wins 
         var ret = new List<UsageTrackingEntry<KeyValuePair<string, object>>>();
         foreach (var attrib in source.Attributes)
         {
            IEnumerable<IAttributeValue> attribValues = attrib.AttributeValues;
            var firstValue = attrib == null
                              ? null
                              : (attrib.AttributeValues == null
                                 ? null
                                 : attrib.AttributeValues.FirstOrDefault());
            if (firstValue != null && string.IsNullOrEmpty(firstValue.Name)) // assume positional param
            {
               ret.Add(new UsageTrackingEntry<KeyValuePair<string, object>>(new KeyValuePair<string, object>(attrib.Name, firstValue.Value)));
               attribValues = attribValues.Skip(1);
            }
            foreach (var attribValue in attribValues)
            {
               AddSensibly(ret, attribValue.Name, attribValue.Value);
            }
         }
         GetConstructorValues(ret, source as IClassOrStructure);
         return ret;
      }

      private void GetConstructorValues(List<UsageTrackingEntry<KeyValuePair<string, object>>> ret, IClassOrStructure source)
      {
         if (source == null) { return; }
         foreach (var constructor in source.Constructors) // no clue what multiple constructors might mean
         {
            foreach (var statement in constructor.Statements)
            {
               var assignment = statement as IAssignmentStatement;
               if (assignment != null)
               {
                  var isLiteral = assignment.Expression.ExpressionType == ExpressionType.Literal;
                  var isIdentifier = assignment.Left.ExpressionType == ExpressionType.Identifier;
                  if (isLiteral && isIdentifier)
                  {
                     AddSensibly(ret, assignment.Left.InitialExpressionString, assignment.Expression.InitialExpressionString);
                  }
               }
            }
         }
      }

      private void AddSensibly(List<UsageTrackingEntry<KeyValuePair<string, object>>> ret, string name, object value)
      {
         if (!string.IsNullOrWhiteSpace(name)
                           && !ret.Exists(x => x.Value.Key == name)) // No error or warning on dupes
         {
            ret.Add(new UsageTrackingEntry<KeyValuePair<string, object>>(new KeyValuePair<string, object>(name, value)));
         }
      }
   }
}
