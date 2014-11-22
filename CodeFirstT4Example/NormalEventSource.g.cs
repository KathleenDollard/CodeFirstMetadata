// This file was generated, if you change it your changes are toast
// Generation was last done on 11/13/2014 12:00:00 AM using template EventSourceTemplate

using System;
using System.Diagnostics.Tracing;

namespace ConsoleRunT4Example
{
   [EventSource(Name = "ConsoleRunT4Example-Normal")]
   public sealed partial class Normal : EventSource
   {
      #region Standard class stuff
      // Private constructor blocks direct instantiation of class
      private Normal() {}
      
      // Readonly access to cached, lazily created singleton instance
      private static readonly Lazy<Normal> _lazyLog = 
              new Lazy<Normal>(() => new Normal()); 
      public static Normal Log
      {
      	get { return _lazyLog.Value; }
      }
      // Readonly access to  private cached, lazily created singleton inner class instance
      private static readonly Lazy<Normal> _lazyInnerlog = 
              new Lazy<Normal>(() => new Normal());
      private static Normal innerLog
      {
      	get { return _lazyInnerlog.Value; }
      }
      #endregion
      
      
      #region Your trace event methods
      
      [Event(1)]
      public void Message(String Message)
      {   
         if (IsEnabled()) WriteEvent(1, Message);
      }
      [Event(3)]
      public void AccessByPrimaryKey(Int32 PrimaryKey)
      {   
         if (IsEnabled()) WriteEvent(3, PrimaryKey);
      }
      #endregion
      
   }
   
   
   
}


