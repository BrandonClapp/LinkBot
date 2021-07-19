using System;
using System.Collections.Generic;

namespace LinkBot.Models
{
    public class LinkPreview
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Uri { get; set; }
        public string ImageUri { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<string> Keywords { get; set; }
    }
}