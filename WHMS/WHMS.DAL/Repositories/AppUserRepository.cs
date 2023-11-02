

using Domain.Models;
using WHMS.DAL.Data;

namespace WHMS.DAL.Repositories
{
    public class AppUserRepository : GenericRepository<AppUser>
    {
        public AppUserRepository(ApplicationDbContext applicationDb) : base(applicationDb)
        {
        }

      
    }

}
