using System;
using System.Net.Http;
// using System.Timers;
using System.Collections.Generic;

// int timeOut = 60;
int delayBase = 128;

// Timer timer = new Timer(timeOut * 1000);
// timer.Elapsed += (s, args) => {
//     Console.Error.WriteLine($"Timeout ({timeOut} seconds) expired, exiting");
//     Environment.Exit(1);
// };

HttpClient client = new HttpClient();

IDictionary<string, int> urls = new Dictionary<string, int>();

foreach (string url in Args)
{
    if (string.IsNullOrWhiteSpace(url))
    {
        continue;
    }

    if (!Uri.TryCreate(url, UriKind.Absolute, out _))
    {
        Console.Error.WriteLine($"Failed to parse url {url}!");
        Environment.Exit(1);
    }
    
    Console.Out.WriteLine($"Adding url {url} to urls to poll");
    urls.Add(url, 0);
}

// Console.Out.WriteLine($"Timeout: {timeOut} seconds");

// timer.Start();

foreach (KeyValuePair<string, int> kvp in urls) 
{
    string url = kvp.Key;
    
    bool success = false;
    double delay = delayBase;

    while (!success) {
        var response = await client.GetAsync(url);
        
        success = response.IsSuccessStatusCode;
        
        if (response.IsSuccessStatusCode) 
        {
           Console.Out.WriteLine($"{url} is ready, statuscode {response.StatusCode}");
        } else {
            delay *= 1.5d;
            Console.Out.WriteLine($"{url} is not ready, statuscode {response.StatusCode}, waiting {delay}ms");
            await Task.Delay(TimeSpan.FromMilliseconds(delay));
        }
    }
}

Console.Out.WriteLine("All urls are ready")