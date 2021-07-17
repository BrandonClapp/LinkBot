using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace WorkerService
{
    public class TextMessageHandler
    {
        private readonly ILogger<TextMessageHandler> _logger;
        
        public TextMessageHandler(ILogger<TextMessageHandler> logger)
        {
            _logger = logger;
        }
        
        public Task Handle(SocketMessage message)
        {
            _logger.LogInformation("Message received.");
            return Task.CompletedTask;
        }
    }
}