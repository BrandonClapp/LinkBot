using System;
using System.Linq;
using System.Threading.Tasks;
using LinkBot.Services;
using Xunit;

namespace LinkBot.Tests
{
    public class HttpMessageParserTests
    {
        private readonly HttpMessageParser _parser = new();

        [Fact]
        public async Task CanIdentifyHttpMessage()
        {
            var message = @"http://google.com/";
            var links = await _parser.GetLinks(message);
            
            Assert.True(links.Count() == 1);
            Assert.Equal(message, links.First());
        }

        [Fact]
        public async Task CanIdentifyHttpsMessage()
        {
            var message = @"https://google.com";
            var links = await _parser.GetLinks(message);
            
            Assert.True(links.Count() == 1);
            Assert.Equal(message, links.First());
        }

        [Fact]
        public async Task CanIdentifyHttpsMessageWithPath()
        {
            var message = @"https://google.com/and-this-path/";
            var links = await _parser.GetLinks(message);
            
            Assert.True(links.Count() == 1);
            Assert.Equal(message, links.First());
        }
        
        [Fact]
        public async Task CanIdentifyHttpsMessageWithPathAndQueryString()
        {
            var message = @"https://docs.microsoft.com/en-us/search/?terms=c%23%20idispose%20example";
            var links = await _parser.GetLinks(message);
            
            Assert.True(links.Count() == 1);
            Assert.Equal(message, links.First());
        }
        
        [Fact]
        public async Task CanIdentifyHttpsMessageWithMultiple()
        {
            var message = @"https://docs.microsoft.com/en-us/search/?terms=c%23%20idispose%20example and https://google.com/and-this-path/";
            var links = await _parser.GetLinks(message);
            
            Assert.True(links.Count() == 2);
        }
    }
}