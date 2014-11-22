using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using CodeFirst;
using CodeFirst.Common;
using CodeFirstMetadataTest.PropertyChanged;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoslynDom;
using RoslynDom.Common;
using RoslynDom.CSharp;

namespace CodeFirstMetadataTests
{
   [TestClass]
   public class LoadPropChangedTests
   {
      private string csharpCode = @"using KadMan.Common;

namespace CodeFirstTest
{
    [NotifyPropertyChanged]
    public class Customer
    {
        /// <summary>
        /// This is the first name
        /// </summary>
        public string FirstName{get; set;}

        [TestAttribute]
        public string LastName{get; set;}

        public int  Id{get; set;}
        public DateTime  BirthDate{get; set;}
    }
}";

      [TestMethod]
      public void Can_load_prop_changed_code()
      {
         var root = RDom.CSharp.Load(csharpCode);
         Assert.IsNotNull(root);
      }

      [TestMethod]
      public void Can_load_prop_changed_code_from_file()
      {
         var str = FileSupport.GetFileContents(@"..\..\PropertyChanged1.cfcs");
         var root = RDom.CSharp.Load(str);
         Assert.IsNotNull(root);
      }

      [TestMethod]
      public void Can_map_prop_changed_namespace()
      {
         var root = RDom.CSharp.Load(csharpCode);
         Assert.IsNotNull(root, "Root is null");
         var cfNamespace = root.Namespaces.First();
         Assert.IsNotNull(cfNamespace, "cfNamespace is null");
         var classType = typeof(CodeFirstClassGroup);
         var namespaceMapping = TargetMapping.DeriveMapping("testPropChanged", "test", classType.GetTypeInfo()) as TargetNamespaceMapping;
         Assert.IsNotNull(namespaceMapping, "namespaceMapping is null");
         var mapper = new CodeFirstMapper();
         var newObj = mapper.Map(namespaceMapping, cfNamespace);
         var newNamespace = newObj as CodeFirstClassGroup;
         Assert.IsNotNull(newObj);
         Assert.IsNotNull(newNamespace);
         Assert.AreEqual("CodeFirstTest", newNamespace.Name);
         Assert.AreEqual(1, newNamespace.Classes.Count());
      }

      [TestMethod]
      public void Can_map_prop_changed_class()
      {
         var root = RDom.CSharp.Load(@"
namespace CodeFirstTest
{
    /// <summary>
    /// This is the class
    /// </summary>
    public class Customer
    {   }
}");
         Assert.IsNotNull(root, "Root is null");
         var cfClass = root.Namespaces.First().Classes.First();
         Assert.IsNotNull(cfClass, "cfClass is null");
         var propertyType = typeof(CodeFirstClass);
         var classMapping = TargetMapping.DeriveMapping("testPropChanged", "test", propertyType.GetTypeInfo()) as TargetClassMapping;
         var mapper = new CodeFirstMapper();
         var newObj = mapper.Map(classMapping, cfClass);
         var newClass = newObj as CodeFirstClass;
         Assert.IsNotNull(newObj);
         Assert.IsNotNull(newClass);
         var expectedXmlComment = "<member name=\"T:CodeFirstTest.Customer\">\r\n    <summary>\r\n    This is the class\r\n    </summary>\r\n</member>\r\n";

         CheckClass(newClass,
                 className: "Customer",
                 namespaceName: "CodeFirstTest",
                 scopeAccess: ScopeAccess.Public,
                 comments: null,
                 xmlCommentString: expectedXmlComment);
         Assert.AreEqual(0, newClass.Properties.Count());
      }

      [TestMethod]
      public void Can_map_prop_changed_property()
      {
         var root = RDom.CSharp.Load(csharpCode);
         Assert.IsNotNull(root, "Root is null");
         var cfProperties = root.Namespaces.First().Classes.First().Properties.ToArray();
         Assert.AreEqual(4, cfProperties.Count());
         var eventSourceType = typeof(CodeFirstClass);
         var classMapping = TargetMapping.DeriveMapping("testPropChanged", "test", eventSourceType.GetTypeInfo()) as TargetClassMapping;
         Assert.IsNotNull(classMapping, "classMapping is null");
         var propertyMapping = classMapping.Children.First();
         Assert.IsNotNull(propertyMapping, "propertyMapping is null");
         var mapper = new CodeFirstMapper();
         var newObj = mapper.Map(propertyMapping, cfProperties[0]);
         var newProperty = newObj as CodeFirstProperty;
         Assert.IsNotNull(newObj);
         Assert.IsNotNull(newProperty);
         CheckProperty(newProperty,
                 name: "FirstName",
                 typeName: "string");

      }

      private void CheckProperty(CodeFirstProperty newProperty,
             string name,
             string typeName)
      {
         Assert.AreEqual(name, newProperty.Name, "Name     is wrong");
         //  Assert.AreEqual(typeName, newProperty.TypeName, "TypeName  is wrong");
      }

      private void CheckClass(CodeFirstClass eventSource,
                  string className,
                  string namespaceName,
                  ScopeAccess scopeAccess,
                  string comments,
                  string xmlCommentString)
      {
         Assert.AreEqual(className, eventSource.Name, "ClassName is wrong");
         Assert.AreEqual(namespaceName, eventSource.Namespace, "Namespace is wrong");
         Assert.AreEqual(scopeAccess, eventSource.ScopeAccess, "ScopeAccess is wrong");
         Assert.AreEqual(comments, eventSource.Comments, "Comments is wrong");
         Assert.AreEqual(xmlCommentString, eventSource.XmlCommentString, "XmlCommentString is wrong");

      }

   }
}
