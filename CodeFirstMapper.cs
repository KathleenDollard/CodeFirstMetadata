using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeFirstMetadataTest.Common;
using RoslynDom.Common;

namespace CodeFirstMetadataTest
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
            foreach (var namedProperty in mapping.NamedProperties)
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
            var usage = MakeAttributeValuePairList(source);

            foreach (var map in mapping.Attributes)
            {
                var valueUsage = usage.Where(x => x.Value.Name == map.TargetName).FirstOrDefault();
                if (valueUsage != null)
                {
                    if (ReflectionUtilities.CanSetProperty(obj, map.TargetName))
                    {
                        valueUsage.IncrementUse();
                        // TODO: Fix naming to avoid Value.Value
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
                    var newItems =  ReflectionHelpers.InvokeGenericMethod(thisType, "CreateTargetChildren",
                          childMap.UnderlyingTypInfo , this, sourceChildren, childMap );
                    ReflectionHelpers.AssignPropertyValue(obj, childMap.TargetName, newItems);
                }
            }
        }

        private IEnumerable<IDom> GetSourceChildren(IDom source, TargetChildMapping mapping)
        {
            // TODO: Add class, properties, etc. maybe namespace, depending on abstract root 
            // yes, it's ugly, but the reflection here is worse, should have used DI
            if ((mapping as TargetMethodMapping) != null)
            { return ((ITypeMemberContainer)source).Methods; }
            if ((mapping as TargetParameterMapping) != null)
            { return ((IPropertyOrMethod)source).Parameters; }
            return null;
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

        private List<UsageTrackingEntry<IAttributeValue>> MakeAttributeValuePairList(IHasAttributes source)
        {
            // Since I just overwrite existing values, they are placed in the array in reverse order of precendence
            // This is like On3 or something, but the lists should remain small. If attribute lists get non-trivial keep an eye out here. 
            var ret = new List<UsageTrackingEntry<IAttributeValue>>();
            foreach (var attrib in source.Attributes)
            {
                foreach (var attribValue in attrib.AttributeValues)
                {
                    if (!ret.Exists(x => x.Value.Name == attribValue.Name)) // Currently no error or warning
                    {
                        ret.Add(new UsageTrackingEntry<IAttributeValue>(attribValue));
                    }
                }
            }
            return ret;
        }
    }
}
