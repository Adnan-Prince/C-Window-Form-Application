using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHMS.DAL.Repositories.Interfaces
{
    public interface IReciever:IBaseRepository<WhmsTransaction>
    {
        public void getAdnan();
    }
}
