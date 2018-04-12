namespace DevLib.Blockchain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BlockChain
    {
        public BlockChain()
        {
            this.Blocks.Insert(0, this.CreateGenesisBlock());
        }

        public List<Block> Blocks { get; set; } = new List<Block>();

        public List<Transaction> PendingTransactions { get; set; } = new List<Transaction>();

        public int Difficulty { get; set; } = 2;

        public decimal MiningReward { get; set; } = 100;

        public Block CreateGenesisBlock()
        {
            return new Block(null, "0", DateTimeOffset.MinValue);
        }

        public Block GetLastBlock()
        {
            return this.Blocks.Last();
        }

        public void MinePendingTransactions(string miningRewardAddress)
        {
            var block = new Block(this.PendingTransactions, this.GetLastBlock().Hash);
            block.MineBlock(this.Difficulty);
            this.Blocks.Add(block);
            this.PendingTransactions = new List<Transaction>
            {
                new Transaction(null, miningRewardAddress, this.MiningReward)
            };
        }

        public void CreateTransaction(Transaction transaction)
        {
            this.PendingTransactions.Add(transaction);
        }

        public decimal GetBalanceOfAddress(string address)
        {
            decimal balance = 0;

            foreach (var block in this.Blocks)
            {
                foreach (var transaction in block.Transactions)
                {
                    if (transaction.FromAddress == address)
                    {
                        balance -= transaction.Amount;
                    }

                    if (transaction.ToAddress == address)
                    {
                        balance += transaction.Amount;
                    }
                }
            }

            return balance;
        }

        public bool IsChainValid()
        {
            for (int i = 1; i < this.Blocks.Count; i++)
            {
                var currentBlock = this.Blocks[i];
                var previousBlock = this.Blocks[i - 1];

                if (currentBlock.Hash != currentBlock.CalculateHash())
                {
                    return false;
                }

                if (currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
