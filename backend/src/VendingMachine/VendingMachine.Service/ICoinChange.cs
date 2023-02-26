namespace VendingMachine.Service
{
    public interface ICoinChange
    {
        #region Methods

        public IEnumerable<int> GetChange(IEnumerable<int> denominations, int amount);

        #endregion Methods
    }
}