using DataAccess.IRepository.Account;
using DataModels.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Account
{
    public class UserTokensRepository : Repository<UserTokens>, IUserTokensRepository
    {
        private readonly ApplicationDbContext _db;
        public UserTokensRepository(ApplicationDbContext db) : base(db) => _db = db;

        public UserTokens GetById(Guid id)
        {
            return _db.UserToken.Where(x => x.Id == id).FirstOrDefault();
        }

        public UserTokens GetByUserId(string id)
        {
            return _db.UserToken.Where(x => x.ApplicationUserId == id).FirstOrDefault();
        }

        public int Update(string userId, string token)
        {
            var obj = _db.UserToken.Where(x => x.ApplicationUserId == userId).FirstOrDefault();
            if (obj == null) return 0;
            obj.Token = token;
            return _db.SaveChanges();
        }
    }
}
