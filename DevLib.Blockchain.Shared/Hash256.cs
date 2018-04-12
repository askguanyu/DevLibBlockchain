namespace DevLib.Blockchain
{
    using System.Security.Cryptography;
    using System.Text;

    public class Hash256 : HashBase
    {
        public override byte[] ComputeHash(string input)
        {
            using (SHA256 sha = SHA256Managed.Create())
            {
                return sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            }
        }
    }
}
