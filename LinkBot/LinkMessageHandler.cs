using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using WorkerService.Models;
using WorkerService.Services;

namespace WorkerService
{
    public class LinkMessageHandler
    {
        private readonly ILogger<LinkMessageHandler> _logger;
        private readonly HttpMessageParser _parser;
        private readonly LinkAnalyzer _analyzer;
        private readonly LinkData _data;

        // TODO: Extract this data.
        private Dictionary<string, string> _icons = new Dictionary<string, string>()
        {
            ["link"] = "\uD83D\uDD17",
            ["2"] = ":two:",
            ["3"] = "\u0033",
            ["4"] = "\u0034",
            ["5"] = "\u0035",
            ["6"] = "\u0036",
            ["7"] = "\u0037",
            ["8"] = "\u0038",
            ["9"] = "\u0039",
        };
        
        public LinkMessageHandler(ILogger<LinkMessageHandler> logger, HttpMessageParser parser, LinkAnalyzer analyzer, LinkData data)
        {
            _logger = logger;
            _parser = parser;
            _analyzer = analyzer;
            _data = data;
        }
        
        public async Task Handle(SocketMessage message)
        {
            _logger.LogInformation("Handling message: {Message}", message.Content);
            var links = await _parser.GetLinks(message.Content);

            var linkData = links
                .Where(link => link is not null)
                .Select(link => _analyzer.Analyze(link));
            
            // Save links in database.
            var existing = await _data.GetLinks(); 


            var linkPreviews = linkData as LinkPreview[] ?? linkData.ToArray();
            if (linkPreviews.Any())
            {
                await message.AddReactionAsync(new Emoji(_icons["link"]), RequestOptions.Default);
            }

            // await message.Channel.SendMessageAsync($"Saved {linkPreviews.Length} links.");
        }
    }
}