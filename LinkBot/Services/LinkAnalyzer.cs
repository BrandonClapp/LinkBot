using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;
using WorkerService.Models;

namespace WorkerService.Services
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

                return new LinkPreview()
                {
                    Title = title,
                    Description = desc,
                    ImageUri = image,
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