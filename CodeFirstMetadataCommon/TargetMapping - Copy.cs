using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstMetadata.Common
{
    public class TargetMappings : TargetMapping
    {
        private List<TargetMapping> _mappings = new List<TargetMapping>();

        public TargetMappings(TypeInfo typeInfo) : base(typeInfo)
        { }

        public TargetMappings(string mappingId,
                    TypeInfo targetTypeInfo,
                    string targetName,
                    TargetLevel targetLevel,
                    bool allowMultiple = false)
            : base(mappingId, targetTypeInfo, targetName, targetLevel, allowMultiple)
        { }

        public TargetMapping AddMapping(string mappingId,  TypeInfo targetType, string targetName, TargetLevel targetLevel)
        {
            var newItem = new TargetMapping(mappingId, targetType, targetName, targetLevel);
            _mappings.Add(newItem);
            return newItem;
        }
        public TargetMapping AddChildMapping(string mappingId, TypeInfo targetType, string targetName, string attributeName, TargetLevel targetLevel, bool allowMultiple)
        {
            var newItem = new TargetMapping(mappingId, targetType, targetName, targetLevel, allowMultiple);
            _mappings.Add(newItem);
            return newItem;
        }

        public IEnumerable<TargetMapping> Mapping { get { return _mappings; } }

        public static TargetMappings DeriveMappings(TypeInfo typeInfo)
        {
            var mappings = new TargetMappings(typeInfo);
            // class -> EventSource
            // method -> Event
            // parameter -> EventParameter
            return mappings;
        }
    }

    public class TargetMapping
    {
        private string _mapingpId;
        private string _targetName;
        private TypeInfo _targetTypeInfo;
        private TargetLevel _targetLevel;
        private bool _allowMultiple;

        public TargetMapping(TypeInfo typeInfo)
        { }

        public TargetMapping(string mapingpId, 
                    TypeInfo targetTypeInfo, 
                    string targetName, 
                    TargetLevel targetLevel, 
                    bool allowMultiple=false)
        {
            _mapingpId = _mapingpId;
            _targetName = targetName;
            _targetTypeInfo = targetTypeInfo;
            _targetLevel = targetLevel;
            _allowMultiple = allowMultiple;
        }


        public string MappingId
        {
            get { return _mapingpId; }
        }

        public TypeInfo TargetTypeInfo
        {
            get { return _targetTypeInfo; }
        }

        public string TargetName
        {
            get { return _targetName; }
        }

        public TargetLevel TargetLevel
        {
            get { return _targetLevel; }
        }
    }

    public class TargetMappingChild : TargetMapping
    {
        private bool _allowMultiple;
        private IEnumerable<TargetMappings> _children = new List<TargetMappings>();

        public TargetMappingChild(string mappingId, TypeInfo targetTypeInfo, string targetName, TargetLevel targetLevel, bool allowMultiple)
            : base(mappingId, targetTypeInfo, targetName, targetLevel)
        {
            _allowMultiple = allowMultiple;
        }

        public bool AllowMultiple
        {
            get { return _allowMultiple; }
        }
    }

    public enum TargetLevel
    {
        Unknown,
        Class,
        Method,
        Property,
        Field
    }
}
