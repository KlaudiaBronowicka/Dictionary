using System;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Dictionary
{
    class Program
    {
        private const string BablaUrl = "https://en.bab.la/dictionary/polish-english";


        static async Task Main(string[] args)
        {
            while (true)
            {
                var word = Console.ReadLine();

                HttpClient httpClient = new HttpClient();
                var result = await httpClient.GetAsync($"{BablaUrl}/{word}");

                var content = await result.Content.ReadAsStringAsync();
                var doc = new HtmlDocument();
                doc.LoadHtml(content);

                var error = doc.DocumentNode.SelectSingleNode("//div[contains (@class, 'content') and not(@id)] /div[contains(@class, 'quick-results') and contains(@class, 'container')] /div[@class = 'quick-results-header']");

                if (error.InnerText.Contains("is currently not in our dictionary"))
                {
                    Console.WriteLine("ERROR. Word not found");
                    continue;
                }

                var nodes = doc.DocumentNode.SelectNodes("//div[contains (@class, 'content') and not(@id)] /div[contains(@class, 'quick-results') and contains(@class, 'container')] /div[@class = 'quick-result-entry'] /div[@class = 'quick-result-overview'] /ul[@class = 'sense-group-results'] /li");

                foreach (var node in nodes)
                {
                    Console.WriteLine(node.InnerText);
                }
            }
        }
    }
}
