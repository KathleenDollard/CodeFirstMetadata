﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// TODO: Attempt to remove this line after generating base class
using System.Diagnostics.Tracing;
using CodeFirst.Common;
using System.ComponentModel;

namespace CodeFirstMetadataTest.Diagnostic
{
   // TODO: Generate this base class based on expected attributes
   public class CodeFirstDiagnosticBase : CodeFirstMetadataClass<CodeFirstDiagnostic>
   {
      public CodeFirstDiagnosticBase()
      {
         Analyzers = new List<CodeFirstAnalyzer>();
         Fixers = new List<CodeFirstFix>();
      }

      public string Id { get; set; }
      public string Description { get; set; }
      public string MessageFormat { get; set; }
      public string Category { get; set; }
      public IEnumerable<CodeFirstAnalyzer> Analyzers { get; private set; }
      public IEnumerable<CodeFirstFix> Fixers { get; private set; }

   }

}
