using CodeFirst.Common;

namespace CodeFirstMetadataTest.Diagnostic
{

   public class CodeFirstDiagnosticGroup : CodeFirstDiagnosticGroupBase, ICodeFirstEntry
   {
      // TODO: Supply custom code
      // TODO: Provide way for multi-property notification
      public string AttributeId
      { get { return "DiagnosticAndCodeFix"; } }
   }


}
