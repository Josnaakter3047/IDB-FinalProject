using DataAccess.IRepository.Account;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IUnitRepository : IDisposable
    {
        IAccountRepository Users { get; }
        IRefreshTokenRepository RefreshToken { get; }
        IUserTokensRepository UserToken { get; }
       

    }
}
