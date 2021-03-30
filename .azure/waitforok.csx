using System;
using System.Net.Http;
using System.Collections.Generic;

int delayBase = 128;

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

foreach (KeyValuePair<string, int> kvp in urls) 
{
    string url = kvp.Key;
    
    bool success = false;
    double delay = delayBase;

    while (!success) {
        try {
            var response = await client.GetAsync(url);
            
            success = response.IsSuccessStatusCode;
        } catch {
            success = false;
        }
        
        if (success) 
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