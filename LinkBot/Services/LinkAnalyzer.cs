using System;
using System.Collections.Generic;
using System.Net;
using HtmlAgilityPack;
using LinkBot.Models;

namespace LinkBot.Services
{
    public class LinkAnalyzer
    {
        public LinkPreview Analyze(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }
            
            var web = new HtmlWeb();

            try
            {
                var htmlDoc = web.Load(url);

                var title = htmlDoc.DocumentNode.SelectSingleNode("//head/title")?.InnerText;
                var desc = htmlDoc.DocumentNode.SelectSingleNode("//meta[@name='description']")?.Attributes["content"]
                    ?.Value;
                var image = htmlDoc.DocumentNode.SelectSingleNode("//meta[@property='og:image']")?.Attributes["content"]
                    ?.Value;
                var keywords = htmlDoc.DocumentNode.SelectSingleNode("//meta[@name='keywords']")?.Attributes["content"]
                    ?.Value;

                if (string.IsNullOrWhiteSpace(desc))
                {
                    desc = htmlDoc.DocumentNode.SelectSingleNode("//meta[@property='og:description']")
                        ?.Attributes["content"]?.Value;
                }
                
                title = HtmlEntity.DeEntitize(title);
                desc = HtmlEntity.DeEntitize(desc);
                image = HtmlEntity.DeEntitize(image);
                
                return new LinkPreview()
                {
                    Title = title,
                    Description = desc,
                    Uri = url,
                    ImageUri = image,
                    CreatedAt = DateTime.UtcNow,
                    Keywords = new List<string>(keywords?.Split(",") ?? Array.Empty<string>())
                };
            }
            catch (WebException ex)
            {
                return null;
            }
        }
    }
}