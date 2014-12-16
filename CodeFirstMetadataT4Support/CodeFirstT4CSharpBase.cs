using System;
using KadGen.Common;
using RoslynDom.Common;

namespace CodeFirst.Common
{
   public class CodeFirstT4CSharpBase<T> : RuntimeT4CSharpBase, ITemplate<T>
        where T : CodeFirstMetadata<T>
   {
      public T Meta { get; set; }

      public Type MetadataType
      { get { return typeof(T); } }

      protected override void OutputGenerationWarning()
      {
         // TODO: This is a crappy standard header, do better
         this.WriteLine("// This file was generated, if you change it your changes are toast");
         this.WriteLine(string.Format("// Generation was last done on {0} using template {1}",
             DateTime.Today, this.GetType().Name));
         this.WriteLine();
      }

      protected void OutputClassPrefix()
      {
         OutputXmlComments(Meta);
         OutputAttributes(Meta);
      }

      protected void OutputClassSuffix()
      { }

      protected void OutputXmlComments(CodeFirstMetadata metaNode)
      {
         var block = metaNode.XmlCommentString;
         if (block != null)
         {
            var lines = block.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            { this.WriteLine("///" + line); }
         }
      }

      protected void OutputNamespaceOpen()
      {
         var classMeta = Meta as CodeFirstMetadataClass<T>;
         if (classMeta == null) return; // perhaps an error
         if (classMeta.Namespace != null)
         {
            this.PushBracket("namespace " + classMeta.Namespace);
         }
         this.WriteLine();
      }

      protected void OutputNamespaceClose()
      {
         var classMeta = Meta as CodeFirstMetadataClass<T>;
         if (classMeta.Namespace != null) this.PopBracket();
      }

      protected void OutputAttributes(CodeFirstMetadata metaNode)
      {
         foreach (var attr in metaNode.AdditionalAttributes)
         {
            var constructor = GetAttributeConstructor(attr);
            this.WriteLine(string.Format("[{0}]", constructor));
         }
      }

      public string GetOutput(T metadata)
      {
         Meta = metadata;
         return TransformText();
      }
   }
}
