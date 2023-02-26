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

            query = query
                .Skip(r.Skip ?? 0)
                .Take(r.Take ?? 10)
                .OrderBy(u => u.UserName).ThenBy(u => u.Id);

            var userList = await query.SelectMany(
                u => _dbContext.UserRoles.Where(ur => u.Id == ur.UserId).DefaultIfEmpty(),
                (u, rmp) => new { User = u, RoleMapEntry = rmp })
                .SelectMany(
                x => _dbContext.Roles.Where(r => r.Id == x.RoleMapEntry.RoleId).DefaultIfEmpty(),
                (x, role) => new { User = x.User, Role = role })
                .ToListAsync();

            var usersWithRoles = userList.Aggregate(new Dictionary<string, List<VendingMachine.Domain.Identity.Role>>(),
                (dict, data) =>
                {
                    dict.TryAdd(data.User.Id, new List<VendingMachine.Domain.Identity.Role>());

                    if (data.Role != null)
                    {
                        dict[data.User.Id].Add(data.Role!);
                    }

                    return dict;
                }, x => x);

            Response.TotalItems = totalCount;
            Response.Users = usersWithRoles.Select(x =>
            {
                var data = userList.First(u => u.User.Id == x.Key);
                return new UserViewModel
                {
                    Id = data.User.Id,
                    Email = data.User.Email,
                    Username = data.User.UserName,
                    Roles = x.Value.Select(r => r.Name).ToList()
                };
            });

            await SendAsync(Response);
        }

        #endregion Methods
    }
}