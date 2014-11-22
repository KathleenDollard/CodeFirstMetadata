using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RoslynDom.Common;

namespace CodeFirst.Common
{

    public abstract class TargetChildMapping : TargetMapping
    {
        // TODO: change to new protected and internal scope 
        private IList<TargetAttributeMapping> _attributes = new List<TargetAttributeMapping>();
        private IList<TargetChildMapping> _children = new List<TargetChildMapping>();

        internal TargetChildMapping(string name, string prefix, TypeInfo typeInfo, TargetLevel targetLevel)
                : base(name, prefix, null, true)
        {
            _targetLevel = targetLevel;
            _targetTypeInfo = typeInfo.GenericTypeArguments.First().GetTypeInfo();
            var namedProperties = GetNamedProperties().ToList();
            var properties = _targetTypeInfo.GetAllProperties();
            foreach (var prop in properties)
            {
                if (prop.Name == "NestedLevel") // I am trying to remove this
                { }
                else if (IsChild(prop))
                {
                    AddChildLevel(this, prop);
                }
                else if (
                     prop.SetMethod != null 
                     && prop.SetMethod.IsPublic
                     && !_attributes.Any(x=>x.TargetName == prop.Name))
                { _attributes.Add(new TargetAttributeMapping(prop, _mappingId)); }
                else { } // Do Nothing, it's a special readonly property
            }
        }

        private bool IsChild(PropertyInfo prop)
        {
            if(this.CanHaveChildren)
            {
                var propType = prop.PropertyType;
                if (propType.Name.StartsWith("IEnumerable"))
                {
                    var itemType = propType.GetGenericArguments().First();
                    return (typeof(CodeFirstMetadata).IsAssignableFrom(itemType));
                }
            }
            return false;
        }

        public IEnumerable<TargetAttributeMapping> Attributes
        { get { return _attributes; } }

        public virtual IEnumerable<string> GetNamedProperties()
        {
            return new List<string>() { "DefiningAttributes", "Name" };
        } 

        public IEnumerable<TargetChildMapping> Children
        { get { return _children; } }

        internal abstract bool CanHaveChildren { get; }

        internal void AddChildLevel(TargetChildMapping parent, PropertyInfo prop)
        {
            var underlyingTypeInfo = prop.PropertyType.GenericTypeArguments.First();
            var childMapping = (TargetChildMapping)DeriveMapping(prop.Name, parent._mappingId, underlyingTypeInfo.GetTypeInfo());
            parent._children.Add(childMapping);
        }

        internal void AddNamedProps()
        {
        }
    }

    // TODO: Consider root layer
  
}
