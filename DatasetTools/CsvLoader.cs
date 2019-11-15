using System;
using System.Collections.Generic;
using System.IO;

namespace DatasetTools
{
    public static class CsvLoader
    {
        public static List<string> Load(CsvInfo csvInfo)
        {
            List<string> retList = _Preproc(csvInfo, null);

            return retList;
        }

        public static int LoadProcessDataAndSave(CsvInfo csvInfo, string ouputFullFilename, Dictionary<string, string> replacementDico=null)
        {
            Func<string, string> func = (line) => { return DatasetTools.ProcessData(line, replacementDico); };

            List<string> retList = _Preproc(csvInfo, func);

            // Then Save 
            System.IO.File.WriteAllLines(ouputFullFilename, retList);

            return retList.Count;
        }

        public static List<string> LoadAndProcessData(CsvInfo csvInfo, Dictionary<string, string> replacementDico=null)
        {
            Func<string, string> func = (line) => { return DatasetTools.ProcessData(line, replacementDico); };

            List<string> retList = _Preproc(csvInfo, func);

            return retList;
        }

        private static List<string> _Preproc(CsvInfo csvInfo, Func<string, string> stringAction)
        {
            string fullFillname = csvInfo.fullFilename;

            List<string> retList = new List<string>();

            int nHeaderLines = csvInfo.headerCount;

            // Load CSV File
            try
            {
                //var reader = new StreamReader(fullFillname);
                var stream = new FileStream(fullFillname, FileMode.Open, FileAccess.Read,
                        FileShare.ReadWrite);

                var reader = new StreamReader(stream);

                // Skip header
                while (!reader.EndOfStream && nHeaderLines > 0)
                {
                    var line = reader.ReadLine();
                    if (stringAction != null)
                    {
                        retList.Add(line);
                    }
                        
                    nHeaderLines--;
                }

                // Reads data lines
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    try
                    {
                        line = line.Trim();

                        // Skip special lines
                        if (line.StartsWith("#") || String.IsNullOrEmpty(line))
                        {
                            // It is a comment => skip 
                            // Empty line => skip
                            continue;
                        }

                        if (stringAction != null)
                        {
                            line = stringAction(line); // Entire line. Not each field by each field
                                                       //line = DatasetTools.PerformData(line); // Entire line. Not each field by each field
                        }

                        retList.Add(line);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return retList;
        }
    }
}
