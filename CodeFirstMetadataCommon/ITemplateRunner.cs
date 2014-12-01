using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.Common
{
    public interface ITemplateRunner
    {
      // TODO: Major mechanism should probably return a syntax tree
      IDictionary<string, string> CreateOutputStringsFromFiles(string inputRootDirectory, string outputRootDirectory, bool noRecurse = false, bool whatIf = false);
      IDictionary<string, string> CreateOutputStringsFromProject(Project project, string outputRootDirectory, bool whatIf = false);
   }
}
