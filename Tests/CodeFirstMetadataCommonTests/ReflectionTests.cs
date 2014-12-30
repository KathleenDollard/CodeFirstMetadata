using System;
using CodeFirst.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Reflection;
using RoslynDom.Common;

namespace CodeFirstMetadataCommonTests
{
    [TestClass]
    public class ReflectionTests
    {
        private const string GetInfoCategory = "GetInfo";
        private const string ConstructAndAssignCategory = "ConstructAndAssign";
        private const string PlaygroundCategory = "Playground";

        #region get info
        [TestMethod]
        [TestCategory(GetInfoCategory)]
        public void GetAllProperties_should_include_base_and_derived_properties()
        {
            var typeInfo = typeof(B).GetTypeInfo();
            var props = ReflectionHelpers.GetAllProperties(typeInfo);
            Assert.AreEqual(4, props.Count());
        }

        [TestMethod]
        [TestCategory(GetInfoCategory)]
        public void GetAllProperties_respects_stop_class()
        {
            var typeInfo = typeof(B).GetTypeInfo();
            var props = ReflectionHelpers.GetAllProperties(typeInfo, typeof(A));
            Assert.AreEqual(2, props.Count());
        }

        [TestMethod]
        [TestCategory(GetInfoCategory)]
        public void GetAllProperties_returns_null_for_null_type_info()
        {
            var props = ReflectionHelpers.GetAllProperties(null, typeof(A));
            Assert.IsNull(props);
        }

        [TestMethod]
        [TestCategory(GetInfoCategory)]
        public void GetAllBaseTypeNames_should_include_base_and_derived_names()
        {
            var typeInfo = typeof(C).GetTypeInfo();
            var names = ReflectionHelpers.GetAllBaseTypeNames(typeInfo).ToArray();
            Assert.AreEqual(2, names.Count());
            Assert.AreEqual("B", names[0]);
            Assert.AreEqual("A", names[1]);
        }

        [TestMethod]
        [TestCategory(GetInfoCategory)]
        public void GetAllBaseTypeNames_respects_stop_class()
        {
            var typeInfo = typeof(C).GetTypeInfo();
            var names = ReflectionHelpers.GetAllBaseTypeNames(typeInfo, typeof(A)).ToArray();
            Assert.AreEqual(1, names.Count());
            Assert.AreEqual("B", names[0]);
        }

        [TestMethod]
        [TestCategory(GetInfoCategory)]
        public void GetAllBaseTypeNames_returns_empty_list_for_null_type_info()
        {
            var names = ReflectionHelpers.GetAllBaseTypeNames(null);
            Assert.AreEqual(0, names.Count());
        }

        [TestMethod]
        [TestCategory(GetInfoCategory)]
        public void GetAllBaseTypes_should_include_base_and_derived_names()
        {
            var typeInfo = typeof(C).GetTypeInfo();
            var typeInfos = ReflectionHelpers.GetAllBaseTypes(typeInfo).ToArray();
            Assert.AreEqual(2, typeInfos.Count());
            Assert.IsInstanceOfType(typeInfos[0], typeof(TypeInfo));
            Assert.AreEqual("B", typeInfos[0].Name);
            Assert.AreEqual("A", typeInfos[1].Name);
        }

        [TestMethod]
        [TestCategory(GetInfoCategory)]
        public void GetAllBaseTypes_respects_stop_class()
        {
            var typeInfo = typeof(C).GetTypeInfo();
            var typeInfos = ReflectionHelpers.GetAllBaseTypes(typeInfo, typeof(A)).ToArray();
            Assert.AreEqual(1, typeInfos.Count());
            Assert.AreEqual("B", typeInfos[0].Name);
        }

        [TestMethod]
        [TestCategory(GetInfoCategory)]
        public void GetAllBaseTypes_returns_empty_list_for_null_type_info()
        {
            var typeInfos = ReflectionHelpers.GetAllBaseTypes(null);
            Assert.AreEqual(0, typeInfos.Count());
        }

        #endregion

        #region construct and assign
        [TestMethod]
        [TestCategory(ConstructAndAssignCategory)]

