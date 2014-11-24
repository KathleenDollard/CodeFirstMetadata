using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeFirst.Common;
using RoslynDom.Common;
using System.Collections;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;

namespace CodeFirst
{
   public class CodeFirstMapper : IMapper
   {
      private static readonly TypeInfo thisType = typeof(CodeFirstMapper).GetTypeInfo();

      private static PluralizationService pluralizeService = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));

      public object Map(TargetChildMapping mapping, IDom source)
      {
         var codeFirstType = mapping.GetType().GetTypeInfo().GenericTypeArguments.First();
         var innerType = codeFirstType.GetTypeInfo().GenericTypeArguments.First();
         return ReflectionHelpers.InvokeGenericMethod(thisType, "MapInternal",
               innerType, this, mapping, source);
      }

      private T MapInternal<T>(TargetChildMapping mapping, IDom source)
          where T : CodeFirstMetadata<T>
      {
         var ret = ReflectionHelpers.CreateInstanceOfType<T>();
         AssignNamedProperties(source, ret, mapping);
         var sourceWithAttributes = source as IHasAttributes;
         if (sourceWithAttributes != null)
         {
            AssignAttributesToProperties(sourceWithAttributes, ret, mapping);
         }
         AssignChildren(source, ret, mapping);
         return ret;
      }

      private void AssignNamedProperties<T>(IDom source, T obj, TargetChildMapping mapping) 
         where T : CodeFirstMetadata<T>
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
            var value = source.RequestValue(namedProperty);
            if (ReflectionUtilities.CanSetProperty(obj, namedProperty))
            {
               ReflectionHelpers.AssignPropertyValue(obj, namedProperty, value);
            }
         }
      }

      private void AssignAttributesToProperties<T>(IHasAttributes source, T obj, TargetChildMapping mapping)
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

      private void AssignChildren<T>(IDom source, T obj, TargetChildMapping mapping)
      {
         foreach (var childMap in mapping.Children)
         {
            var sourceChildren = GetSourceChildren(source, childMap);
            if (sourceChildren != null)
            {
               var newItems = ReflectionHelpers.InvokeGenericMethod(thisType, "CreateTargetChildren",
                     childMap.UnderlyingTypInfo, this, sourceChildren, childMap);
               newItems = ReflectionHelpers.InvokeGenericMethod(thisType, "AddFromConstructor",
                    childMap.UnderlyingTypInfo, this, source, childMap, newItems);

               ReflectionHelpers.AssignPropertyValue(obj, childMap.TargetName, newItems);
            }
         }
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

      private IEnumerable<T> CreateTargetChildren<T>(IEnumerable<IDom> sourceChildren, TargetChildMapping mapping)
      {
         var newItems = new List<T>();
         foreach (var sourceChild in sourceChildren)
         {
            var newChild = (T)Map(mapping, sourceChild);
            newItems.Add(newChild);
         }
         return newItems;
      }

      private IEnumerable<T> AddFromConstructor<T>(IDom source, 
                  TargetChildMapping mapping, IEnumerable<T> items)
      {
         if (source == null) { return items; }
         var newItems = new List<T>();
         var sourceWithConstructor = source as IClassOrStructure;
         if (sourceWithConstructor == null) { return items; }
         foreach (var constructor in sourceWithConstructor.Constructors) // no clue what multiple constructors might mean
         {
            foreach (var statement in constructor.Statements)
            {
               var assignment = statement as IInvocationStatement;
               if (assignment != null)
               {
                  var name = assignment.MethodName;
                  // Convention used here
                  if (!name.StartsWith("Add")) { continue; }
                  name = name.SubstringAfter("Add");
                  var plural = pluralizeService.Pluralize(name);
                  var property = (source as ITypeMemberContainer).Properties
                                    .Where(x => x.Name == plural)
                                    .FirstOrDefault();
                  if (property == null) { continue; }

                  // TODO: KAD: Start Here
                  ////var isLiteral = assignment.Expression.ExpressionType == ExpressionType.Literal;
                  //var isIdentifier = assignment.Left.ExpressionType == ExpressionType.Identifier;
                  //if (isLiteral && isIdentifier)
                  //{
                  //   AddSensibly(ret, assignment.Left.Expression, assignment.Expression.Expression);
                  //}
               }
            }
         }
         return items.Union(newItems);
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
            IEnumerable<IAttributeValue > attribValues = attrib.AttributeValues;
            var firstValue = attrib?.AttributeValues?.FirstOrDefault();
            if (firstValue != null && string.IsNullOrEmpty(firstValue.Name )) // assume positional param
            {
               ret.Add(new UsageTrackingEntry<KeyValuePair<string, object>>(new KeyValuePair<string, object>(attrib.Name, firstValue.Value)));
               attribValues = attribValues.Skip(1);
            }
            foreach (var attribValue in attribValues)
            {
               AddSensibly(ret, attribValue.Name, attribValue.Value);
            }
         }
         GetConstructorValues(ret, source as IClassOrStructure );
         return ret;
      }

      private void GetConstructorValues(List<UsageTrackingEntry<KeyValuePair<string, object>>> ret, IClassOrStructure source)
      {
         if (source == null) { return; }
         foreach (var constructor in source.Constructors ) // no clue what multiple constructors might mean
         {
            foreach(var statement in constructor.Statements)
            {
               var assignment = statement as IAssignmentStatement;
               if (assignment != null)
               {
                  var isLiteral = assignment.Expression.ExpressionType == ExpressionType.Literal;
                  var isIdentifier = assignment.Left.ExpressionType == ExpressionType.Identifier;
                  if (isLiteral && isIdentifier )
                  {
                     AddSensibly(ret, assignment.Left.InitialExpressionString , assignment.Expression.InitialExpressionString);
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
