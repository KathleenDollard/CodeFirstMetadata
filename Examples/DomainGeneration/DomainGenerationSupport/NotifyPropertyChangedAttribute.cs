using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainGeneration.Metadata
{
   [AttributeUsage (AttributeTargets.Class)]
   public class NotifyPropertyChangedAttribute :Attribute 
   {
   }
}
