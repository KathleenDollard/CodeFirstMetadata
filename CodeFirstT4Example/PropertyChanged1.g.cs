// This file was generated, if you change it your changes are toast
// Generation was last done on 11/13/2014 12:00:00 AM using template PropertyChangedTemplate

using System;
using System.ComponentModel;

namespace CodeFirstTest
{
   
   public class Customer : INotifyPropertyChanged
   
   {private String _firstName;
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
            if ( _firstName != value )
            { _firstName = value;
               this.OnPropertyChanged("FirstName");
            }
         }
      }
      public String LastName
      {  
         get { return _lastName; }
         set
         {
            if ( _lastName != value )
            { _lastName = value;
               this.OnPropertyChanged("LastName");
            }
         }
      }
      public Int32 Id
      {  
         get { return _id; }
         set
         {
            if ( _id != value )
            { _id = value;
               this.OnPropertyChanged("Id");
            }
         }
      }
      public DateTime BirthDate
      {  
         get { return _birthDate; }
         set
         {
            if ( _birthDate != value )
            { _birthDate = value;
               this.OnPropertyChanged("BirthDate");
            }
         }
      }
      
   }
}


