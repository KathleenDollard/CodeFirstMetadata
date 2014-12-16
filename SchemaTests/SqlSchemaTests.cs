using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlSchema;
using System.Linq;

namespace SchemaTests
{
   [TestClass]
   public class SqlSchemaTests
   {
      [TestMethod]
      public void Can_get_mapping()
      {
         var map = SqlLoader.LoadFromConnectionString(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=""C:\Users\Kathleen\Documents\Visual Studio 2013\Projects\CodeFirstMetadata\SchemaTests\TestDatabase.mdf"";Integrated Security=True;Connect Timeout=30");
         Assert.AreEqual(7, map.Tables.Count());
         Assert.AreEqual(2, map.Views.Count());
         Assert.AreEqual(4, map.Tables.First().Columns.Count());
         Assert.AreEqual(5, map.Views.First().Columns.Count());
      }
   }
}
