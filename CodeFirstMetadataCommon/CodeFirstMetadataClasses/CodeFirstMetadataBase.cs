using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;
using System.Xml.Linq;
using RoslynDom.Common;

namespace CodeFirst.Common
{


   public abstract class CodeFirstMetadata : ICodeFirstMetadata
   {
      public CodeFirstMetadata()
      {
         AdditionalAttributes = new List<CodeFirstAttribute>();
      }

      public string XmlCommentString { get; set; }

      private List<Type> _definingAttributes = new List<Type>();
      public IEnumerable<Type> DefiningAttributes
      { get { return _definingAttributes; } }

      protected Type AddDefiningAttribute(Type type)
      {
         if (typeof(Attribute).IsAssignableFrom(type))
         {
            _definingAttributes.Add(type);
            return type;
         }
         throw new InvalidOperationException("Type is not an attribute");
      }

      public List<CodeFirstAttribute> AdditionalAttributes { get; private set; }

      public  ICodeFirstMetadata Parent { get; set; }

      public IEnumerable<ICodeFirstMetadata> Descendants
      {
         get
         {
            var list = new List<ICodeFirstMetadata>();
            var children = this.Children;
            foreach (var child in children)
            { list.AddRange(child.DescendantsAndSelf); }
            return list;
         }
      }

      public IEnumerable<ICodeFirstMetadata> DescendantsAndSelf
      {
         get
         {
            var list = new List<ICodeFirstMetadata>();
            list.Add(this);
            list.AddRange(Descendants);
            return list;
         }
      }

      public virtual IEnumerable<ICodeFirstMetadata> Children
      { get { return new List<ICodeFirstMetadata>(); } }

      public virtual IEnumerable<ICodeFirstMetadata> Ancestors
      {
         get
         {
            if (Parent == null) { return new List<ICodeFirstMetadata>(); } // top/end of recursion
            return Parent.AncestorsAndSelf;
         }
      }

      public IEnumerable<ICodeFirstMetadata> AncestorsAndSelf
      {
         get
         {
            var list = Ancestors.ToList();
            list.Insert(0, this);
            return list;
         }
      }

      protected void FixAttributes()
      {
         var type = this.GetType().GetTypeInfo();
         foreach (var attrType in this.DefiningAttributes)
         {
            var attrName = attrType.Name.Replace("Attribute", "");
            var additional = AdditionalAttributes.Where(x => x.Name == attrName).FirstOrDefault();
            if (additional == null)
            {
               additional = new CodeFirstAttribute(attrName);
               AdditionalAttributes.Add(additional);
            }
            var propInfos = attrType.GetTypeInfo().DeclaredProperties.Where(x => x.CanWrite);
            foreach (var propInfo in propInfos)
            {
               var prop = additional.Properties.Where(x => x.Name == propInfo.Name).FirstOrDefault();
               if (prop == null)
               {
                  var thisPropInfo = type.DeclaredProperties.Where(x => x.Name == propInfo.Name).FirstOrDefault();
                  if (thisPropInfo != null)
                  {
                     var value = thisPropInfo.GetValue(this);
                     if (thisPropInfo.PropertyType == typeof(string))
                     { value = "\"" + value + "\""; }
                     additional.Properties.Add(new CodeFirstAttributeProperty(propInfo.Name, value));
                  }
               }
            }
         }
      }

      public bool ValidateAndUpdate()
      {
         FixAttributes();
         if (!ValidateAndUpdateCore()) return false;
         IEnumerable<CodeFirstMetadata> children = GetChildren();
         foreach (var child in children)
         {
            // TODO: Provide more sophistacated reporting and don't just abort
            if (!child.ValidateAndUpdate()) return false;

         }
         return true;
      }

      private IEnumerable<CodeFirstMetadata> GetChildren()
      {
         var ret = new List<CodeFirstMetadata>();
         var props = ReflectionHelpers
           .GetAllProperties(this.GetType().GetTypeInfo(), typeof(CodeFirstMetadata)).ToArray();
         foreach (var prop in props)
         {
            var propertyType = prop.PropertyType;
            if (propertyType.IsGenericType
                && propertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
               var innerType = propertyType.GetGenericArguments().First();
               if (typeof(CodeFirstMetadata).IsAssignableFrom(innerType))
               {
                  ret.AddRange((IEnumerable<CodeFirstMetadata>)prop.GetValue(this));
               }
            }
         }
         return ret;
      }

      protected virtual bool ValidateAndUpdateCore()
      { return true; }

   }

   public abstract class CodeFirstMetadata<T> : CodeFirstMetadata where T : CodeFirstMetadata<T>
   {

   }

}
