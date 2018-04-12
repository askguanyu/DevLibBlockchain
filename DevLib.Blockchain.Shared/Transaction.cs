namespace DevLib.Blockchain
{
    public class Transaction
    {
        public Transaction()
        {
        }

        public Transaction(string fromAddress, string toAddress, decimal amount)
        {
            this.FromAddress = fromAddress;
            this.ToAddress = toAddress;
            this.Amount = amount;
        }

        public string FromAddress { get; set; }

        public string ToAddress { get; set; }

        public decimal Amount { get; set; }
    }
}
