using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RoslynDom.Common;

namespace CodeFirst.Common
{
    public enum TargetLevel
    {
        Unknown,
        Namespace,
        Class,
        Method,
        Property,
        Parameter
    }
}
