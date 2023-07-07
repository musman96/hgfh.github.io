using HGFH.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGFH.Services
{
    public interface IEmailSenderService
    {
        Task ExecuteEmails(string subject, string message, string email, string emailName);
        Task ExecuteEmailsList(List<Subscriber> subscribers,string subject, string message, string emailName);
    }
}