        public void Can_construct_generic_type()
        {
            var newType = ReflectionHelpers.MakeGenericType(typeof(D<>).GetTypeInfo(), typeof(A).GetTypeInfo());
            var testType = typeof(D<A>);
            Assert.AreEqual(testType, newType);
        }

        [TestMethod]
        [TestCategory(ConstructAndAssignCategory)]
        public void Can_create_instance_of_generic_type()
        {
            var newInstance = ReflectionHelpers.CreateInstanceOfType<C>(
                typeof(D<>).GetTypeInfo(), typeof(A).GetTypeInfo(), "Fred", 42);
            Assert.IsInstanceOfType(newInstance, typeof(D<A>));
        }

        [TestMethod]
        [TestCategory(ConstructAndAssignCategory)]
        public void Can_create_instance_of_type()
        {
            var newInstance = ReflectionHelpers.CreateInstanceOfType<C>(
                typeof(C).GetTypeInfo());
            Assert.IsInstanceOfType(newInstance, typeof(C));
        }

        [TestMethod]
        [TestCategory(ConstructAndAssignCategory)]
        public void Can_create_instance_of_type_simple()
        {
            var newInstance = ReflectionHelpers.CreateInstanceOfType<C>();
            Assert.IsInstanceOfType(newInstance, typeof(C));
        }

        [TestMethod]
        [TestCategory(ConstructAndAssignCategory)]
        public void Can_assign_value_to_public_property()
        {
            var obj = new B();
            ReflectionHelpers.AssignPropertyValue(obj, "Bar", "Fred");
            Assert.AreEqual("Fred", obj.Bar);
        }

        [TestMethod]
        [TestCategory(ConstructAndAssignCategory)]
        public void Can_assign_value_to_public_property_in_base()
        {
            var obj = new B();
            ReflectionHelpers.AssignPropertyValue(obj, "Foo", "Fred");
            Assert.AreEqual("Fred", obj.Foo);
        }

        [TestMethod]
        [TestCategory(ConstructAndAssignCategory)]
        public void Can_assign_value_to_private_property()
        {
            var obj = new B();
            ReflectionHelpers.AssignPropertyValue(obj, "Bar2", "Fred");
            Assert.AreEqual("Fred", obj.Bar2);
        }


        [TestMethod]
        [TestCategory(ConstructAndAssignCategory)]
        public void Can_assign_value_to_private_property_in_base()
        {
            var obj = new B();
            ReflectionHelpers.AssignPropertyValue(obj, "Foo2", "Fred");
            Assert.AreEqual("Fred", obj.Foo2 );
        }

        [TestMethod]
        [TestCategory(ConstructAndAssignCategory)]
        public void Can_run_specified_method()
        {
            var obj = new Z();
            var ret = (int)ReflectionHelpers.InvokeGenericMethod(typeof(Z).GetTypeInfo(), "Foo1", typeof(string).GetTypeInfo(), obj, "Fred");
            Assert.AreEqual(31, ret);
        }

        #endregion

        #region playground
        [TestMethod]
        [TestCategory(PlaygroundCategory)]

        public void Record_nested_inferrence()
        {
            var z = new Z();
            C x = new D<string>("", 42);
            C y = new E<D<string>>();
            z.Foo1(x);
            // The following won't work 
            //z.Foo2(x);
            //z.Foo3(y);
        }
        #endregion

        private class A
        {
            public string Foo { get; set; }
            public string Foo2 { get; private set; }
            public void BaseInstanceMethod() { }
        }

        private class B : A
        {
            public string Bar { get; set; }
            public string Bar2 { get; private set; }
            public string TestInstanceMethod() { return "Hello"; }
            public static string TestStaticMethod() { return "Goodbye"; }
            public string TestInstanceMethodWithParams(int val) { return val.ToString(); }
            public void GenericMethod<T>() { }
        }

        private class C : B
        { }

        private class D<T> : C
        {
            internal D(string foo, int bar) { }
        }

        private class E<T2> : D<T2>
        { public E() : base("", 42) { } }

        private class Z
        {
            public int Foo1<T>(T p1)
            { return 31; }
            public void Foo2<T>(D<T> p1)
            { }
            public void Foo3<T>(E<D<T>> p1)
            { }
        }
    }
}
