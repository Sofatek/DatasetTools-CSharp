using System;
using System.Collections.Generic;
using System.IO;

namespace DatasetTools
{
   public class FileTools
   {
      /// <summary>
      /// Get All lines of the file. Each line is tranformed by the Func if exists
      /// </summary>
      public static List<string> GetAllLines(string fullFilename, Func<string, string> stringAction = null)
      {
         List<string> retList = new List<string>();

         // Read
         FileStream fs = File.Open(fullFilename, FileMode.Open, FileAccess.Read, FileShare.Read);
         using (BufferedStream bs = new BufferedStream(fs, 1024 * 1024))
         using (StreamReader sr = new StreamReader(bs))
         {
               string sLine;
               while ((sLine = sr.ReadLine()) != null)
               {
                  if (stringAction != null)
                  {
                     sLine = stringAction(sLine);
                  }

                  if (!String.IsNullOrEmpty(sLine) && !sLine.StartsWith("#"))
                  {
                     retList.Add(sLine);
                  }
               }
               sr.Close();
               sr.Dispose();
         }
         fs.Close();
         fs.Dispose();

         return retList;
      }

      /// <summary>
      /// Get All lines of the file filtered by the condition Func
      /// </summary>
      public static List<string> GetFilteredLines(string fullFilename, Func<string, bool> condAction)
      {
         List<string> retList = new List<string>();

         // Read
         FileStream fs = File.Open(fullFilename, FileMode.Open, FileAccess.Read, FileShare.Read);
         using (BufferedStream bs = new BufferedStream(fs, 1024 * 1024))
         using (StreamReader sr = new StreamReader(bs))
         {
               string sLine;
               while ((sLine = sr.ReadLine()) != null)
               {
                  if (condAction(sLine))
                  {
                     retList.Add(sLine);
                  }
                  
               }
               sr.Close();
               sr.Dispose();
         }
         fs.Close();
         fs.Dispose();

         return retList;
      }

      /// <summary>
      /// Get All lines of the list filtered by the condition Func
      /// </summary>
      public static List<string> GetFilteredLines(List<string> lines, Func<string, bool> condAction)
      {
         List<string> retList = new List<string>();

         foreach (var line in lines)
         {
               if (condAction(line))
               {
                  retList.Add(line);
               }
         }

         return retList;
      }

      /// <summary>
      /// Delete all file of the list
      /// </summary>
      public static void DeleteAllFiles(List<string> listOfFullFilename)
      {
         foreach (var fullFilename in listOfFullFilename)
         {
               try
               {
                  System.IO.File.Delete(fullFilename);
               }
               catch { }
         }
      }

      /// <summary>
      /// Delete all file given in arguments (the number of arguments is free)
      /// </summary>
      public static void DeleteAllFiles(params string[] args)
      {
         for (int i=0; i<args.Length; i++)
         {
               try
               {
                  System.IO.File.Delete(args[i]);
               }
               catch { }
         }
      }        
   }
}
