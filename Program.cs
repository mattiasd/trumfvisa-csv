using System;
using System.IO;
using System.Text;

namespace trumfvisa_csv
{
    class Program
    {
        static void Main(string[] args)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Dato;Tekst;Beløp;Referense;Rentedato");

            var lines = File.ReadAllLines(args[0]);

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                line = line.Replace("Bank-terminal", "");

                if (line.IndexOf(".18") == 5 && line.EndsWith(".18"))
                {
                    line = FixDate(line.Substring(0, 8)) + ";" + line.Substring(9);
                    line = ReplaceLastOccurrence(line, " ", ";");
                    line = ReplaceLastOccurrence(line, " ", ";");

                    sb.AppendLine(line.Replace(" +", ";+").Replace(" -", ";-"));
                }
                else if (line.IndexOf(".18") == 5)
                {
                    string value = ReplaceFirst(line, " ", ";");
                    sb.Append( FixDate(value.Substring(0, 8)) + ";" + value.Substring(9));
                }
                else if (line.StartsWith('+') || line.StartsWith('-'))
                {
                    line = ReplaceLastOccurrence(line, " ", ";");
                    line = ReplaceLastOccurrence(line, " ", ";");
                    sb.AppendLine(";" + line);
                }
                else
                {
                    sb.Append(" " + line);
                }
            }

            Console.Write(sb.ToString());
        }

        private static string FixDate(string s)
        {
            var parts = s.Split('.');

            return parts[2] + "-" + parts[1] + "-" + parts[0];
        }

        public static string ReplaceFirst(string s, string find, string replace)
        {
            var first = s.IndexOf(find);
            return s.Substring(0, first) + replace + s.Substring(first + find.Length);
        }
        public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
        {
            int place = Source.LastIndexOf(Find);

            if (place == -1)
                return Source;

            string result = Source.Remove(place, Find.Length).Insert(place, Replace);
            return result;
        }
    }
}
