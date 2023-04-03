using PuppeteerSharp;

namespace FOLSharpConnect;

public class FOL
{
    private static bool ChromeSetupComplete = false;
    
    public static async Task SetupChrome()
    {
        if (ChromeSetupComplete)
            return;
        
        Console.WriteLine("Setting Up Chrome...");
        using BrowserFetcher browserFetcher = new();
        await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
        Console.WriteLine("Chrome Setup Complete!");
        
        ChromeSetupComplete = true;
    }
}