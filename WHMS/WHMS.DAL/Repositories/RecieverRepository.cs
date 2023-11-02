
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using WHMS.DAL.Data;
using WHMS.DAL.Repositories;
using WHMS.DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class RecieverRepository : GenericRepository<WhmsTransaction>,IReciever
    {

        public RecieverRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {

        }
        public void getAdnan()
        {
            throw new NotImplementedException();
        }
    }
}
