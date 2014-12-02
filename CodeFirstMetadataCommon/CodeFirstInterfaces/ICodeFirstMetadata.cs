using System.Collections.Generic;

namespace CodeFirst.Common
{
   public interface ICodeFirstMetadata
   {
      ICodeFirstMetadata Parent { get; }
      IEnumerable<ICodeFirstMetadata> Children { get; }
      IEnumerable<ICodeFirstMetadata> Ancestors { get; }
      IEnumerable<ICodeFirstMetadata> AncestorsAndSelf { get; }
      IEnumerable<ICodeFirstMetadata> Descendants { get; }
      IEnumerable<ICodeFirstMetadata> DescendantsAndSelf { get; }
   }
}