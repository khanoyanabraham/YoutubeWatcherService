using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumProxyAuthentication;
using System.Xml.Linq;
using YoutubeWatcherService.SeleniumVideoByPass;

namespace YoutubeWatcherService.Watcher
{
    public sealed class WatchUnit : IDisposable
    {
        private readonly ChromeDriver driver;
        private INetwork? networkInterceptor;
        private readonly string url;
        private readonly string proxy;
        private readonly int watchTime;

        public WatchUnit(string url, string proxy, int watchTime)
        {
            ChromeOptions edgeOptions = new ChromeOptions();
            edgeOptions.AddArgument("headless");
            edgeOptions.AddArgument("disable-gpu");
            edgeOptions.AddArgument("--no-sandbox");
            edgeOptions.AddArgument("--disable-dev-shm-usage");
            edgeOptions.AddArgument("--disable-plugins");
            edgeOptions.AddArgument("--disable-notifications");
            edgeOptions.AddArgument("--disable-infobars");
            edgeOptions.AddArgument("--log-level=3");
            edgeOptions.AddArgument("--silent");
            edgeOptions.AddArgument("--memory-pressure-off");
            edgeOptions.AddArgument("--disable-background-networking");
            edgeOptions.AddArgument("--disable-background-timer-throttling");
            edgeOptions.AddArgument("--disable-backgrounding-occluded-windows");
            edgeOptions.AddArgument("--disable-breakpad");
            edgeOptions.AddArgument("--disable-client-side-phishing-detection");
            edgeOptions.AddArgument("--disable-default-apps");
            edgeOptions.AddArgument("--disable-hang-monitor");
            edgeOptions.AddArgument("--disable-popup-blocking");
            edgeOptions.AddArgument("--disable-prompt-on-repost");
            edgeOptions.AddArgument("--disable-renderer-backgrounding");
            edgeOptions.AddArgument("--disable-sync");
            edgeOptions.AddArgument("--metrics-recording-only");
            edgeOptions.AddArgument("--no-first-run");
            edgeOptions.AddArgument("--safebrowsing-disable-auto-update");
            edgeOptions.AddArgument("--enable-automation");
            edgeOptions.AddArgument($"proxy-server={proxy}");
            edgeOptions.AddArguments("accept-lang=en-EN");
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            driver = new ChromeDriver(chromeDriverService, edgeOptions, TimeSpan.FromHours(1));
            this.url = url;
            this.proxy = proxy;
            this.watchTime = watchTime;
        }
        public async Task WatchAsync(CancellationToken token)
        {
            try
            {
                NetworkAuthenticationHandler handler = new()
                {
                    UriMatcher = (d) => true,
                    Credentials = new PasswordCredentials("khanoyanabraham", "BPBdYPxvZz")
                };
                Console.WriteLine($"Video Processing Stared");

                networkInterceptor = driver.Manage().Network;
                networkInterceptor.AddAuthenticationHandler(handler);
                networkInterceptor.NetworkRequestSent += NetworkInterceptor_NetworkRequestSent;
                await networkInterceptor.StartMonitoring();
                Console.WriteLine($"Pass 1");

                driver.Navigate().GoToUrl(url);
                Console.WriteLine($"Pass 2");

                await networkInterceptor.StopMonitoring();
                await Task.Delay(5000, token);
                AcceptCookieByPass acceptCookieByPass = new AcceptCookieByPass();
                acceptCookieByPass.ByPass(driver);
                await Task.Delay(5000, token);
                EnSureVideoPlayingByPass enSureVideoPlayingByPass = new EnSureVideoPlayingByPass();
                enSureVideoPlayingByPass.EnsureVideoPlaying(driver);
                await Task.Delay(5000, token);

                if (enSureVideoPlayingByPass.EnsureVideoPlaying(driver))
                {
                    try
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            driver.Navigate().Refresh();
                            await Task.Delay(TimeSpan.FromSeconds(watchTime / 10), token);
                        }
                    }
                    catch
                    {
                        //ignored to be disposed
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void NetworkInterceptor_NetworkRequestSent(object? sender, NetworkRequestSentEventArgs e)
        {

        }

        public void Dispose()
        {
            if (networkInterceptor != null)
                networkInterceptor.StopMonitoring();
            driver.Close();
            driver.CloseDevToolsSession();
            driver.Dispose();

        }
    }
}
