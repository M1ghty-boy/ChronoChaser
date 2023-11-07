using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

class Program
{
    private static Dictionary<string, string> htmlCache = new Dictionary<string, string>();
    private static Timer timer;

    static async Task Main(string[] args)
    {
        var urls = new List<string>
        {
            //urls here
            //too lazy for file rn
        };

        foreach (var url in urls)
        {
            htmlCache[url] = "";
        }

        // Set up the timer and the callback method
        timer = new Timer(async _ =>
        {
            Console.WriteLine("Checking URLs...");
            await CheckUrls(urls);
        },
        null,
        TimeSpan.Zero,
        TimeSpan.FromSeconds(10));

        // just to keep it running
        Console.WriteLine("Press Enter to exit the program...");
        Console.ReadLine();
    }

    static async Task CheckUrls(List<string> urls)
    {
        using (var httpClient = new HttpClient())
        {
            foreach (var url in urls)
            {
                try
                {

                    var currentContent = await httpClient.GetStringAsync(url);

                    // Check if the content has changed since the last check
                    if (htmlCache[url] != currentContent)
                    {
                        if (htmlCache[url] != "") { 
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine($"{DateTime.Now.ToString()} - {url}> Change detected!");
                            Console.BackgroundColor = ConsoleColor.Black;
                            // do something here
                        }
                        else
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan; Console.WriteLine($"{DateTime.Now.ToString()} - {url}> First instance copied to memory");
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                        // update cache
                        htmlCache[url] = currentContent;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine($"{DateTime.Now.ToString()} - {url}> No change");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while checking {url}: {ex.Message}");
                }
            }
        }
    }
}
