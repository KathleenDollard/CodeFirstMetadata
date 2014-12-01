using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RoslynDom.Common
{
   /// <summary>
   /// 
   /// </summary>
   /// <remarks>
   /// This class exists for items not sufficiently general for RosnlyDom's reflection utillities
   /// </remarks>
   public static class ReflectionHelpers
   {
      public static object GetAttributeForEnumValue(Type type, Type attributeType, object value)
      {
         if (type == null) { return null; }
         if (attributeType == null) { return null; }
         // FUTURE: Not localizing because current usage is to create C# code. Could be localized via convention of a named parameter on such as Culture and CultureValue, assuming multiplicty was not otherwise required
         if (!type.IsEnum) throw new InvalidOperationException("Must run on enum types");
         var fields = type.GetTypeInfo().DeclaredFields.Where(x => x.Name == value.ToString());
         if (fields.Count() == 0) return null;
         var field = fields.First();
         var attributes = field.CustomAttributes.Where(x => x.AttributeType == attributeType);
         if (attributes.Count() == 0) return null;
         return attributes.First().ConstructorArguments.First().Value;
      }

      public static string CultureSpecificToString(
          Type type, object value, IFormatProvider formatProvider)
      {
         if (type == null) { return null; }
         System.Reflection.MethodInfo method = type.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
         if ((method == null))

         {
            // Aggregate exception can be used to test this. I am currently 
            // leaving this an exception so scenarios are evaluated. I trust
            // that types in the framework have been considered and only lack 
            // the overload when appropriate. I do not trust external code. 
            // This may not be the best decision, so this test helps the question
            // be revisited.
            throw new InvalidOperationException();
            //return objectToConvert.ToString();
         }
         else
         {
            return ((string)(method.Invoke(value, new object[] {
                                formatProvider })));
         }
      }

      public static bool HasAttribute(this Type type, Type attributeType)
      {
         if (type == null) { return false; }
         if (attributeType == null) { return false; }
         return type.GetCustomAttributes(attributeType, true).Length > 0;
      }


      public static IEnumerable<PropertyInfo> GetAllProperties(this TypeInfo typeInfo, Type stopClass = null)
      {
         if (typeInfo == null) { return null; }
         stopClass = stopClass ?? typeof(object); // If the stop class is not present, use root
         if (typeInfo.BaseType == stopClass) return typeInfo.DeclaredProperties;
         return typeInfo.DeclaredProperties.Union(GetAllProperties(typeInfo.BaseType.GetTypeInfo(), stopClass));
      }

      public static IEnumerable<string> GetAllBaseTypeNames(this TypeInfo typeInfo, Type stopClass = null)
      {
         var retList = new List<string>();
         if (typeInfo == null) { return retList; }
         stopClass = stopClass ?? typeof(object); // If the stop class is not present, use root
         if (typeInfo.BaseType == stopClass)
         {
            return retList;
         }
         retList.Add(typeInfo.BaseType.Name);
         return retList.Union(GetAllBaseTypeNames(typeInfo.BaseType.GetTypeInfo(), stopClass));
      }

      public static IEnumerable<TypeInfo> GetAllBaseTypes(this TypeInfo typeInfo, Type stopClass = null)
      {
         var retList = new List<TypeInfo>();
         if (typeInfo == null) { return retList; }
         stopClass = stopClass ?? typeof(object); // If the stop class is not present, use root
         if (typeInfo.BaseType == stopClass)
         {
            return retList;
         }
         retList.Add(typeInfo.BaseType.GetTypeInfo());
         return retList.Union(GetAllBaseTypes(typeInfo.BaseType.GetTypeInfo(), stopClass));
      }

      public static TypeInfo MakeGenericType(this TypeInfo type, TypeInfo genericType)
      {
         if (!type.ContainsGenericParameters)
         { throw new InvalidOperationException("Type does not have generic paramters"); }
         if (type.GenericTypeParameters.Count() != 1)
         { throw new InvalidOperationException("Wrong number of expected generic parameters"); }
         var newType = type.MakeGenericType(new Type[] { genericType });
         return newType.GetTypeInfo();
      }


      public static T CreateInstanceOfType<T>(this TypeInfo type, TypeInfo genericType, params object[] parameters)
          where T : class // current restriction to ref types so exact type returned
      {
         var newType = MakeGenericType(type, genericType);
         return CreateInstanceOfType<T>(newType, parameters);
      }

      public static T CreateInstanceOfType<T>(this TypeInfo type, params object[] parameters)
          where T : class // current restriction to ref types so exact type returned
      {
         var constructors = type.DeclaredConstructors;
         var constructor = constructors.Where(x => ParameterTypesMatch(x, parameters)).FirstOrDefault();
         if (constructor == null) throw new InvalidOperationException("Parameters do not match constructor signature");
         object ret = constructor.Invoke(parameters);
         return ret as T;
      }

      public static T CreateInstanceOfType<T>(params object[] parameters)
          where T : class // current restriction to ref types so exact type returned
      {
         var type = typeof(T).GetTypeInfo();
         var constructors = type.DeclaredConstructors;
         var constructor = constructors.Where(x => ParameterTypesMatch(x, parameters)).FirstOrDefault();
         if (constructor == null) throw new InvalidOperationException("Parameters do not match constructor signature");
         object ret = constructor.Invoke(parameters);
         return ret as T;
      }

      private static bool ParameterTypesMatch(MethodBase constructor, object[] parameters)
      {
         var expectedParams = constructor.GetParameters();
         if (expectedParams.Length != parameters.Length) return false;
         var i = 0;
         foreach (var expected in expectedParams)
         {
            if (!expected.ParameterType.IsAssignableFrom(parameters[i].GetType())) return false;
            i++;
         }
         return true;
      }

      public static void AssignPropertyValue(object obj, string propertyName, object value)
      {
         var type = obj.GetType();
         var propInfo = type.GetProperty(propertyName);

         // The following lines is critical when there is a get; private set; in a base class. 
         // DO NOT REMOVE THE NEXT LINES WITHOUT TESTING THE SCENARIO ABOVE.
         var declaringType = propInfo.DeclaringType;
         if (declaringType != null && declaringType != type)
         { propInfo = propInfo.DeclaringType.GetProperty(propertyName); }

         if (value != null)
         {
            // Adjust integers
            var propType = propInfo.PropertyType;
            var valueType = value.GetType();
            if (!propType.IsEnum && TypeIsInteger(valueType) && TypeIsInteger(propType))
            {
               var maxAllowedValue = InvokeGenericMethod(typeof(ReflectionHelpers).GetTypeInfo(), "GetMaxValue", propType, null);
               value = Convert.ChangeType(value, propType);
               //if (value<= maxAllowedValue ) { value = Convert.ChangeType(value, propType) ;  }
            }
         }

         propInfo.SetValue(obj, value,
             BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.SetProperty,
             null, null, null);
      }

      public static bool TypeIsInteger(Type type)
      {
         switch (Type.GetTypeCode(type))
         {
         case TypeCode.Byte:
         case TypeCode.SByte:
         case TypeCode.UInt16:
         case TypeCode.UInt32:
         case TypeCode.UInt64:
         case TypeCode.Int16:
         case TypeCode.Int32:
         case TypeCode.Int64:
            return true;
         }

         return false;
      }

      public static T GetMaxValue<T>()
      {
         var type = typeof(T);
         if (type.IsEnum)
         { type = type.GetEnumUnderlyingType(); }
         var maxValueField = type.GetField("MaxValue");
         var maxValue = maxValueField.GetValue(null);
         return (T)maxValue;
      }


      public static object InvokeGenericMethod(TypeInfo type, string methodName, Type innerType, object obj, params object[] parameters)
      {
         var bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
         var methodInfo = type.GetMethod(methodName, bindingFlags);
         if (methodInfo == null) throw new InvalidOperationException("Method not found");
         var concreteInfo = methodInfo.MakeGenericMethod(innerType);
         return concreteInfo.Invoke(obj, parameters);
      }

   }
}
