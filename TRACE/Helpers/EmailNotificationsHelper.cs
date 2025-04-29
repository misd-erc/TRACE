using System.Net.Mail;
using System.Net;

namespace TRACE.Helpers
{
    public class EmailNotificationsHelper
    {
        private readonly string smtpServer = "smtp.office365.com";
        private readonly int smtpPort = 587;
        private readonly string smtpEmail = "test5@erc.ph";
        private readonly string smtpPassword = "Crispbacon22429";

        public void SendCaseAssignmentEmail(string assignedUserEmail, string caseNo, string assignedByUsername)
        {
            string assignedToUsername = assignedUserEmail.Split('@')[0];
            using (var smtpClient = new SmtpClient(smtpServer)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(smtpEmail, smtpPassword),
                EnableSsl = true
            })
            {
                string emailBody = $@"
                    <html>
                        <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0;'>
                            <div style='max-width: 600px; margin: 30px auto; background-color: #ffffff; padding: 20px; border: 1px solid #ddd; border-radius: 5px;'>
                                <h2 style='color: #007bff; margin-bottom: 20px;'>Case Assignment Notification</h2>
                                <p style='font-size: 16px; color: #333;'>Hello {assignedToUsername},</p>
                                <p style='font-size: 16px; color: #333;'>You have been assigned to a new case by <strong style='color: #007bff;'>{assignedByUsername}</strong>.</p>
                                <p style='font-size: 18px; font-weight: bold; color: #28a745; text-align: center;'>Case No: {caseNo}</p>
                                <p style='font-size: 14px; color: #666;'>Please check the ICDMS system for more details.</p>
                                <p style='font-size: 14px; color: #666;'>Thank you.</p>
                                <hr style='margin: 20px 0; border: none; border-top: 1px solid #ddd;' />
                                <p style='font-size: 12px; color: #999; text-align: center;'>This is an automated message. Please do not reply.</p>
                            </div>
                        </body>
                        </html>";

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpEmail, "ERC Support"),
                    Subject = "Case Assignment Notification",
                    Body = emailBody,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(assignedUserEmail);

                smtpClient.Send(mailMessage);
            }
        }
    }
}
