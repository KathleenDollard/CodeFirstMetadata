using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RoslynDom.Common;

namespace CodeFirst.Common
{
   
    public class TargetAttributeMapping : TargetMapping
    {

        private PropertyInfo _propertyInfo;

        internal TargetAttributeMapping(PropertyInfo propInfo, string prefix)
            : base(propInfo.Name, prefix, propInfo.PropertyType.GetTypeInfo(), false)
        {
            _propertyInfo = propInfo;
        }

    }

}
