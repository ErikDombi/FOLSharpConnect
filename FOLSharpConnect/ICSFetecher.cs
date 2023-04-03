using System.Formats.Tar;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices;
using PuppeteerSharp;
using Calendar = Ical.Net.Calendar;

namespace FOLSharpConnect;

public class ICSFetecher
{
    public static async Task<Calendar> FetchICS(string username, string password)
    {
        FOL.SetupChrome();
        var browser = await Puppeteer.LaunchAsync(new LaunchOptions()
        {
            Headless = !RuntimeInformation.IsOSPlatform(OSPlatform.Windows),
            Args = new []
            {
                "--no-sandbox"
            }
        });
        
        var page = await browser.NewPageAsync();
        var navWait = page.WaitForNavigationAsync(new NavigationOptions() { WaitUntil = new []
        {
            WaitUntilNavigation.DOMContentLoaded
        }});
        await page.GoToAsync("https://www.fanshaweonline.ca/", WaitUntilNavigation.DOMContentLoaded);
        await navWait;
        await Task.Delay(1000);
        
        await page.ScreenshotAsync("prePass.png");
        await page.WaitForSelectorAsync("#password");
        
        var usernameField = await page.QuerySelectorAsync("#username");
        var passwordField = await page.QuerySelectorAsync("#password");
        var loginButton = await page.QuerySelectorAsync("body > div > div > div > div.column.one > form > div:nth-child(4) > button");

        await usernameField.TypeAsync(username);
        await passwordField.TypeAsync(password);
        
        navWait = page.WaitForNavigationAsync(new NavigationOptions() { WaitUntil = new []
        {
            WaitUntilNavigation.DOMContentLoaded
        }});
        
        await loginButton.ClickAsync();

        await navWait;
        
        await Task.Delay(1000);
        
        await page.GoToAsync("https://www.fanshaweonline.ca/d2l/le/calendar/0/subscribe/subscribeDialogLaunch?subscriptionOptionId=-1");

        await page.WaitForSelectorAsync(".subscribe-feed-url-partial-container > div");
        
        var linkElem = await page.QuerySelectorAsync(".subscribe-feed-url-partial-container > div");
        var linkProperty = await linkElem.GetPropertyAsync("innerText");
        var link = linkProperty.RemoteObject.Value.ToString();

        browser.CloseAsync();
        
        using WebClient webClient = new();

        var iCalString = await webClient.DownloadStringTaskAsync(new Uri(link));

        return Calendar.Load(iCalString);
    }
}