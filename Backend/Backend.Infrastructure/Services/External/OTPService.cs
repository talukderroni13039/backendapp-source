using Backend.Infrastructure.Interface.Email;
using Backend.Infrastructure.Interface.OTP;
using Backend.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services.External
{
    public class OTPService : IOTPService
    {
        ISmsService _smsService;
        private readonly  IEmailService _emailSvc;


        public OTPService(IEmailService EmailService, ISmsService smsService)
        {
            //_smsSvc = SMSSvc.Instance;
            _emailSvc = EmailService;
            _smsService = smsService;    
        }

        public async Task<long> SendOTP(string mobileNo, string email=null)
        {
            try
            {

                bool isSendToMobile = false, isSendToEmail = false;

                string refNo = "";

                // get otp configuration

                int otpLength = 6;
                string otp = GenerateRandomOTP(otpLength);
                int otpSessionTimeOutDuration = 120;

                StringBuilder otpMessage = new StringBuilder(@"Please Use this Otp:  "); 
                
                otpMessage.Append(otp);


                //send otp to email
                if (!string.IsNullOrEmpty(email) && email != "null")
                {
                    EmailInfo emailInfo = new EmailInfo();
                    emailInfo.To = email;
                    emailInfo.Body = otpMessage.ToString();

                    isSendToEmail =await _emailSvc.SendEmail(emailInfo);

                    refNo = email;
                }

                //send otp to mobile
                if (!string.IsNullOrEmpty(mobileNo) && mobileNo != "null")
                {
                    isSendToMobile = _smsService.SendSMS(mobileNo, otpMessage.ToString());
                    refNo = mobileNo;
                }

                if (isSendToMobile || isSendToEmail)
                {
                   // return await SaveOTP(otp, otpSessionTimeOutDuration, refNo);
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //private async Task<long> SaveOTP(string otp, int otpSessionTimeOutDuration, string refNo)
        //{
        //    try
        //    {
        //        AD_UserOTP _userOTP = new AD_UserOTP();
        //        _userOTP.RefNo = refNo;
        //        _userOTP.OTP = otp;
        //        _userOTP.IsOTPMatched = false;
        //        _userOTP.SendDate = DateTime.Now;
        //        _userOTP.SendTime = DateTime.Now.ToString("HH:mm:ss");
        //        _userOTP.ExpiryTimeInSecond = otpSessionTimeOutDuration;

        //        DataContext.Add(_userOTP);
        //        await DataContext.SaveChangesAsync();

        //        return _userOTP.id;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        private string GenerateRandomOTP(int otpLength)
        {
            string[] allowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };

            string sOTP = String.Empty;

            string sTempChars = String.Empty;


            Random rand = new Random();

            for (int i = 0; i < otpLength; i++)
            {
                sTempChars = allowedCharacters[rand.Next(0, allowedCharacters.Length)];
                sOTP += sTempChars;
            }
            return sOTP;
        }
       
        //public async Task<bool> ConfirmOTP(long userOTPID, string otp)
        //{
        //    try
        //    {
        //        bool isConfirmed = false;

        //        // get otp configuration
        //        var otpConfigList = GetOTPConfigList();
        //        int otpSubmissionCount = Convert.ToInt32(otpConfigList.FirstOrDefault(a => a.Value == FixedIDs.PageConfigType.OSC.ToString()).PageConfigValue);

        //        var userOTP = DataContext.AD_UserOTP.FirstOrDefault(a => a.id == userOTPID);

        //        if (otpSubmissionCount == userOTP.OTPCount) return isConfirmed;

        //        TimeSpan sendTime = TimeSpan.Parse(userOTP.SendTime);
        //        DateTime sendDateTime = userOTP.SendDate + sendTime;
        //        DateTime endDateTime = sendDateTime.AddSeconds(userOTP.ExpiryTimeInSecond);
        //        DateTime currentDateTime = DateTime.Now;

        //        userOTP.OTPCount++;

        //        if (sendDateTime <= currentDateTime && currentDateTime <= endDateTime && otp == userOTP.OTP && userOTP.OTPCount <= otpSubmissionCount)
        //        {
        //            userOTP.IsOTPMatched = true;
        //            isConfirmed = true;
        //        }
        //        DataContext.Entry(userOTP).State = EntityState.Modified;
        //        await DataContext.SaveChangesAsync();

        //        return isConfirmed;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}






    }
}
