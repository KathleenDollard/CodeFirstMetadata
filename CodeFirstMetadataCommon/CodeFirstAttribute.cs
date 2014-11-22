using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.Common
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CodeFirstAttribute : Attribute
    {
        public CodeFirstAttribute(string name)
        { 
            this.Name = name;
            Properties = new List<CodeFirstAttributeProperty>(); 
        }

        public string Name { get; private set; }

        public List<CodeFirstAttributeProperty> Properties {get; private set;}
    }

    public class CodeFirstAttributeProperty
    {
        public CodeFirstAttributeProperty(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }

        public CodeFirstAttributeProperty(int ordinal, object value)
        {
            this.Ordinal = ordinal;
            this.Value = value;
        }

        public string Name { get; private set; }
        public int Ordinal { get; private set; }
        public object Value { get; private set; }
    }
}
