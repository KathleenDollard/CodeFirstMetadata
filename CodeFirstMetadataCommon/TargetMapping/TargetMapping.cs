using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RoslynDom.Common;

namespace CodeFirst.Common
{
   public class TargetMapping
   {
      // TODO: change to new protected and internal scope 
      internal string _idSeparator = ".";
      internal bool _allowMultiple;
      internal string _mappingId;
      internal string _targetName;
      internal TypeInfo _targetTypeInfo;
      internal TargetLevel _targetLevel;

      internal TargetMapping(string targetName, string prefix, TypeInfo targetTypeInfo, bool allowMultiple = false)
      {
         _mappingId = (string.IsNullOrWhiteSpace(prefix) ? "" : prefix + _idSeparator) + targetName;
         _allowMultiple = allowMultiple;
         _targetName = targetName;
         _targetTypeInfo = targetTypeInfo;
      }

      public TypeInfo UnderlyingTypInfo { get { return _targetTypeInfo; } }
      public string TargetName { get { return _targetName; } }
      public string MappingId { get { return _mappingId; } }

      public static TargetMapping DeriveMapping(string targetName, string prefix, TypeInfo typeInfo)
      {
         TargetMapping mapping = null;
         var baseTypes = typeInfo.GetAllBaseTypes().Where(x => x.IsGenericType);
         var firstGenericType = baseTypes.First();
         // TODO: Consider root and namespace layers or nested types, when problems are better understood
         if (TryGetMapping(firstGenericType, typeof(CodeFirstMetadataNamespace<>), typeof(TargetNamespaceMapping<>), targetName, prefix, ref mapping)) return mapping;
         if (TryGetMapping(firstGenericType, typeof(CodeFirstMetadataClass<>), typeof(TargetClassMapping<>), targetName, prefix, ref mapping)) return mapping;
         if (TryGetMapping(firstGenericType, typeof(CodeFirstMetadataMethod<>), typeof(TargetMethodMapping<>), targetName, prefix, ref mapping)) return mapping;
         if (TryGetMapping(firstGenericType, typeof(CodeFirstMetadataProperty<>), typeof(TargetPropertyMapping<>), targetName, prefix, ref mapping)) return mapping;
         if (TryGetMapping(firstGenericType, typeof(CodeFirstMetadataParameter<>), typeof(TargetParameterMapping<>), targetName, prefix, ref mapping)) return mapping;
         throw new NotImplementedException();
      }

      private static bool TryGetMapping(TypeInfo firstGenericType, Type sourceType, Type targetType,
         string targetName, string prefix, ref TargetMapping mapping)
      {
         if (firstGenericType.GetGenericTypeDefinition() == sourceType.GetTypeInfo())
         {
            // Not crazy about this, but a lot of mapping work happens in the constructors here
            mapping = ReflectionHelpers.CreateInstanceOfType<TargetMapping>(targetType.GetTypeInfo(),
                     firstGenericType, targetName, prefix, firstGenericType);
            return true;
         }
         return false;
      }
   }

}
