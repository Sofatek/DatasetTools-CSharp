using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DatasetTools
{
    public static class MTools
    {
      private static Random random = new Random();

      public static string PrettyMilliseconds(long milliseconds)
      {
         TimeSpan t = TimeSpan.FromMilliseconds(milliseconds);

         string f = "";
         bool bPureSeconds = (milliseconds % 1000) == 0;
         if (!bPureSeconds)
         {
               f = "{3:D3}ms";
         }

         // Add seconds
         if (milliseconds > 1000)
         {
               int nChar = 2;
               if (milliseconds < 10000)
               {
                  nChar = 1;
               }

               string fSeconds = string.Format("{{2:D{0}}}s", nChar);

            if (bPureSeconds)
               {
                  f = fSeconds;
               }
               else
               {
                  f = fSeconds + " " + f;
               }
         }

         bool bPureMinutes = (milliseconds % 60000) == 0;

         // Add minutes
         if (milliseconds > 60000)
         {
               int nChar = 2;
               if (milliseconds < 600000)
               {
                  nChar = 1;
               }

               string fMinutes = string.Format("{{1:D{0}}}mn", nChar);

               if (bPureMinutes)
               {
                  f = fMinutes;
               }
               else
               {
                  f = fMinutes + " " + f;
               }
         }


         bool bPureHours = (milliseconds % 3600000) == 0;
         if (milliseconds > 3600000)
         {
               int nChar = 2;
               if (milliseconds < 36000000)
               {
                  nChar = 1;
               }

               string fHours = string.Format("{{0:D{0}}}h", nChar);

               if (bPureHours)
               {
                  f = fHours;
               }
               else
               {
                  f = fHours + " " + f;
               }

         }

         //string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
         string answer = string.Format(f,
                                 t.Hours,
                                 t.Minutes,
                                 t.Seconds,
                                 t.Milliseconds);

         return answer;
      }

      public static string Today(int n=0, string format = null)
      {
         DateTime time = DateTime.Now;
         time = time.AddDays(n);
         if (string.IsNullOrEmpty(format))
         {
               format = "dd/MM/yyy";
         }

         return time.ToString(format);
      }

      public static string Today(TodayAdd todayAdd, int n, string format = null)
      {
         if (todayAdd == TodayAdd.DAY)
         {
               return Today(n, format);
         }
         else
         {
               DateTime time = DateTime.Now;
               switch(todayAdd)
               {
                  case TodayAdd.MONTH:
                     time = time.AddMonths(n);
                     break;
                  case TodayAdd.YEAR:
                     time = time.AddYears(n);
                     break;
               }

               if (string.IsNullOrEmpty(format))
               {
                  format = "dd/MM/yyy";
               }

               return time.ToString(format);
         }
      }

      /// <summary>
      /// Add or remove days to a fr date
      /// </summary>
      /// <param name="frDate"></param>
      /// <param name="n"></param>
      /// <param name="format"></param>
      /// <returns></returns>
      public static string FrDateAdd(string frDate, int n, string format=null)
      {
         string ret = "";
         string[] tab = frDate.Split('/');
         if (tab.Length == 3)
         {
               DateTime time = new DateTime(Int32.Parse(tab[2]), Int32.Parse(tab[1]), Int32.Parse(tab[0]));
               time = time.AddDays(n);

               if (string.IsNullOrEmpty(format))
               {
                  format = "dd/MM/yyy";
               }

               ret = time.ToString(format);
         }

         return ret;
      }

      /// <summary>
      /// Indicates if the fr date is currently a past date or not
      /// </summary>
      public static bool IsPastFrDate(string frDate)
      {
         string[] tab = frDate.Split('/');
         if (tab.Length == 3)
         {
               DateTime time = new DateTime(Int32.Parse(tab[2]), Int32.Parse(tab[1]), Int32.Parse(tab[0]));
               return time < DateTime.Now;
         }
         else
         {
               throw new Exception("La date n'est pas au format fr");
         }            
      }

      /// <summary>
      /// 20180625 -> 25/06/2018
      /// </summary>
      /// <param name="dateNum"></param>
      /// <returns></returns>
      public static string DateNumToFrDate(string dateNum)
      {
         string ret = dateNum;
         if (!string.IsNullOrEmpty(dateNum) && dateNum.Length == 8)
         {
               ret = dateNum.Substring(6, 2);
               ret += "/" + dateNum.Substring(4, 2);
               ret += "/" + dateNum.Substring(0, 4);
         }

         return ret;
      }

      /// <summary>
      /// Compares two fr dates
      /// </summary>>
      /// <returns></returns>
      public static int CompareFrDate(string date1, string date2)
      {
         if (date1 == null)
         {
               return date2 == null ? 0 : -1; 
         }
         
         if (date2 == null)
         {
               return 1;
         }

         string _date1 = YyyyMmDdDate(date1);
         string _date2 = YyyyMmDdDate(date2);

         return _date1.CompareTo(_date2);
      }

      /// <summary>
      /// 30/07/2018 -> 2018/07/30
      /// </summary>
      /// <param name="frDateToConvert"></param>
      /// <returns></returns>
      public static string FrDateToYMDDate(string frDateToConvert, char sep = '/')
      {
         string ret = frDateToConvert;
         if (!string.IsNullOrEmpty(frDateToConvert) && frDateToConvert.Length == 10)
         {
               ret = frDateToConvert.Substring(6, 4);
               ret += sep + frDateToConvert.Substring(3, 2);
               ret += sep + frDateToConvert.Substring(0, 2);
         }

         return ret;
      }

      /// <summary>
      /// 30/07/2018 -> 20180730
      /// </summary>
      /// <param name="frDate"></param>
      /// <returns></returns>
      public static string FrDateToDateNum(string frDate)
      {
         string ret = frDate;
         if (!string.IsNullOrEmpty(frDate) && frDate.Length == 10)
         {
               string[] tab = frDate.Split('/');
               if (tab.Length == 3)
               {
                  ret = tab[2] + tab[1] + tab[0];
               }
         }

         return ret;
      }

      /// <summary>
      /// Returns a date (fr by default) of the year given in arguments, and applies a "random" for day and month
      /// </summary>
      /// <param name="year"></param>
      /// <returns></returns>
      public static string RandDate(int year, string format = null)
      {
         int month = random.Next(1, 13);
         int day = 1;
         switch(month)
         {
               case 1:
               case 3:
               case 5:
               case 7:
               case 8:
               case 10:
               case 12:
                  day = random.Next(1, 32);
                  break;
               case 2:
                  day = random.Next(1, 29);
                  break;
               case 4:
               case 6:
               case 9:
               case 11:
                  day = random.Next(1, 31);
                  break;
               default:
                  day = 1;
                  break;
         }
         DateTime time = new DateTime(year, month, day);

         if (string.IsNullOrEmpty(format))
         {
            format = "dd/MM/yyy";
         }

         return time.ToString(format);
      }

      public static string FirstCharToUpper(string input)
      {
         switch (input)
         {
            case null: throw new ArgumentNullException(nameof(input));
            case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
            default: return input.First().ToString().ToUpper() + input.Substring(1);
         }
      }

      public static string RandAlphaNum(int n)
      {
         const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
         return Rand(chars, n);
      }

      public static string RandNum(int n)
      {
         // Don't want a num that starts with 0
         const string chars = "123456789";
         const string chars2 = "0123456789";
         if (n==1)
         {
            return Rand(chars, 1);
         }

         return Rand(chars, n-1) + Rand(chars2, 1);
      }

      public static string RandStr(int n)
      {
         const string chars = "abcdefghijklmnopqrstuvwxyz";
         return Rand(chars, n);
      }

      private static string Rand(string baseChars, int n)
      {
         return new string(Enumerable.Repeat(baseChars, n)
         .Select(s => s[random.Next(s.Length)]).ToArray());
      }

      /// <summary>
      /// Create a valid siren ('approximately' unique)
      /// </summary>
      public static string Siren() 
      {
         // Some protections... (really ???) the two first digits will be the same within a week
         // Get week number and add 10
         DateTime time = DateTime.Now;
         DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
         Calendar cal = dfi.Calendar;
         int week = cal.GetWeekOfYear(time, dfi.CalendarWeekRule,
                                       dfi.FirstDayOfWeek);
         week += 10;
         // --

         string num = week + MTools.RandNUM(6);

         // Luhn algo : 8 + 1
         var sum = 0;
         var alt = true;
         var digits = num.ToCharArray();
         string numCtrl;

         for (int i = digits.Length - 1; i >= 0; i--)
         {
               var curDigit = (digits[i] - 48);
               if (alt)
               {
                  curDigit *= 2;
                  if (curDigit > 9)
                     curDigit -= 9;
               }
               sum += curDigit;
               alt = !alt;
         }
         if ((sum % 10) == 0)
         {
               numCtrl = "0";
         }
         else
         {
               numCtrl = (10 - (sum % 10)).ToString();
         }

         return num + numCtrl;
      }

      /// <summary>
      /// Returns a country (by random) with the possibility to exclude one 
      /// </summary>
      /// <param name="exclude"></param>
      /// <returns></returns>
      public static string RandCountry(string exclude=null)
      {
         string[] countries = { "AFGHANISTAN", "AFRIQUE DU SUD", "ALBANIE", "ALGERIE", "ALLEMAGNE", "ANDORRE", "ANGOLA", "ARABIE SAOUDITE", "ARGENTINE", "ARMENIE", "ARUBA", "AUSTRALIE", "AUTRICHE", "AZERBAIDJAN", "BAHAMAS", "BAHREIN", "BANGLADESH", "BARBADE", "BELARUS", "BELGIQUE", "BENIN", "BERMUDES", "BHOUTAN", "BOLIVIE", "BOSNIE-HERZEGOVINE", "BOTSWANA", "BRESIL", "BULGARIE", "BURKINA FASO", "BURUNDI", "CAMBODGE", "CAMEROUN", "CANADA", "CAP-VERT", "CHILI", "CHINE", "CHYPRE", "COLOMBIE", "COMORES", "CONGO", "COSTA RICA", "CROATIE", "CUBA", "DANEMARK", "DJIBOUTI", "DOMINIQUE", "EGYPTE", "EL SALVADOR", "EMIRATS ARABES UNIS", "EQUATEUR", "ERYTHREE", "ESPAGNE", "ESTONIE", "ETATS-UNIS", "ETHIOPIE", "FIDJI", "FINLANDE", "FRANCE", "GABON", "GAMBIE", "GEORGIE", "GHANA", "GIBRALTAR", "GRECE", "GRENADE", "GROENLAND", "GUATEMALA", "GUERNESEY", "GUINEE", "GUINEE EQUATORIALE", "GUINEE-BISSAU", "GUYANA", "HAITI", "HONDURAS", "HONG-KONG", "HONGRIE", "INDE", "INDONESIE", "IRAQ", "IRLANDE", "ISLANDE", "ISRAEL", "ITALIE", "JAMAIQUE", "JAPON", "JERSEY", "JORDANIE", "KAZAKHSTAN", "KENYA", "KIRGHIZISTAN", "KIRIBATI", "KOSOVO", "KOWEIT", "LESOTHO", "LETTONIE", "LIBAN", "LIBERIA", "LIECHTENSTEIN", "LITUANIE", "LUXEMBOURG", "MACAO", "MADAGASCAR", "MALAISIE", "MALAWI", "MALDIVES", "MALI", "MALTE", "MAROC", "MAURICE", "MAURITANIE", "MEXIQUE", "MONACO", "MONGOLIE", "MONTSERRAT", "MOZAMBIQUE", "MYANMAR", "NAMIBIE", "NAURU", "NEPAL", "NICARAGUA", "NIGER", "NIGERIA", "NORVEGE", "NOUVELLE-ZELANDE", "OCEAN INDIEN", "OMAN", "OUGANDA", "OUZBEKISTAN", "PAKISTAN", "PANAMA", "PAPOUASIE-NOUVELLE-GUINEE", "PARAGUAY", "PAYS-BAS", "PEROU", "PHILIPPINES", "POLOGNE", "PORTO RICO", "PORTUGAL", "QATAR", "ROUMANIE", "ROYAUME-UNI", "RWANDA", "SENEGAL", "SERBIE", "SERBIE-ET-MONTENEGRO", "SEYCHELLES", "SIERRA LEONE", "SINGAPOUR", "SLOVAQUIE", "SLOVENIE", "SOMALIE", "SOUDAN", "SOUDAN DU SUD", "SRI LANKA", "SUEDE", "SUISSE", "SURINAME", "SWAZILAND", "TCHAD", "THAILANDE", "TOGO", "TUNISIE", "TURKMENISTAN", "TURQUIE", "UKRAINE", "URUGUAY", "VENEZUELA", "YEMEN", "ZAMBIE", "ZIMBABWE" };

         if (string.IsNullOrEmpty(exclude))
         {
               return countries[random.Next(countries.Length)];
         }
         else
         {
               // Nouvel essai
               string ret = countries[random.Next(countries.Length)];
               if (ret != exclude)
               {
                  return ret;
               }
               else
               {
                  // On fait un remove pour être sûr
                  List<string> myArr = new List<string>(countries);
                  myArr.Remove(exclude);
                  string[] countries2 = myArr.ToArray();
                  return countries2[random.Next(countries2.Length)];
               }
         }
      }
    }

    public enum TodayAdd
    {
        DAY,
        MONTH,
        YEAR
    }
}
