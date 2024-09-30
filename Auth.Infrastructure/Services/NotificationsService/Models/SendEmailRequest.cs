namespace Auth.Infrastructure.Services.NotificationsService.Models;

public class NotificationsIServiceSendEmailRequest
{
    public required string Template { get; set; }
    public required string Recipient { get; set; }
    public required object Data { get; set; }
}
