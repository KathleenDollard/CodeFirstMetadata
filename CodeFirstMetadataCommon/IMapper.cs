﻿using RoslynDom.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.Common
{
   public interface IMapper
   {
      IEnumerable<Type> SupportedTypes { get; }
      CodeFirstMetadata Map(TargetChildMapping mapping, IDom source);
   }

   public interface IMapper<out T> : IMapper 
      where T : CodeFirstMetadata
   {
   }
}
