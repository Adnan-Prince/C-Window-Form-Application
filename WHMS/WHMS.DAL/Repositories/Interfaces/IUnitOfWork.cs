using DAL.Repositories;
using Domain.Models;
using System;
using WHMS.DAL.Data;

namespace WHMS.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    { 
         IBaseRepository<AppUser> AppUserRepository { get; }
         IBaseRepository<WhmsTransaction> RecieverRepository { get; }
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        private IBaseRepository<AppUser> _personRepository;
        public IBaseRepository<AppUser> AppUserRepository
        {
            get
            {
                if (_personRepository == null)
                {
                    _personRepository = new AppUserRepository(_context);
                }

                return _personRepository;
            }
        }

        private IBaseRepository<WhmsTransaction> baseRepository;
        public IBaseRepository<WhmsTransaction> RecieverRepository
        {
            get
            {
                if (baseRepository == null)
                {
                    baseRepository = new RecieverRepository(_context);
                }

                return baseRepository;
            }
        }
    }
}
