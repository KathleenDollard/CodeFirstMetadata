using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeFirst.Common;
using RoslynDom.Common;

namespace CodeFirst
{
   public class CodeFirstMapper : IMapper
   {
      private static readonly TypeInfo thisType = typeof(CodeFirstMapper).GetTypeInfo();

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

      private void AssignNamedProperties<T>(IDom source, T obj, TargetChildMapping mapping) where T : CodeFirstMetadata<T>
      {
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
            foreach (var attribValue in attrib.AttributeValues)
            {
               if (string.IsNullOrWhiteSpace(attribValue.Name))
               {
                  // Relies on unnamed coming before named
                  if (attribValue == attrib.AttributeValues.First())
                  {
                     ret.Add(new UsageTrackingEntry<KeyValuePair<string, object>>(new KeyValuePair<string, object>(attrib.Name, attribValue.Value)));
                  }
               }
               if (!string.IsNullOrWhiteSpace(attribValue.Name)
                     && !ret.Exists(x => x.Value.Key == attribValue.Name)) // No error or warning non dupes
               {
                  ret.Add(new UsageTrackingEntry<KeyValuePair<string, object>>(new KeyValuePair<string, object>(attribValue.Name, attribValue.Value)));
               }
            }
         }
         return ret;
      }
   }
}
