using Auth.Domain.Common;
using Auth.Domain.Services;
using Auth.Domain.Shared.Models;
using Auth.Infrastructure.RabbitMQ;
using Auth.Infrastructure.Services.Common;

namespace Auth.Infrastructure.Services.NotificationsService;

public class AccountsIS : BaseIService, IAccountsIS
{
    public AccountsIS(IAppSettings appSettings, IRabbitMQProducer rabbitMQProducer)
        : base(appSettings, rabbitMQProducer) { }

    public async Task<Password?> UpdatePasswordAsync(int accountId, string password)
    {
        Password createdPassword = AuthBS.CreatePasswordHash(password);

        Password? newPassword = await _rabbitMQProducer.Emit<Password>(
            RMQ.Queue.Accounts,
            RMQ.AccountsQueuePattern.UpdatePassword,
            new { accountId, createdPassword }
        );

        return newPassword;
    }

    public async Task<Account?> GetByIdAsync(int accountId)
    {
        Account? account = await _rabbitMQProducer.Emit<Account>(
            RMQ.Queue.Accounts,
            RMQ.AccountsQueuePattern.GetById,
            new { accountId }
        );

        return account;
    }

    public async Task<Account?> GetByEmailAsync(string email)
    {
        Account? account = await _rabbitMQProducer.Emit<Account>(
            RMQ.Queue.Accounts,
            RMQ.AccountsQueuePattern.GetByEmail,
            new { email }
        );

        return account;
    }
}
