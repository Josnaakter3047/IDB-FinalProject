using DataAccess.IRepository;
using DataAccess.IRepository.Account;

using DataAccess.Repository.Account;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class UnitRepository : IUnitRepository
    {
        private readonly ApplicationDbContext _db;
        public IAccountRepository Users { get; private set; }
        public IRefreshTokenRepository RefreshToken { get; private set; }

        public IUserTokensRepository UserToken { get; private set; }

        


        public UnitRepository(ApplicationDbContext db)
        {
            _db = db;
            Users = new AccountRepository(_db);
            RefreshToken = new RefreshTokenRepository(_db);

            UserToken = new UserTokensRepository(_db);

            

            

        }

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
