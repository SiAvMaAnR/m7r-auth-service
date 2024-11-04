using Auth.Domain.Common;
using Auth.Domain.Services;
using Auth.Domain.Shared.Models;
using Auth.Infrastructure.RabbitMQ;
using Auth.Infrastructure.Services.Common;

namespace Auth.Infrastructure.Services.AccountsService;

public class AccountsIS : BaseIService, IAccountsIS
{
    public AccountsIS(IAppSettings appSettings, IRabbitMQProducer rabbitMQProducer)
        : base(appSettings, rabbitMQProducer) { }

    public async Task<Password?> UpdatePasswordAsync(int accountId, string password)
    {
        RMQResponse<Password>? response = await _rabbitMQProducer.Send<RMQResponse<Password>>(
            RMQ.Queue.Accounts,
            RMQ.AccountsQueuePattern.UpdatePassword,
            new { accountId, password = AuthBS.CreatePasswordHash(password) }
        );

        return response?.Data;
    }

    public async Task<Account?> GetByIdAsync(int accountId)
    {
        RMQResponse<Account?>? response = await _rabbitMQProducer.Send<RMQResponse<Account?>>(
            RMQ.Queue.Accounts,
            RMQ.AccountsQueuePattern.GetById,
            new { accountId }
        );

        return response?.Data;
    }

    public async Task<Account?> GetByEmailAsync(string email)
    {
        RMQResponse<Account?>? response = await _rabbitMQProducer.Send<RMQResponse<Account?>>(
            RMQ.Queue.Accounts,
            RMQ.AccountsQueuePattern.GetByEmail,
            new { email }
        );

        return response?.Data;
    }
}
