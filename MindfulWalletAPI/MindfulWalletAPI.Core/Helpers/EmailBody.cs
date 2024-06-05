using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindfulWallet.Core.Helpers
{
    public static class EmailBody
    {
        public static string EmailStringBody(string email, string emailToken)
        {
            return $@"
    <html>
    <head></head>
    <body style=""margin:0;padding:0;font-family: Arial, Helvetica, sans-serif;background-color:#f4f4f7;"">
        <div style=""max-width:600px;margin:40px auto;padding:20px;background:white;border-radius:8px;box-shadow:0 4px 6px rgba(0,0,0,0.1);"">
            <div style=""text-align:center;"">
                <img src=""https://i.imgur.com/TbQzZJ9.png"" alt=""Mindful Wallet Logo"" style=""max-width:100px;margin-bottom:20px;"">
                <h1 style=""font-size:24px;color:#333333;margin-bottom:10px;"">Password reset</h1>
                <p style=""font-size:16px;color:#666666;margin-bottom:30px;"">Someone requested that the password be reset for the following Mindful Wallet account:</p>
                <p style=""font-size:16px;color:#666666;margin-bottom:30px;"">To reset your password, visit the following address:</p>
                <a href=""http://localhost:4200/reset?email={email}&code={emailToken}"" target=""_blank"" style=""background:#0d6efd;padding:15px 25px;border:none;color:white;border-radius:4px;text-align:center;text-decoration:none;font-size:16px;display:inline-block;margin-bottom:30px;"">Click here to reset your password</a>
                <p style=""font-size:16px;color:#666666;margin-bottom:30px;"">Your email: <a href=""mailto:{email}"" style=""color:#0d6efd;text-decoration:none;"">{email}</a></p>
                <p style=""font-size:16px;color:#666666;"">If this was a mistake, just ignore this email and nothing will happen.</p>
            </div>
        </div>
        <div style=""text-align:center;margin-top:20px;font-size:12px;color:#999999;"">
            <p>All Rights Reserved.</p>
        </div>
    </body>
    </html>";
        }
    }
}
