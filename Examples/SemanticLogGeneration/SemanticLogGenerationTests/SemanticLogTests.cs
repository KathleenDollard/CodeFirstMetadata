using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeFirst.Provider;
using CodeFirstMetadataT4Support;
using System.IO;
using RoslynDom.Common;
using Microsoft.CodeAnalysis.MSBuild;
using System.Linq;

namespace SemanticLogGenerationTests
{
   [TestClass]
   public class SemanticLogTests
   {
      [TestMethod]
      public void Should_create_file_from_semantic_log_from_project()
      {
         var runner = new T4TemplateRunner();
         var startDirectory = Path.Combine(FileSupport.ProjectPath(AppDomain.CurrentDomain.BaseDirectory), "..\\SemanticLogGenerationMetadata");
         startDirectory = Path.GetFullPath(startDirectory);
         var ws = MSBuildWorkspace.Create();
         var projectPath = FileSupport.GetNearestCSharpProject(startDirectory);
         // For now: wait for the result
         var project = ws.OpenProjectAsync(projectPath).Result;
         var dict = runner.CreateOutputStringsFromProject(project, "..\\Output");
         Assert.AreEqual(1, dict.Count());
         var actual = dict.First().Value;
         actual = StringUtilities.RemoveFileHeaderComments(actual);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Should_create_file_from_semantic_log_from_path()
      {
         var runner = new T4TemplateRunner();
         var relativePath = "..\\SemanticLogGenerationMetadata";
         var dict = runner.CreateOutputStringsFromProject(relativePath, "..\\Output");
         Assert.AreEqual(1, dict.Count());
         var actual = dict.First().Value;
         actual = StringUtilities.RemoveFileHeaderComments(actual);
         Assert.AreEqual(expected, actual);
      }
      private string expected = "using System;\r\nusing System.Diagnostics.Tracing;\r\n\r\n// UniqueName: MyUniqueName\r\n\r\nnamespace Examples.SemanticLog\r\n{\r\n   [EventSource(Name = \"MyUniqueName\")]\r\n   public sealed partial class Normal : EventSource\r\n   {\r\n      #region Standard class stuff\r\n      // Private constructor blocks direct instantiation of class\r\n      private Normal() {}\r\n      \r\n      // Readonly access to cached, lazily created singleton instance\r\n      private static readonly Lazy<Normal> _lazyLog = \r\n              new Lazy<Normal>(() => new Normal()); \r\n      public static Normal Log\r\n      {\r\n      \tget { return _lazyLog.Value; }\r\n      }\r\n      // Readonly access to  private cached, lazily created singleton inner class instance\r\n      private static readonly Lazy<Normal> _lazyInnerlog = \r\n              new Lazy<Normal>(() => new Normal());\r\n      private static Normal innerLog\r\n      {\r\n      \tget { return _lazyInnerlog.Value; }\r\n      }\r\n      #endregion\r\n      \r\n      \r\n      #region Your trace event methods\r\n      \r\n      [Event(0)]\r\n      public void Message(String Message)\r\n      {   \r\n         if (IsEnabled()) WriteEvent(0, Message);\r\n      }\r\n      [Event(3)]\r\n      public void AccessByPrimaryKey(Int32 PrimaryKey)\r\n      {   \r\n         if (IsEnabled()) WriteEvent(3, PrimaryKey);\r\n      }\r\n      #endregion\r\n      \r\n   }\r\n   \r\n   \r\n   \r\n}\r\n\r\n\r\n";

   }
}
