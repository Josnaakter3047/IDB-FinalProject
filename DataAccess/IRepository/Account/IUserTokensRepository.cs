using DataModels.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository.Account
{
    public interface IUserTokensRepository : IRepository<UserTokens>
    {
        public UserTokens GetById(Guid id);
        public UserTokens GetByUserId(string id);
        public int Update(string userId, string token);
    }
}
