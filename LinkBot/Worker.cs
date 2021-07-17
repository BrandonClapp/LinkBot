using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IOptions<BotConfig> _config;

        public Worker(ILogger<Worker> logger, IOptions<BotConfig> config)
        {
            _logger = logger;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                
                var client = new DiscordSocketClient();

                client.Log += Log;

                var token = _config.Value.Token;

                await client.LoginAsync(TokenType.Bot, token);
                await client.StartAsync();
                
                client.MessageReceived += ClientOnMessageReceived;
                
                // Block this task until the program is closed.
                await Task.Delay(-1);
            }
        }

        private Task ClientOnMessageReceived(SocketMessage message)
        {
            _logger.LogInformation(message.Content);
            return Task.CompletedTask;
        }

        private Task Log(LogMessage arg)
        {
            _logger.LogInformation(arg.Message);
            return Task.CompletedTask;
        }
    }
}