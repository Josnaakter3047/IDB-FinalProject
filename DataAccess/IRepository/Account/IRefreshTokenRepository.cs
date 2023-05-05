using DataModels.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository.Account
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        public RefreshToken GetRefreshTokenByToken(string token);
        public List<RefreshToken> GetRefreshTokenByUserId(string Id);
    }
}
