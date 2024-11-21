using Backend.Infrastructure.Interface.OTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Backend.Infrastructure.Services.External
{
    public class SmsService : ISmsService
    {
        public bool SendSMS( string mobileNumber, string messsage)
        {
            string fromPhoneNumber = "+12157038737";
            try
            {
                var message = MessageResource.Create(
                    body: messsage,
                    from: new Twilio.Types.PhoneNumber(fromPhoneNumber),
                    to: new Twilio.Types.PhoneNumber(mobileNumber)
                );

                Console.WriteLine($"SMS sent successfully. SID: {message.Sid}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send SMS: {ex.Message}");
            }

            return true;
        }


    }
}
