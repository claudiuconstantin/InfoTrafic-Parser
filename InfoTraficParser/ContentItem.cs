using System.Collections.Generic;

namespace InfotraficParser
{
    public class ContentItem
    {
        public ContentItem(string link)
        {
            Link = link;
        }

        public string Link { get; set; }

        // TODO: further break this down to details
        public IEnumerable<string> Content { get; set; }
    }
}