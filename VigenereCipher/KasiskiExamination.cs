using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VigenereCipher
{
    public class KasiskiExamination
    {
        public KasiskiExamination()
        {

        }

        public int FindKeyLength(string cipherTextTrimmed)
        {
            if (string.IsNullOrWhiteSpace(cipherTextTrimmed))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(cipherTextTrimmed));
            }

            for (int i = 3; i < cipherTextTrimmed.Length; i++) // substring length
            {
                Dictionary<string, IList<int>> substrings = new Dictionary<string, IList<int>>();
                List<int> distances = new List<int>();

                for (int j = 0; j < cipherTextTrimmed.Length - i + 1; j++)
                {
                    string substring = cipherTextTrimmed.Substring(j, i);

                    if (!substrings.ContainsKey(substring))
                    {
                        substrings.Add(substring, new List<int>());
                    }

                    substrings[substring].Add(j);
                }

                foreach (var pair in substrings.Where(pair => pair.Value.Count > 1))
                {
                    for (int j = 0; j < pair.Value.Count - 1; j++)
                    {
                        distances.Add(pair.Value[j+1] - pair.Value[j]);   
                    }
                }

                int keyLength = Gcd(distances);

                if (keyLength != 1)
                {
                    return keyLength;
                }
            }

            return -1;
        }

        public KasiskiExaminationResult DecryptVigenereCipher(string ciphertext, string language)
        {
            string charset = Utils.GetCharset(language);
            string ciphertextTrimmed = Utils.RemoveNonAlphabetChars(ciphertext, charset);

            int keyLength = FindKeyLength(ciphertextTrimmed);

            if (keyLength == -1)
            {
                return new KasiskiExaminationResult(null, null, ciphertext);
            }

            List<char> keyword = new List<char>();

            Dictionary<int, List<char>> band = new Dictionary<int, List<char>>();

            for (int i = 0; i < ciphertextTrimmed.Length; i++)
            {
                if (!band.ContainsKey(i%keyLength))
                {
                    band.Add(i%keyLength, new List<char>());
                }

                band[i%keyLength].Add(ciphertextTrimmed[i]);
            }

            for (int i = 0; i < keyLength; i++)
            {
                int i1 = i;

                Dictionary<char, double> frequenciesInCurrentBand = string.Join(string.Empty, band[i].ToArray()).GroupBy(x => x).Select(x => new KeyValuePair<char, double>(x.Key, (double)x.Count() / band[i1].Count)).ToDictionary(x => x.Key, x => x.Value);

                foreach (var symbol in charset)
                {
                    if (!frequenciesInCurrentBand.ContainsKey(symbol))
                    {
                        frequenciesInCurrentBand.Add(symbol, 0);
                    }
                }
                List<KeyValuePair<char, double>> actualCharFrequencies = frequenciesInCurrentBand.OrderBy(pair => charset.IndexOf(pair.Key)).ToList();

                int offset = 0;
                double maxProbability = 0;

                for (int j = 0; j < charset.Length + 1; j++)
                {
                    double currentProbability = NaturalAlphabetProbability(Utils.LanguagesCharsFrequency[language].ToList(), actualCharFrequencies);

                    if (currentProbability > maxProbability)
                    {
                        maxProbability = currentProbability;
                        offset = j;
                    }

                    KeyValuePair<char, double> first = actualCharFrequencies.First();

                    actualCharFrequencies.RemoveAt(0);
                    actualCharFrequencies.Insert(actualCharFrequencies.Count, first);
                }

                keyword.Add(charset[(offset + charset.Length) % charset.Length]);
                
            }

            string actualKeyword = string.Concat(keyword);
            VigenereAlgorithm alg = new VigenereAlgorithm();
            string plaintext = alg.Decrypt(ciphertext, actualKeyword, language);

            return new KasiskiExaminationResult(plaintext, actualKeyword, ciphertext);
        }

        #region Private Methods

        private static double NaturalAlphabetProbability(List<KeyValuePair<char, double>> alphabet, List<KeyValuePair<char, double>> rotated)
        {
            double sum = 0;
            for (int i = 0; i < alphabet.Count; i++)
                sum += alphabet[i].Value*rotated[i].Value;
            return sum;
        }

        private static int Gcd(List<int> arrayValue)
        {
            if (arrayValue == null || arrayValue.Count < 3)
            {
                return 1;
            }

            int tempValue = Gcd(arrayValue[0], arrayValue[1]);
            for (int i = 2; i < arrayValue.Count; i++)
            {
                if (tempValue == 1)
                {
                    return tempValue;
                }
                tempValue = Gcd(tempValue, arrayValue[i]);
            }

            return tempValue;
        }

        private static int Gcd(int a, int b)
        {
            if (a == 0 && b == 0)
            {
                return 0;
            }

            a = Math.Abs(a);
            b = Math.Abs(b);

            while (b != 0)
                b = a % (a = b);

            return a;
        }

        #endregion

    }
}
