using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.Common
{
   public static class Pluralizer
   {
      private static PluralizationService pluralizeService = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));
      public static string Pluralize(string input)
      {         return pluralizeService.Pluralize(input);      }
      public static string Singularize(string input)
      { return pluralizeService.Singularize (input); }
      public static bool IsPlural(string input)
      { return pluralizeService.IsPlural(input); }
      public static bool IsSingular(string input)
      { return pluralizeService.IsSingular(input); }
      public static CultureInfo Culture(string input)
      { return pluralizeService.Culture; }
   }
}
