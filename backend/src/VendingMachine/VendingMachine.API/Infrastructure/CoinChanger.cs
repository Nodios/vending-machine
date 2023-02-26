namespace VendingMachine.API.Infrastructure
{
    public class CoinChanger
    {
        #region Methods

        public CoinChangeResult GetChange(IEnumerable<int> denomination, int amount)
        {
            var result = new CoinChangeResult();

            // sort denominations from high to low
            denomination = denomination.OrderDescending().ToList();

            int denomIndex = 0;
            while (denomIndex < denomination.Count())
            {
                if ((amount / denomination.ElementAt(denomIndex)) <= 0)
                {
                    denomIndex++;
                    continue;
                }

                result.Change.Add(denomination.ElementAt(denomIndex));
                amount %= denomination.ElementAt(denomIndex);
            }

            // leftover of the total amount
            result.Left = amount;

            return result;
        }

        #endregion Methods

        #region Classes

        public class CoinChangeResult
        {
            #region Constructors

            public CoinChangeResult()
            {
                Change = new List<int>();
                Left = 0;
            }

            #endregion Constructors

            #region Properties

            public List<int> Change { get; set; }
            public int Left { get; set; }

            #endregion Properties
        }

        #endregion Classes
    }
}