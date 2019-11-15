using MasterTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DatasetTools
{
    public class DatasetTools
    {
        /// <summary>
        ///  Perform replacement within the string that contains functions. Functions are declared with curly bracket like that {myFunction} or {myFunction(5)}
        ///  For instance: 
        ///  nous somme le {today(5)}
        ///  becomes
        ///  nous sommes le 23/11/2017             => (i.e. today + 5 days)
        /// </summary>
        static public string[] ProcessData(string[] dataArrayExpression)
        {
            List<string> ret = new List<string>();

            foreach (string s in dataArrayExpression)
            {
                ret.Add(ProcessData(s));
            }

            return ret.ToArray();
        }

        /// <summary>
        /// ProcessData interprète la chaine de caractère dataExpression
        /// Si elle contient {} alors le contenu est interprété :
        ///     Si {#xxxx} => cela correspond à une clé qui sera remplacé à partir d'une dictionnaire replacementDico
        ///     Si {xxx} => cela correspond une fonction de transformation
        /// Exemple :
        ///     1/ "Voici un exemple pour illustrer : L'auteur du livre est {#author}"
        ///     => "Voici un exemple pour illustrer : L'auteur du livre est Antoine De Saint-Exupéry" si la clé author est dans le dictionnaire replacementDico et qu'il contient "Antoine De Saint-Exupéry"
        ///     2/ "Voici un exemple d'utilisation de ddj soit la date-du-jour : Demain nous serons le {ddj(1)}"
        ///     => "Voici un exemple d'utilisation de ddj soit la date-du-jour : Demain nous serons le 26/06/2018"  (aujourdhui = 25/06/2018)
        /// Note : il ne doit jamais y avoir d'espace après { sinon c'est ignoré (par exemple { ddj(1)} ne sera pas interprété)
        /// </summary>
        /// <param name="dataExpression"></param>
        /// <param name="replacementDico"></param>
        /// <returns></returns>
        static public string ProcessData(string dataExpression, Dictionary<string, string> replacementDico=null)
        {
            // Parse dataExpression
            // Could be {ddj}
            // Could be {ddj(-1)}
            // Could be {ddj} france
            // Could be {ddj(5)} france

            // Check validity
            int nBrace1 = dataExpression.Count(c => c == '{');
            int nBrace2 = dataExpression.Count(c => c == '}');

            if (nBrace1 != nBrace2 && (nBrace1 + nBrace2 != 0))
            {
                throw new Exception("Open curly brace or close curly brace is missing");
            }

            if (nBrace1 == 0)
            {
                // Nothing to do
                return dataExpression;
            }

            int nParenthesis = dataExpression.Count(c => c == '(');
            int nParenthesis2 = dataExpression.Count(c => c == ')');
            if (nParenthesis != nParenthesis2)
            {
                throw new Exception("Open parenthesis or close parenthesis is missing");
            }

            // Replace { and } by ;{ and }; (assume count of bracket is good)
            // then split
            string sep = ";|%$£";
            dataExpression = dataExpression.Replace("{", sep+"{").Replace("}", "}"+sep);
            string[] substrings = dataExpression.Split(new[] { sep }, StringSplitOptions.None);

            string sRet = "";
            foreach (string s in substrings)
            {
                if (string.IsNullOrEmpty(s))
                {
                    continue;
                }

                // If contains
                if (s.Contains("{"))
                {
                    nParenthesis = s.Count(c => c == '(');
                    nParenthesis2 = s.Count(c => c == ')');
                    if (nParenthesis > 1 || nParenthesis2 > 1)
                    {
                        throw new Exception("Too much parentheses in " + s.Trim());
                    }

                    // Replace from dico or function
                    if (s.StartsWith("{#"))
                    {
                        string s2 = "";
                        try
                        {
                            s2 = replacementDico[s.Replace("{", "").Replace("}", "")];
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Trying to get key " + s + "...");
                            Console.WriteLine(e);
                        }

                        sRet += s2;
                    }
                    else
                    {
                        Regex regex = new Regex(@"^{[a-zA-Z]");
                        Match match = regex.Match(s);
                        if (match.Success)
                        {
                            sRet += GetResultFunc(s.Trim());
                        }
                        else
                        {
                            sRet += s;
                        }
                        
                    }                    
                }
                else
                {
                    sRet += s;
                }
            }

            return sRet;
        }

        static private string GetResultFunc(string func)
        {
            // Could be {ddj}
            // Could be {ddj(-1)}

            var pattern = @"{(.*?)(\((.*?)\))?}";
            var match = Regex.Match(func, pattern);
            string nameFunc = match.Groups[1].Value;
            string paramFunc = null;

            if (!string.IsNullOrEmpty(match.Groups[3].Value))
            {
                paramFunc = match.Groups[3].Value;
            }            

            // Case sensitive
            switch (nameFunc.ToLower().Trim())
            {
                case "ddj":
                case "today":
                    if (paramFunc != null)
                    {
                        string[] arr = paramFunc.Split(',');
                        if (arr.Length > 1)
                        {
                            return MTools.Today(Int32.Parse(arr[0]), arr[1]);
                        }
                        else
                        {
                            return MTools.Today(Int32.Parse(paramFunc));
                        }
                    }
                    return MTools.Today();                    
                case "randdate":
                case "randDate":
                    return MTools.RandDate(Int32.Parse(paramFunc));
                case "randan":
                case "randAn":
                case "randAN":
                case "randAlphaNum":
                    return MTools.RandAlphaNum(Int32.Parse(paramFunc));
                case "randnum":
                case "randNum":
                case "randNUM":
                    return  MTools.RandNUM(Int32.Parse(paramFunc));
                case "randstr":
                case "randStr":
                case "randSTR":
                    return MTools.RandSTR(Int32.Parse(paramFunc));
                case "randu1str":
                case "randU1Str":
                    return MTools.FirstCharToUpper(MTools.RandSTR(Int32.Parse(paramFunc)));
                case "siren":
                    return MTools.Siren();
                case "randcountry":
                case "randCountry":
                    return MTools.RandCountry();
                default:
                    match = Regex.Match(func, "^[a-zA-Z]+");
                    if (!match.Success) 
                        return func;    // Les cas ne commançant pas par une lettre tel que {3} ou {,3} sont ignorés

                    throw new Exception("Unknow function " + func + ". Check syntax. Currently List is: today, ddj (same as today), randan, randnum, randstr, randu1str, siren, randcountry");
            }
        }
    }
}
