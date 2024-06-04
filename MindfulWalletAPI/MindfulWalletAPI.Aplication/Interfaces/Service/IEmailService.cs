using MindfulWallet.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MindfulWallet.Aplication.Interfaces.Service
{
    public interface IEmailService
    {
        void SendEmail(EmailModel emailModel);
    }
}
