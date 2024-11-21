using Backend.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Interface.Email
{
    public interface IEmailService
    {
     Task<bool> SendEmail(EmailInfo emailInfo);
   
    }
}
