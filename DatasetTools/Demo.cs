using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DatasetTools
{
    class Demo
    {
        public void Run()
        {
            // Example of each tools
            Console.WriteLine("Today(): " + MTools.Today());
            Console.WriteLine("Today(): " + MTools.Today(0, "dddd, dd MMMM yyyy HH:mm:ss"));
            
            Console.WriteLine("Today(-7): " + MTools.Today(-7));
            Console.WriteLine("Today(7): " + MTools.Today(7));

            Console.WriteLine("MTools.FrDateAdd(\"21/06/2004\", 15): " + MTools.FrDateAdd("21/06/2004", 15));

            Console.WriteLine("MTools.IsPastFrDate(\"21/06/2004\"): " + MTools.IsPastFrDate("21/06/2004"));

            Console.WriteLine("MTools.DateNumToFrDate(\"20191115\"): " + MTools.DateNumToFrDate("20191115"));

            Console.WriteLine("MTools.CompareFrDate(\"21/06/2004\", \"21/07/2004\"): " + MTools.CompareFrDate("21/06/2004","21/07/2004"));

            Console.WriteLine("MTools.FrDateToYMDDate(\"21/06/2004\"): " + MTools.FrDateToYMDDate("21/06/2004"));

            Console.WriteLine("MTools.FrDateToDateNum(\"21/06/2004\"): " + MTools.FrDateToDateNum("21/06/2004"));

            Console.WriteLine("RandDate(2020): " + MTools.RandDate(2020));            
            
            Console.WriteLine("RandNum(3): " + MTools.RandNum(3));
            
            Console.WriteLine("RandAlphaNum(10): " + MTools.RandAlphaNum(10));
            
            Console.WriteLine("RandStr(10): " + MTools.RandStr(10));

            Console.WriteLine("FirstCharToUpper(\"john\"): " + MTools.FirstCharToUpper("john"));

            Console.WriteLine("Siren(): " + MTools.Siren());
            
            Console.WriteLine("RandCountry(): " + MTools.RandCountry());
            Console.WriteLine("RandCountry(\"FRANCE\"): " + MTools.RandCountry("FRANCE"));
            
            Console.WriteLine(System.Environment.NewLine);

            // Demo on csv string
            string s = "{};COM;{}  etc...
            string s = DatasetTools.ProcessData("{today};{today(5)};CHATEAUROUX;Jean-{randu1str(12)};{randstr(12)};{randnum(5)}; {randcountry}")
            Console.WriteLine("ProcessData: " + s);

            Dictionary<string, string> replacementDico = new Dictionary<string, string>();
            replacementDico.Add("author", "Antoine De Saint-Exupéry")
            s = DatasetTools.ProcessData("{today};{today(5)};CHATEAUROUX;Jean-{randu1str(12)};{randstr(12)};{randnum(5)}; {randcountry};{#author}");

            Console.WriteLine(System.Environment.NewLine);

            // Demo on csv file
            CsvInfo csvInfo = new CsvInfo();
            csvInfo.fullFilename = "demo.csv";

            var resList = CSVLoader.LoadAndProcessData(csvInfo);
            foreach(var line in resList)
            {
               Console.WriteLine(line);
            }
        }
    }
}
