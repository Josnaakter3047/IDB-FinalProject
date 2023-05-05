using DataModels.Auth;
using DataModels.Other.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository.Account
{
    public interface IAccountRepository : IRepository<ApplicationUser>
    {
        public bool IfUserEmailAlreadyExists(Guid id, string eamil);
        public bool IfPhoneNumberAlreadyExists(Guid id, string number);
        public ApplicationUser Get(string id);
        public ApplicationUser GetByCustomerId(string customerId);
        public IEnumerable<ApplicationUser> UserList(FilterModel model);
        public int TotalRecord(FilterModel model);
        public int TotalRecord();
        
        public float GetRegFee(string customerId);

        IEnumerable<ApplicationUser> AllUsers();
    }
}
