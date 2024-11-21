using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Interface.OTP
{
    public interface IOTPService
    {
        Task<long> SendOTP(string mobileNo, string email);
       // Task<bool> ConfirmOTP(long userOTPID, string otp);
    }
}
