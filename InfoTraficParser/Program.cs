using System;
using System.Collections.Generic;

namespace InfotraficParser
{
    public class Program
    {
        private static void Main()
        {
            Bootstrap.Start();

            ReadItems();

            Console.ReadKey(); // wait
        }

        private static void ReadItems()
        {
            var crawler = Bootstrap.Container.GetInstance<PageCrawler>();

            var keepReading = true;
            while (keepReading)
            {
                int count;
                var items = crawler.ReadContentItems(out count);

                // using an out variable instead of doing a .Any(), which will cause the Linq Query Execution to soon
                keepReading = count > 0;

                LogItemsToConsole(items);
            }
        }

        private static void LogItemsToConsole(IEnumerable<ContentItem> contentItems)
        {
            foreach (var contentItem in contentItems)
            {
                var link = contentItem.Link;
                Log($"http://[...]{link.Substring(contentItem.Link.LastIndexOf('/'), link.Length - link.LastIndexOf('/'))}:", ConsoleColor.Yellow);

                foreach (var contentLine in contentItem.Content)
                {
                    Log($" {contentLine.Substring(0, Math.Min(contentLine.Length, 100))}[...]"); // truncate at 100 characters for now
                }

                Log();
            }
        }

        private static void Log(string message = "", ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray; // reset
        }
    }
}