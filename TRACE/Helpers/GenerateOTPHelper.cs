using System.Net.Mail;
using System.Net;

namespace TRACE.Helpers
{
    public class GenerateOTPHelper
    {
        public string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public void SendOtpEmail(string email, string otp)
        {
            var smtpClient = new SmtpClient("smtp.office365.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("test5@erc.ph", "Crispbacon22429"),
                EnableSsl = true
            };

            string emailBody = $@"
                <html>
                <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0;'>
                    <div style='max-width: 600px; margin: 30px auto; background-color: #ffffff; padding: 20px; border: 1px solid #ddd; border-radius: 5px;'>
                        <h2 style='color: #007bff; margin-bottom: 20px;'>Your One-Time Passcode (OTP) for ICDMS</h2>
                        <p style='font-size: 16px; color: #333;'>Hello,</p>
                        <p style='font-size: 16px; color: #333;'>Please use the OTP below to complete your login:</p>
                        <p style='font-size: 24px; font-weight: bold; color: #28a745; text-align: center; letter-spacing: 2px;'>{otp}</p>
                        <p style='font-size: 14px; color: #666;'>This OTP will expire in 3 minutes. Please do not share it with anyone.</p>
                        <p style='font-size: 14px; font-weight: bold; color: #dc3545;'>IMPORTANT: Do not share this OTP with anyone else. MISD staff and Developers will NEVER ask for your OTP. If someone asks for your OTP, consider it a security threat.</p>
                        <p style='font-size: 14px; color: #666;'>Thank you.</p>
                        <hr style='margin: 20px 0; border: none; border-top: 1px solid #ddd;' />
                        <p style='font-size: 12px; color: #999; text-align: center;'>This is an automated message. Please do not reply.</p>
                    </div>
                </body>
                </html>";

            var mailMessage = new MailMessage
            {
                From = new MailAddress("test5@erc.ph", "ERC Support"),
                Subject = "ICMDS OTP",
                Body = emailBody,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            smtpClient.Send(mailMessage);
        }

    }
}
