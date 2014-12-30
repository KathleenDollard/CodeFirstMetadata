using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeFirstMetadataT4Support;
using RoslynDom.Common;
using System.IO;
using Microsoft.CodeAnalysis.MSBuild;
using System.Linq;
using System.Collections.Generic;

namespace DomainGenerationTests
{
   [TestClass]
   public class DomainGenerationTests
   {
      [TestMethod]
      public void Should_create_domain_file_from_project()
      {
         var runner = new T4TemplateRunner();
         var startDirectory = Path.Combine(FileSupport.ProjectPath(AppDomain.CurrentDomain.BaseDirectory), "..\\DomainGenerationMetadata");
         startDirectory = Path.GetFullPath(startDirectory);
         var ws = MSBuildWorkspace.Create();
         var projectPath = FileSupport.GetNearestCSharpProject(startDirectory);
         // For now: wait for the result
         var project = ws.OpenProjectAsync(projectPath).Result;
         var dict = runner.CreateOutputStringsFromProject(project, "..\\Output");
         AssertCreation(dict);
      }

      [TestMethod]
      public void Should_create_domain_file_from_from_path()
      {
         var runner = new T4TemplateRunner();
         var relativePath = "..\\DomainGenerationMetadata";
         var dict = runner.CreateOutputStringsFromProject(relativePath, "..\\Output");
         AssertCreation(dict);
      }

      [TestMethod]
      private void AssertCreation(IDictionary<string, string> dict)
      {
         Assert.AreEqual(2, dict.Count());
         var actual = dict
                        .Where(x => x.Key.EndsWith("Customer2.g.cs"))
                        .First()
                        .Value ;
         actual = StringUtilities.RemoveFileHeaderComments(actual);
         Assert.AreEqual(expected1, actual);
         actual = dict
                        .Where(x => x.Key.EndsWith("Customer.g.cs"))
                        .First()
                        .Value;
         actual = StringUtilities.RemoveFileHeaderComments(actual);
         Assert.AreEqual(expected2, actual);
      }

      private string expected1 = @"using System;
using System.ComponentModel;

namespace CodeFirstTest
{
   public class Customer : INotifyPropertyChanged
   {
      public event PropertyChangedEventHandler PropertyChanged;

      private String firstName
      public public String FirstName
      {
         get { return firstName; }
         set { SetProperty(ref firstName, value); }
      }

      private String lastName
      public public String LastName
      {
         get { return lastName; }
         set { SetProperty(ref lastName, value); }
      }

      private Int32 id
      public public Int32 Id
      {
         get { return id; }
         set { SetProperty(ref id, value); }
      }

      private DateTime birthDate
      public public DateTime BirthDate
      {
         get { return birthDate; }
         set { SetProperty(ref birthDate, value); }
      }

   }
}
";

      private string expected2 = @"using System;
using System.ComponentModel;

namespace CodeFirstTest
{
   
   public class Customer : INotifyPropertyChanged
   
   {
      private String _firstName;
      private String _lastName;
      private Int32 _id;
      private DateTime _birthDate;
      
      public event PropertyChangedEventHandler PropertyChanged;
      
      protected virtual void OnPropertyChanged(string name)
      {
          if (this.PropertyChanged != null)
          {
              this.PropertyChanged(this, new PropertyChangedEventArgs(name));
          }
      }
      
      public String FirstName
      {
         get { return _firstName; }
         set
         {
            if (_firstName != value)
            {
               _firstName = value;
               this.OnPropertyChanged(""FirstName"");
            }
         }
      }
      public String LastName
      {
         get { return _lastName; }
         set
         {
            if (_lastName != value)
            {
               _lastName = value;
               this.OnPropertyChanged(""LastName"");
            }
         }
      }
      public Int32 Id
      {
         get { return _id; }
         set
         {
            if (_id != value)
            {
               _id = value;
               this.OnPropertyChanged(""Id"");
            }
         }
      }
      public DateTime BirthDate
      {
         get { return _birthDate; }
         set
         {
            if (_birthDate != value)
            {
               _birthDate = value;
               this.OnPropertyChanged(""BirthDate"");
            }
         }
      }
   }
}
";

   }
}
