namespace Product.Get
{
    public class Request
    {
        #region Properties

        public Guid Id { get; set; }

        #endregion Properties
    }

    public class Response
    {
        #region Properties

        public int AmountAvailable { get; set; }
        public decimal Cost { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SellerId { get; set; }
        public string SellerName { get; set; }

        #endregion Properties
    }
}