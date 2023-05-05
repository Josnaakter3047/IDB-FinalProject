using DataAccess.IRepository.Account;
using DataModels.Auth;
using DataModels.Other.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Account
{
    public class AccountRepository : Repository<ApplicationUser>, IAccountRepository
    {
        private readonly ApplicationDbContext _db;
        public AccountRepository(ApplicationDbContext db) : base(db) => _db = db;

        public bool IfUserEmailAlreadyExists(Guid id, string eamil) => (id == Guid.Empty) ?
            _db.Users.Where(x => x.Email.Trim() == eamil.Trim())
            .FirstOrDefault() != null :
            _db.Users.Where(x => x.Email.Trim() == eamil.Trim() && x.Id != id.ToString())
            .FirstOrDefault() != null;

        public ApplicationUser Get(string id) => _db.Users
            .Where(x => x.Id == id)
            .FirstOrDefault();

        public bool IfPhoneNumberAlreadyExists(Guid id, string number) => (id == Guid.Empty) ?
            _db.Users.Where(x => x.PhoneNumber.Trim() == number.Trim())
            .FirstOrDefault() != null :
            _db.Users.Where(x => x.PhoneNumber.Trim() == number.Trim() && x.Id != id.ToString())
            .FirstOrDefault() != null;

        public IEnumerable<ApplicationUser> UserList(FilterModel model)
        {
            return model.SortField != null && model.SortOrder == -1 ?
                _db.Users
                .Where(x => x.ParentId == null)
                .Where(x =>
                x.FullName.Contains(model.GlobalFilter ?? "") ||
                x.PhoneNumber.Contains(model.GlobalFilter ?? "") ||
                x.CustomerId.Contains(model.GlobalFilter ?? "") ||
                x.Email.Contains(model.GlobalFilter ?? ""))
                .OrderByDescending(x =>
                model.SortField.ToLower() == nameof(x.FullName) ? x.FullName :
                model.SortField.ToLower() == nameof(x.PhoneNumber) ? x.PhoneNumber :
                model.SortField.ToLower() == nameof(x.Email) ? x.Email : x.FullName)
                .Skip(model.First)
                .Take(model.Rows)
                :
                _db.Users
                .Where(x => x.ParentId == null)
                .Where(x =>
                x.FullName.Contains(model.GlobalFilter ?? "") ||
                x.PhoneNumber.Contains(model.GlobalFilter ?? "") ||
                x.CustomerId.Contains(model.GlobalFilter ?? "") ||
                x.Email.Contains(model.GlobalFilter ?? ""))
                .OrderByDescending(x => x.TimeStamp)
                .Skip(model.First)
                .Take(model.Rows);
        }

        public int TotalRecord(FilterModel model)
        {
            return _db.Users
                .Where(x => x.ParentId == null)
                .Where(x =>
                x.FullName.Contains(model.GlobalFilter ?? "") ||
                x.PhoneNumber.Contains(model.GlobalFilter ?? "") ||
                x.Email.Contains(model.GlobalFilter ?? "")).Count();
        }

        public int TotalRecord()
        {
            return _db.Users.Count();
        }

        public ApplicationUser GetByCustomerId(string customerId) => _db.Users
            .Where(x => x.CustomerId == customerId).FirstOrDefault();

        public IEnumerable<ApplicationUser> AllUsers()
        {
            return _db.Users.ToList();
        }

        public float GetRegFee(string customerId)
        {
            throw new NotImplementedException();
        }
    }
}
