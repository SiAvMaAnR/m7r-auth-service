using Auth.Infrastructure.Services.NotificationsService.Models;

namespace Auth.Infrastructure.Services.NotificationsService;

public interface INotificationsIS
{
    Task<NotificationsIServiceSendEmailResponse> SendEmailAsync(
        NotificationsIServiceSendEmailRequest request
    );
}
