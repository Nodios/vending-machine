using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using VendingMachine.DataAccess;
using VendingMachine.Domain.Configuration;

namespace User.Find
{
    public class Endpoint : Endpoint<Request, Response>
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
            Get("users");
            Roles(RoleNames.ADMIN);
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            var query = _dbContext.Users.AsNoTracking().AsQueryable();

            var totalCount = await query.CountAsync();

            query = query.Skip(r.Skip ?? 0).Take(r.Take ?? 10);

            var userList = await query.ToListAsync();

            Response.TotalItems = totalCount;
            Response.Users = userList.Select(u => new UserViewModel
            {
                Id = u.Id,
                Email = u.Email,
                Username = u.UserName
            });

            await SendAsync(Response);
        }

        #endregion Methods
    }
}