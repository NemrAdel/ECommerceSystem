using ECommerce.Shared.DTOs;

namespace ECommerce.Services.Abstraction
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailDTO emailDTO);
    }
}
