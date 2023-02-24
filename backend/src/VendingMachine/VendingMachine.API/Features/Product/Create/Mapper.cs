using FastEndpoints;

namespace Product.Create
{
    public class Mapper : Mapper<Request, Response, VendingMachine.Domain.Product>
    {
        #region Methods

        public override Response FromEntity(VendingMachine.Domain.Product e)
        {
            return new Response
            {
                Id = e.Id,
                DateCreated = e.DateCreated,
                DateUpdated = e.DateUpdated,
                AmountAvailable = e.AmountAvailable,
                Cost = Convert.ToInt32(e.Cost),
                Name = e.Name
            };
        }

        public override VendingMachine.Domain.Product ToEntity(Request r)
        {
            return new VendingMachine.Domain.Product
            {
                AmountAvailable = r.AmountAvailable,
                Cost = r.Cost,
                Name = r.Name,
                SellerId = r.SellerId
            };
        }

        #endregion Methods
    }
}