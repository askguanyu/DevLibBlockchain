namespace DevLib.Blockchain
{
    using System.Text;

    public abstract class HashBase
    {
        public abstract byte[] ComputeHash(string input);

        public string ComputeHashString(string input)
        {
            return this.ToHashString(this.ComputeHash(input));
        }

        public virtual string ToHashString(byte[] hash)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }

            return result.ToString();
        }
    }
}
