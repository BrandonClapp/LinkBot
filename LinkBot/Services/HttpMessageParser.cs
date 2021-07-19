using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LinkBot.Services
{
    public class HttpMessageParser
    {
        private readonly Regex _matcher = new Regex(@"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+(:[0-9]+)?|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)");

        public Task<IEnumerable<string>> GetLinks(string message)
        {
            var matches = _matcher.Matches(message);
            var links = matches?.Select(m => m.Value);
            return Task.FromResult(links);
        }
    }
}