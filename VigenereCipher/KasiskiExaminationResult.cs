namespace VigenereCipher
{
    public class KasiskiExaminationResult
    {
        public string Keyword { get; }
        public string Plaintext { get; }
        public string Ciphertext { get; }

        public KasiskiExaminationResult(string plaintext, string keyword, string ciphertext)
        {
            Keyword = keyword;
            Plaintext = plaintext;
            Ciphertext = ciphertext;
        }
    }
}