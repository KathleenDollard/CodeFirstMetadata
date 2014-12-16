﻿namespace Examples.SemanticLog
{
   [SemanticLog()]
   [UniqueName("MyUniqueName")]
   public class Normal
   {
      public void Message(string Message) { }

      [EventId(3)]
      public void AccessByPrimaryKey(int PrimaryKey) { }
   }

}
