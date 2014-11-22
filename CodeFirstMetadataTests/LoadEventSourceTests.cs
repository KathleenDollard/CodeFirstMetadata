using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using CodeFirst;
using CodeFirst.Common;
using CodeFirstMetadataTest.SemanticLog;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoslynDom;
using RoslynDom.Common;
using RoslynDom.CSharp;

namespace CodeFirstMetadataTests
{
   [TestClass]
   public class LoadEventSourceTests
   {
      private string csharpCode = @"using KadMan.Common;

namespace CodeFirstTest
{
    [LocalizationResources(""Fred"")]
    [Name(""KadGen-Test-Temp"")]
    [SemanticLogEntry()]
    public class NormalEventSource
    {
        [Version(2)]
        [Keywords(3)]
        public void foo(int Bar, string Bar2) { }

        [Version(2)]
        [Keywords(3)]
        public void foo2() { }
    }
}";

      [TestMethod]
      public void Can_load_eventsource_code()
      {
         var root = RDom.CSharp.Load(csharpCode);
         Assert.IsNotNull(root);
      }

      [TestMethod]
      public void Can_load_code_eventsource_from_file()
      {
         var str = FileSupport.GetFileContents(@"..\..\EventSource1.cfcs");
         var root = RDom.CSharp.Load(str);
         Assert.IsNotNull(root);
      }

      [TestMethod]
      public void Can_map_eventsource_namespace()
      {
         var root = RDom.CSharp.Load(csharpCode);
         Assert.IsNotNull(root, "Root is null");
         var cfNamespace = root.Namespaces.First();
         Assert.IsNotNull(cfNamespace, "cfNamespace is null");
         var eventSourceType = typeof(CodeFirstEventSourceGroup);
         var namespaceMapping = TargetMapping.DeriveMapping("testEventSource", "test", eventSourceType.GetTypeInfo()) as TargetNamespaceMapping;
         Assert.IsNotNull(namespaceMapping, "namespaceMapping is null");
         var mapper = new CodeFirstMapper();
         var newObj = mapper.Map(namespaceMapping, cfNamespace);
         var newNamespace = newObj as CodeFirstEventSourceGroup;
         Assert.IsNotNull(newObj);
         Assert.IsNotNull(newNamespace);
         Assert.AreEqual("CodeFirstTest", newNamespace.Name);
         Assert.AreEqual(1, newNamespace.EventSources.Count());
      }

