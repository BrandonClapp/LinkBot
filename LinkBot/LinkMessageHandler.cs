using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using WorkerService.Services;

namespace WorkerService
{
    public class LinkMessageHandler
    {
        private readonly ILogger<LinkMessageHandler> _logger;
        private readonly HttpMessageParser _parser;

        private List<string> _links = new();

        public LinkMessageHandler(ILogger<LinkMessageHandler> logger, HttpMessageParser parser)
        {
            _logger = logger;
            _parser = parser;
        }
        
        public async Task Handle(SocketMessage message)
        {
            _logger.LogInformation("Handling message: {Message}", message.Content);
            var links = await _parser.GetLinks(message.Content);
            
            // Log links to database.
            _links.AddRange(links);
            
            // TODO: Analyze all of the links.
            // Title, URL, Tags
            
            await Task.Run(() => { });
        }
    }
}