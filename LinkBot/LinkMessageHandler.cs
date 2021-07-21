using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using LinkBot.Data;
using LinkBot.Models;
using LinkBot.Services;
using Microsoft.Extensions.Logging;

namespace LinkBot
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
            
            var channel = message.Channel as SocketTextChannel;
            
            // TODO: Add these columns to the database.
            var messageInfo = new
            {
                ChannelId = message.Channel.Id,
                ChannelName = message.Channel.Name,
                ServerId = channel?.Guild.Id,
                ServerName = channel?.Guild.Name,
                AuthorId = message.Author.Id,
                AuthorName = $"{message.Author.Username}#{message.Author.Discriminator}"
            };
            
            var links = await _parser.GetLinks(message.Content);

            var linkData = links
                .Where(link => link is not null)
                .Select(link =>
                {
                    var preview = _analyzer.Analyze(link);
                    preview.ServerId = messageInfo.ServerId.ToString();
                    preview.ServerName = messageInfo.ServerName;
                    preview.ChannelId = messageInfo.ChannelId.ToString();
                    preview.ChannelName = messageInfo.ChannelName;
                    preview.AuthorId = messageInfo.AuthorId.ToString();
                    preview.AuthorName = messageInfo.AuthorName;
                    return preview;
                });
            
            var linkPreviews = linkData as LinkPreview[] ?? linkData.ToArray();
            if (linkPreviews.Any())
            {
                foreach (var link in linkPreviews)
                {
                    // Save each link to the database.
                    await _data.SaveLink(link);
                }
                
                await message.AddReactionAsync(new Emoji(_icons["link"]), RequestOptions.Default);
            }

            // await message.Channel.SendMessageAsync($"Saved {linkPreviews.Length} links.");
        }
    }
}