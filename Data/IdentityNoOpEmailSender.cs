using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using Vaperia_drink.Data;

public class IdentityNoOpEmailSender : IEmailSender<ApplicationUser>
{
    public Task SendEmailAsync(ApplicationUser user, string subject, string htmlMessage)
    {
        // No hace nada
        return Task.CompletedTask;
    }

    public Task SendPasswordResetCodeAsync(ApplicationUser user, string code, string email)
    {
        // No hace nada
        return Task.CompletedTask;
    }

    public Task SendPasswordResetLinkAsync(ApplicationUser user, string link, string email)
    {
        // No hace nada
        return Task.CompletedTask;
    }

    public Task SendConfirmationLinkAsync(ApplicationUser user, string link, string email)
    {
        // No hace nada
        return Task.CompletedTask;
    }
}
