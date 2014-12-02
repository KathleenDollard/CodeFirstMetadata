namespace CodeFirst.Common
{
   public interface ICodeFirstMetadataNamespace : ICodeFirstMetadata
   {
      // By convention, one namespace per file
      string FilePath { get; set; }
   }

}
