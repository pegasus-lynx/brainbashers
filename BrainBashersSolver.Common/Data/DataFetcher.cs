using PuppeteerSharp;

namespace BrainBashersSolver.Common.Data
{
    public static class DataFetcher
    {
        public static async Task<string> GetPageContentAsync(string url)
        {
            if (s_Browser is null)
                await InitializeAsync();

            using var page = await s_Browser!.NewPageAsync();
            await page.GoToAsync(url, WaitUntilNavigation.Networkidle0);
            return await page.GetContentAsync();
        }

        private static async Task InitializeAsync()
        {
            await EnsureBrowserDownloaded();
            LaunchOptions launchOptions = new() { Headless = true };
            s_Browser = await Puppeteer.LaunchAsync(launchOptions);
        }

        private static async Task EnsureBrowserDownloaded()
        {
            await new BrowserFetcher().DownloadAsync();
        }

        private static IBrowser? s_Browser = null;
    }
}
