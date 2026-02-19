using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace Health_Insurance.Services
{
    // Dummy email sender - disables actual email sending for now
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // No email sending logic needed yet
            // Just return completed task to satisfy Identity dependency
            return Task.CompletedTask;
        }
    }
}
