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
            string accountSid = "ACd6c54eb6025a8875c8ffdf4d737fd6d0";
            string authToken = "a5b559da49cfd95fabb1e14a67bfc7b6";
            string fromPhoneNumber = "+12157038737";


            TwilioClient.Init(accountSid, authToken);

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
