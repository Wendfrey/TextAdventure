using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TextAdventure
{
    class FileReader
    {
        public static string[] ReadFile(string path)
        {
            List<string> resultado = new List<string>();
            string temp = "";
            using (StreamReader fs = File.OpenText(path))
            {
                while ((temp = fs.ReadLine()) != null)
                {
                    resultado.Add(temp);
                }
            }
            return resultado.ToArray();
        }

        public static string RandomDescr(string path)
        {
            string[] frases = ReadFile(path);
            return frases[CustomMath.RandomIntNumber(frases.Length-1)];
        }
    }
}
