using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VigenereCipher
{
    public static class Utils
    {
        public static string RemoveNonAlphabetChars(string text, string charset) 
        {
            string uppertext = string.Concat(text.Select(char.ToUpperInvariant));
            return
                new string(
                    uppertext.Where(c => charset.Contains(c.ToString()))
                        .ToArray());
        }

        public static string RetrieveNonAlphabetChars(string text, string template, string charset)
        {
            List<char> result = new List<char>(text.ToUpper().ToCharArray());
            List<char> source = new List<char>(template.ToUpper().ToCharArray());

            Dictionary<int, char> aChars = new Dictionary<int, char>();

            for (int i = 0; i < source.Count; i++)
            {
                if (!charset.Contains(source[i].ToString()))
                {
                    aChars[i] = source[i];
                }
            }

            foreach (var pair in aChars)
            {
                result.Insert(pair.Key, pair.Value);
            }

            for (int i = 0; i < template.Length; i++)
            {
                if (char.IsUpper(template[i]))
                {
                    result[i] = char.ToUpperInvariant(result[i]);
                }

                if (char.IsLower(template[i]))
                {
                    result[i] = char.ToLowerInvariant(result[i]);
                }
            }

            return new string(result.ToArray());
        }

        public static string TrimText(string text, string language)
        {
            string charset = GetCharset(language);
            string textTrimmed = Utils.RemoveNonAlphabetChars(text, charset);

            return textTrimmed;
        }

        public static string UntrimText(string text, string template, string language)
        {
            string charset = GetCharset(language);
            string textUntrimmed = RetrieveNonAlphabetChars(text, template, charset);

            return textUntrimmed;
        }

        public static string GetCharset(string language)
        {
            IDictionary<char, double> charFrequency;
            if (!Utils.LanguagesCharsFrequency.TryGetValue(language, out charFrequency))
            {
                throw new ArgumentException($"{nameof(GetCharset)} doesn't contain info about {language}'s charset and char frequency");
            }

            string charset = string.Join(string.Empty, charFrequency.Select(pair => pair.Key));

            return charset;
        }

        #region Dictionaries

        private static readonly IDictionary<char, double> EnglishCharsFrequency = new Dictionary<char, double>
        {
            {'A', 0.08167},
            {'B', 0.01492},
            {'C', 0.02782},
            {'D', 0.04253},
            {'E', 0.12702},
            {'F', 0.0228},
            {'G', 0.02015},
            {'H', 0.06094},
            {'I', 0.06966},
            {'J', 0.00153},
            {'K', 0.00772},
            {'L', 0.04025},
            {'M', 0.02406},
            {'N', 0.06749},
            {'O', 0.07507},
            {'P', 0.01929},
            {'Q', 0.00095},
            {'R', 0.05987},
            {'S', 0.06327},
            {'T', 0.09056},
            {'U', 0.02758},
            {'V', 0.00978},
            {'W', 0.0236},
            {'X', 0.0015},
            {'Y', 0.01974},
            {'Z', 0.00074}
        };

        private static readonly IDictionary<char, double> RussianCharsFrequency = new Dictionary<char, double>
        {
            {'А', 0.07821},
            {'Б', 0.01732},
            {'В', 0.04491},
            {'Г', 0.01698},
            {'Д', 0.03103},
            {'Е', 0.08567},
            {'Ё', 0.0007},
            {'Ж', 0.01082},
            {'З', 0.01647},
            {'И', 0.06777},
            {'Й', 0.01041},
            {'К', 0.03215},
            {'Л', 0.04813},
            {'М', 0.03139},
            {'Н', 0.0685},
            {'О', 0.11394},
            {'П', 0.02754},
            {'Р', 0.04234},
            {'С', 0.05382},
            {'Т', 0.06443},
            {'У', 0.02882},
            {'Ф', 0.00132},
            {'Х', 0.00833},
            {'Ц', 0.00333},
            {'Ч', 0.01645},
            {'Ш', 0.00775},
            {'Щ', 0.00331},
            {'Ъ', 0.00023},
            {'Ы', 0.01854},
            {'Ь', 0.02106},
            {'Э', 0.0031},
            {'Ю', 0.00544},
            {'Я', 0.01979}
        };

        public static readonly IDictionary<string, IDictionary<char, double>> LanguagesCharsFrequency = new Dictionary<string, IDictionary<char, double>>
        {
            {"English", EnglishCharsFrequency},
            {"Russian", RussianCharsFrequency}
        };

        #endregion
    }
}
