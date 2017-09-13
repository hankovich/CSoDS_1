using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VigenereCipher
{
    public class VigenereAlgorithm
    {
        public string Encrypt(string plaintext, string keyword, string language)
        {
            string trimmedText = Utils.TrimText(plaintext, language);
            keyword = Utils.TrimText(keyword, language);
            string charset = Utils.GetCharset(language);

            List<char> chars = new List<char>();

            for (int i = 0; i < trimmedText.Length; i++)
            {
                chars.Add(charset.ElementAt((charset.IndexOf(trimmedText[i]) + charset.IndexOf(keyword[i % keyword.Length])) % charset.Length));        
            }

            string untrimmedText = Utils.UntrimText(string.Concat(chars), plaintext, language);
            return untrimmedText;
        }

        public string Decrypt(string ciphertext, string keyword, string language)
        {
            string trimmedText = Utils.TrimText(ciphertext, language);
            keyword = Utils.TrimText(keyword, language);
            string charset = Utils.GetCharset(language);

            List<char> chars = new List<char>();

            for (int i = 0; i < trimmedText.Length; i++)
            {
                chars.Add(charset.ElementAt((charset.IndexOf(trimmedText[i]) - charset.IndexOf(keyword[i % keyword.Length]) + charset.Length) % charset.Length));
            }

            string untrimmedText = Utils.UntrimText(string.Concat(chars), ciphertext, language);
            return untrimmedText;
        }
    }
}
