using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

namespace IsekaidolUpdater
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("url interval");
                return;
            }

            string url = args[0];
            var interval = TimeSpan.FromMilliseconds(int.Parse(args[1]));

            Console.WriteLine("URL : {0}, Interval : {1}", url, interval.TotalMilliseconds);

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
                    while (DateTime.UtcNow - start < interval)
                    {
                        await Task.Delay(10);
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
