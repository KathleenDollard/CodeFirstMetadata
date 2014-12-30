using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using CodeFirst.Common;
using System.Linq;
using CodeFirstMetadataTest.PropertyChanged;

namespace CodeFirstMetadataTests
{
    [TestClass]
    public class MappingPropertyChangedTests
    {
        [TestMethod]
        public void Can_load_code_first_property_changed_classes()
        {
            var classGroupType = typeof(CodeFirstClassGroup);
            var classType = typeof(CodeFirstClass);
            var propertyType = typeof(CodeFirstProperty);
            Assert.IsNotNull(classGroupType);
            Assert.IsNotNull(classType);
            Assert.IsNotNull(propertyType);
        }

        [TestMethod]
        public void Can_create_property_changed_namespace_mapping()
        {
            var namespaceType = typeof(CodeFirstClassGroup);
            var mapping = TargetMapping.DeriveMapping("testPropParam", "xyz", namespaceType.GetTypeInfo());
            Assert.IsNotNull(mapping);
            Assert.IsInstanceOfType(mapping, typeof(TargetNamespaceMapping));
            Assert.AreEqual("testPropParam", mapping.TargetName);
            Assert.AreEqual("xyz.testPropParam", mapping.MappingId);
            Assert.AreEqual(typeof(CodeFirstClassGroup), mapping.UnderlyingTypInfo);
        }

        [TestMethod]
        public void Can_create_property_changed_class_mapping()
        {
            var propClassType = typeof(CodeFirstClass);
            var mapping = TargetMapping.DeriveMapping("testPropChange", "root", propClassType.GetTypeInfo());
            var classMapping = mapping as TargetClassMapping;
            Assert.IsNotNull(mapping);
            Assert.IsNotNull(classMapping);
            Assert.AreEqual("testPropChange", mapping.TargetName);
            Assert.AreEqual("root.testPropChange", mapping.MappingId);
            Assert.AreEqual(typeof(CodeFirstClass), mapping.UnderlyingTypInfo);
            Assert.AreEqual(1, classMapping.Children.Count());
            Assert.AreEqual(5, classMapping.Attributes.Count());

            var propertyMapping = classMapping.Children.First() as TargetPropertyMapping;
            Assert.IsNotNull(propertyMapping);
            Assert.AreEqual("Properties", propertyMapping.TargetName);
            Assert.AreEqual("root.testPropChange.Properties", propertyMapping.MappingId);
            Assert.AreEqual(typeof(CodeFirstProperty), propertyMapping.UnderlyingTypInfo);
            Assert.AreEqual(8, propertyMapping.Attributes.Count());

        }


               [TestMethod]
        public void Can_create_property_changed_property_mapping()
        {
            var propertyType = typeof(CodeFirstMetadataTest.PropertyChanged.CodeFirstProperty);
            var mapping = TargetMapping.DeriveMapping("testPropChanged", "xyz", propertyType.GetTypeInfo());
            Assert.IsNotNull(mapping);
            Assert.IsInstanceOfType(mapping, typeof(TargetPropertyMapping));
            Assert.AreEqual("testPropChanged", mapping.TargetName);
            Assert.AreEqual("xyz.testPropChanged", mapping.MappingId);
            Assert.AreEqual(typeof(CodeFirstMetadataTest.PropertyChanged.CodeFirstProperty), mapping.UnderlyingTypInfo);
        }
    }
}
