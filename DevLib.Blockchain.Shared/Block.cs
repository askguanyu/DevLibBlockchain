namespace DevLib.Blockchain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Block
    {
        private HashBase _hash = new Hash256();

        public string PreviousHash { get; set; }

        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public decimal Nonce { get; set; } = 0;

        public string Hash { get; private set; }

        public Block()
        {
            this.Hash = this.CalculateHash();
        }

        public Block(IEnumerable<Transaction> transactions, string previousHash = null, DateTimeOffset? timestamp = null)
        {
            this.PreviousHash = previousHash;
            this.Timestamp = timestamp ?? DateTimeOffset.UtcNow;
            this.Transactions = transactions?.ToList() ?? new List<Transaction>();
            this.Hash = this.CalculateHash();
        }

        public string CalculateHash()
        {
            var result = _hash.ComputeHashString(
                $"PreviousHash-{PreviousHash}-Timestamp-{Timestamp.Ticks}-Nonce-{Nonce}-Transactions-{Transactions.SerializeXmlString()}");
            return result;
        }

        public void MineBlock(int difficulty)
        {
            while (this.Hash.Substring(0, difficulty) != new string('0', difficulty))
            {
                this.Nonce++;
                this.Hash = this.CalculateHash();
            }
        }
    }
}
