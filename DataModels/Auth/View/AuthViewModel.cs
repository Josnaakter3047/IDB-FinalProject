using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Auth.View
{
    public class AuthViewModel
    {
        public string FullName { get; set; }
        public List<string> Roles { get; set; }
        public string CustomerId { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpireTime { get; set; }
        public DateTime RefreshTokenExpireTime { get; set; }
        public string RegFP { get; set; }
    }
}
