using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using CodeFirst.Common;
using System.Linq;
using CodeFirstMetadataTest.SemanticLog;

namespace CodeFirstMetadataTests
{
    [TestClass]
    public class MappingEventSourceTests
    {
        [TestMethod]
        public void Can_load_code_first_classes()
        {
            var eventSourceGroupType = typeof(CodeFirstEventSourceGroup);
            var eventSourceType = typeof(CodeFirstEventSource);
            var eventParamType = typeof(CodeFirstEventParam);
            var eventType = typeof(CodeFirstEvent);
            Assert.IsNotNull(eventSourceGroupType);
            Assert.IsNotNull(eventSourceType);
            Assert.IsNotNull(eventParamType);
            Assert.IsNotNull(eventType);
        }

        [TestMethod]
        public void Can_create_namespace_mapping()
        {
            var namespaceType = typeof(CodeFirstEventSourceGroup);
            var mapping = TargetMapping.DeriveMapping("testEventParam", "xyz", namespaceType.GetTypeInfo());
            Assert.IsNotNull(mapping);
            Assert.IsInstanceOfType(mapping, typeof(TargetNamespaceMapping));
            Assert.AreEqual("testEventParam", mapping.TargetName);
            Assert.AreEqual("xyz.testEventParam", mapping.MappingId);
            Assert.AreEqual(typeof(CodeFirstEventSourceGroup), mapping.UnderlyingTypInfo);
        }

        [TestMethod]
        public void Can_create_class_mapping()
        {
            var eventSourceType = typeof(CodeFirstEventSource);
            var mapping = TargetMapping.DeriveMapping("testEventSource", "root", eventSourceType.GetTypeInfo());
            var classMapping = mapping as TargetClassMapping;
            Assert.IsNotNull(mapping);
            Assert.IsNotNull(classMapping);
            Assert.AreEqual("testEventSource", mapping.TargetName);
            Assert.AreEqual("root.testEventSource", mapping.MappingId);
            Assert.AreEqual(typeof(CodeFirstEventSource), mapping.UnderlyingTypInfo);
            Assert.AreEqual(1, classMapping.Children.Count());
            Assert.AreEqual(7, classMapping.Attributes.Count());

            var methodMapping = classMapping.Children.First() as TargetMethodMapping;
            Assert.IsNotNull(methodMapping);
            Assert.AreEqual("Events", methodMapping.TargetName);
            Assert.AreEqual("root.testEventSource.Events", methodMapping.MappingId);
            Assert.AreEqual(typeof(CodeFirstEvent), methodMapping.UnderlyingTypInfo);
            Assert.AreEqual(1, methodMapping.Children.Count());
            Assert.AreEqual(13, methodMapping.Attributes.Count());

            var paramMapping = methodMapping.Children.First() as TargetParameterMapping;
            Assert.IsNotNull(methodMapping);
            Assert.AreEqual("Parameters", paramMapping.TargetName);
            Assert.AreEqual("root.testEventSource.Events.Parameters", paramMapping.MappingId);
            Assert.AreEqual(typeof(CodeFirstEventParam), paramMapping.UnderlyingTypInfo);
            Assert.AreEqual(0, paramMapping.Children.Count());
            Assert.AreEqual(3, paramMapping.Attributes.Count());
        }

        [TestMethod]
        public void Can_create_method_mapping()
        {
            var eventSourceType = typeof(CodeFirstEvent);
            var mapping = TargetMapping.DeriveMapping("testEvent", "abc", eventSourceType.GetTypeInfo());
            Assert.IsNotNull(mapping);
            Assert.IsInstanceOfType(mapping, typeof(TargetMethodMapping));
            Assert.AreEqual("testEvent", mapping.TargetName);
            Assert.AreEqual("abc.testEvent", mapping.MappingId);
            Assert.AreEqual(typeof(CodeFirstEvent), mapping.UnderlyingTypInfo);
        }

        [TestMethod]
        public void Can_create_parameter_mapping()
        {
            var eventSourceType = typeof(CodeFirstEventParam);
            var mapping = TargetMapping.DeriveMapping("testEventParam", "xyz", eventSourceType.GetTypeInfo());
            Assert.IsNotNull(mapping);
            Assert.IsInstanceOfType(mapping, typeof(TargetParameterMapping));
            Assert.AreEqual("testEventParam", mapping.TargetName);
            Assert.AreEqual("xyz.testEventParam", mapping.MappingId);
            Assert.AreEqual(typeof(CodeFirstEventParam), mapping.UnderlyingTypInfo);
        }

        [TestMethod]
        public void Can_create_property_mapping()
        {
            var propertyType = typeof(CodeFirstMetadataTest.PropertyChanged.CodeFirstProperty);
            var mapping = TargetMapping.DeriveMapping("testEventParam", "xyz", propertyType.GetTypeInfo());
            Assert.IsNotNull(mapping);
            Assert.IsInstanceOfType(mapping, typeof(TargetPropertyMapping));
            Assert.AreEqual("testEventParam", mapping.TargetName);
            Assert.AreEqual("xyz.testEventParam", mapping.MappingId);
            Assert.AreEqual(typeof(CodeFirstMetadataTest.PropertyChanged.CodeFirstProperty), mapping.UnderlyingTypInfo);
        }
    }
}
