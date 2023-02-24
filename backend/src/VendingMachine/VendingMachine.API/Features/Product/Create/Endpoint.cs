using FastEndpoints;
using VendingMachine.DataAccess;
using VendingMachine.Domain.Configuration;

namespace Product.Create
{
    public class Endpoint : Endpoint<Request, Response, Mapper>
    {
        #region Fields

        private readonly VendingMachineDbContext _dbContext;

        #endregion Fields

        #region Constructors

        public Endpoint(VendingMachineDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion Constructors

        #region Methods

        public override void Configure()
        {
            Post("products");
            Roles(RoleNames.SELLER);
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            var newProduct = Map.ToEntity(r);
            var createProduct = VendingMachine.Domain.Product.Create(newProduct);

            _dbContext.Products.Add(createProduct);
            await _dbContext.SaveChangesAsync();

            Response = Map.FromEntity(createProduct);

            await SendAsync(Response);
        }

        #endregion Methods
    }
}