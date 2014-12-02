namespace CodeFirst.Common
{
   public interface ICodeFirstMetadataParameter : ICodeFirstMetadata
   {
        string TypeName { get; set; }
        string Name { get; set; }
    }

}
