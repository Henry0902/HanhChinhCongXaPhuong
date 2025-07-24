using System;
using System.Linq;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class UsersTempRp
    {
        private GenericRepository<UsersTemp> _context = null;
        private DbConnectContext _obj;
        public UsersTempRp()
        {
            _obj = new DbConnectContext();
        }

        public bool CheckOtp(string otp)
        {
            var now = DateTime.Now;
            var count = _obj.UsersTemps.Count(x => x.Otp == otp && now >= x.OtpStartTime && now <= x.OtpEndTime && x.OtpStatus == 0);
            return count > 0 ? true : false;
        }
    }
}