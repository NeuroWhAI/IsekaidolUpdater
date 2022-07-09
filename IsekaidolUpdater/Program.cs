using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;

namespace IsekaidolUpdater
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length < 2 || args.Length % 2 != 0)
            {
                Console.WriteLine("url interval ...");
                return;
            }

            var tasks = new List<Task>();

            for (int i = 0; i < args.Length; i += 2)
            {
                string url = args[i];
                var interval = TimeSpan.FromMilliseconds(int.Parse(args[i + 1]));

                Console.WriteLine("URL : {0}, Interval : {1}", url, interval.TotalMilliseconds);

                tasks.Add(Job(url, interval));
            }

            await Task.WhenAll(tasks);
        }

        static async Task Job(string url, TimeSpan interval)
        {
            HttpClient client = new HttpClient();

            while (true)
            {
                try
                {
                    var start = DateTime.UtcNow;
                    var res = await client.GetAsync(url);
                    Console.WriteLine("Status : {0}", res.StatusCode);
                    if (res.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string content = await res.Content.ReadAsStringAsync();
                        Console.WriteLine("Content : {0}", content);
                    }
                    var delay = interval - (DateTime.UtcNow - start);
                    if (delay.TotalMilliseconds > 1)
                    {
                        await Task.Delay(delay);
                    }
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                    Console.WriteLine(err.StackTrace);

                    await Task.Delay(4000);
                }
            }
        }
    }
}
