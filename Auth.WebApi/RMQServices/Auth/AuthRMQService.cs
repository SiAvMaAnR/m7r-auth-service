using System.Text;
using System.Text.Json;
using Auth.Application.Services.AuthService;
using Auth.Application.Services.AuthService.Models;
using Auth.Domain.Exceptions;
using Auth.Infrastructure.RabbitMQ;
using RabbitMQ.Client;

namespace Auth.WebApi.RMQServices;

public partial class AuthRMQService : RMQService
{
    private readonly string _queueName = RMQ.Queue.Auth;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AuthRMQService(
        IRabbitMQConsumer consumer,
        IRabbitMQProducer producer,
        IServiceScopeFactory serviceScopeFactory
    )
        : base(consumer, producer)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task LoginAsync(
        IBasicProperties basicProperties,
        LoginData data,
        IAuthService authService
    )
    {
        AuthServiceLoginResponse response = await authService.LoginAsync(
            new AuthServiceLoginRequest() { Email = data.Email, Password = data.Password }
        );

        _producer.SendReply(
            basicProperties.ReplyTo,
            basicProperties.CorrelationId,
            RMQ.AuthQueuePattern.Login,
            response
        );
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.AddListener(
            _queueName,
            async (_, args) =>
            {
                byte[] body = args.Body.ToArray();
                string bodyJson = Encoding.UTF8.GetString(body);
                JsonSerializerOptions serializerOptions = RabbitMQBase.JsonSerializerOptions;

                RMQResponse<JsonElement> result =
                    JsonSerializer.Deserialize<RMQResponse<JsonElement>>(
                        bodyJson,
                        serializerOptions
                    ) ?? throw new IncorrectDataException("Failed to deserialize json");

                string replyQueue = args.BasicProperties.ReplyTo;
                string correlationId = args.BasicProperties.CorrelationId;

                using IServiceScope scope = _serviceScopeFactory.CreateScope();

                IAuthService authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

                Task task = result.Pattern switch
                {
                    RMQ.AuthQueuePattern.Login
                        => LoginAsync(
                            args.BasicProperties,
                            JsonSerializer.Deserialize<LoginData>(result.Data, serializerOptions)!,
                            authService
                        ),
                    _ => throw new OperationNotAllowedException("Message pattern not found")
                };

                await task;
            }
        );

        return Task.CompletedTask;
    }
}
