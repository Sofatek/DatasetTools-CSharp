using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatasetTools
{
    public class CsvInfo
    {
        // Chemin complet du fichier
        public string fullFilename { get; set; }

        // Nombre de lignes d'entête. Attention 1 par défaut
        public int headerCount { get; set; }

        // Séparateur de champs. ';' par défaut
        public char separator { get; set; }

        public CsvInfo()
        {
            fullFilename = "";
            headerCount = 1;
            separator = ';';
        }
    }
}