      [TestMethod]
      public void Can_map_eventsource_class()
      {
         var root = RDom.CSharp.Load(@"
namespace CodeFirstTest
{
    [LocalizationResources(""Fred"")]
    [EventSourceName(""KadGen-Test-Temp"")]
    [SemanticLogEntry()]
    public class NormalEventSource
    {   }
}");
         Assert.IsNotNull(root, "Root is null");
         var cfClass = root.Namespaces.First().Classes.First();
         Assert.IsNotNull(cfClass, "cfClass is null");
         var eventSourceType = typeof(CodeFirstEventSource);
         var classMapping = TargetMapping.DeriveMapping("testEventSource", "test", eventSourceType.GetTypeInfo()) as TargetClassMapping;
         var mapper = new CodeFirstMapper();
         var newObj = mapper.Map(classMapping, cfClass);
         var newEventSource = newObj as CodeFirstEventSource;
         Assert.IsNotNull(newObj);
         Assert.IsNotNull(newEventSource);
         // TODO: KD 10/20/2014 Localization fails becuase only Attribute values, not attributes with positional attribute values are recongized
         CheckEventSource(newEventSource,
                 name: "NormalEventSource",
                 isLocalized: true,
                 localizationResources: "Fred",
                 eventSourceName: "KadGen-Test-Temp",
                 namespaceName: "CodeFirstTest",
                 scopeAccess: ScopeAccess.Public,
                 comments: null,
                 xmlCommentString: null);
         Assert.AreEqual(0, newEventSource.Events.Count());
      }

      [TestMethod]
      public void Can_map_eventsource_method()
      {
         var root = RDom.CSharp.Load(csharpCode);
         Assert.IsNotNull(root, "Root is null");
         var cfMethod = root.Namespaces.First().Classes.First().Methods.First();
         Assert.IsNotNull(cfMethod, "cfClass is null");
         var eventSourceType = typeof(CodeFirstEventSource);
         var classMapping = TargetMapping.DeriveMapping("testEventSource", "test", eventSourceType.GetTypeInfo()) as TargetClassMapping;
         Assert.IsNotNull(classMapping, "classMapping is null");
         var methodMapping = classMapping.Children.First();
         Assert.IsNotNull(methodMapping, "methodMapping is null");
         var mapper = new CodeFirstMapper();
         var newObj = mapper.Map(methodMapping, cfMethod);
         var newEvent = newObj as CodeFirstEvent;
         Assert.IsNotNull(newObj);
         Assert.IsNotNull(newEvent);
         CheckEvent(newEvent,
                 name: "foo",
                 version: 2,
                 keywords: (EventKeywords)3);

         Assert.AreEqual(2, newEvent.Parameters.Count());
      }

      [TestMethod]
      public void Can_map_eventsource_parameter()
      {
         var root = RDom.CSharp.Load(csharpCode);
         Assert.IsNotNull(root, "Root is null");
         var cfParameter = root.Namespaces.First().Classes.First().Methods.First().Parameters.First();
         Assert.IsNotNull(cfParameter, "cfParameter is null");
         var eventSourceType = typeof(CodeFirstEventSource);
         var classMapping = TargetMapping.DeriveMapping("testEventSource", "test", eventSourceType.GetTypeInfo()) as TargetClassMapping;
         Assert.IsNotNull(classMapping, "classMapping is null");
         var paramterMapping = classMapping.Children.First().Children.First();
         Assert.IsNotNull(paramterMapping, "paramterMapping is null");
         var mapper = new CodeFirstMapper();
         var newObj = mapper.Map(paramterMapping, cfParameter);
         var newParameter = newObj as CodeFirstEventParam;
         Assert.IsNotNull(newObj);
         Assert.IsNotNull(newParameter);
         CheckEventParameter(newParameter,
                 name: "Bar",
                 typeName: "Int32");

      }


      private void CheckEventParameter(CodeFirstEventParam newParameter,
                  string name,
                  string typeName)
      {
         Assert.AreEqual(name, newParameter.Name, "Name is wrong");
         Assert.AreEqual(typeName, newParameter.TypeName, "TypeName  is wrong");
      }

      private void CheckEvent(CodeFirstEvent newEvent,
                  string name,
                  int version,
                  EventKeywords keywords)
      {
         Assert.AreEqual(name, newEvent.Name, "Name     is wrong");
         Assert.AreEqual(version, newEvent.Version, "Version  is wrong");
         Assert.AreEqual(keywords, newEvent.Keywords, "Keywords is wrong");
      }

      private void CheckEventSource(CodeFirstEventSource eventSource,
                  string name,
                  bool isLocalized,
                  string localizationResources,
                  string eventSourceName,
                  string namespaceName,
                  ScopeAccess scopeAccess,
                  string comments,
                  string xmlCommentString)
      {
         Assert.AreEqual(name, eventSource.Name, "Name is wrong");
         Assert.AreEqual(localizationResources, eventSource.LocalizationResources, "LocalizationResources is wrong");
         Assert.AreEqual(isLocalized, eventSource.IsLocalized, "IsLocalized is wrong");
         Assert.AreEqual(eventSourceName, eventSource.EventSourceName, "EventSourceName is wrong");
         Assert.AreEqual(namespaceName, eventSource.Namespace, "Namespace is wrong");
         Assert.AreEqual(scopeAccess, eventSource.ScopeAccess, "ScopeAccess is wrong");
         Assert.AreEqual(comments, eventSource.Comments, "Comments is wrong");
         Assert.AreEqual(xmlCommentString, eventSource.XmlCommentString, "XmlCommentString is wrong");

      }


   }
}
