using System.Threading.Tasks;
using LinkBot.Services;
using Xunit;

namespace LinkBot.Tests
{
    public class LinkAnalyzerTests
    {
        private readonly LinkAnalyzer _analyzer = new();

        public LinkAnalyzerTests()
        {
        }
        
        [Fact]
        public Task AnalyzeLink()
        {
            var linkData = _analyzer.Analyze("https://www.youtube.com/watch?v=pF1Qh2Ty7MQ&t=443s&ab_channel=NickChapsas");
            Assert.Contains("YouTube", linkData.Title);
            return Task.CompletedTask;
        }
        
        [Fact]
        public Task AnalyzeBadLink()
        {
            var linkData = _analyzer.Analyze("https://iamnotareallink23453234323.com");
            Assert.Null(linkData);
            return Task.CompletedTask;
            
        }
    }
}