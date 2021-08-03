using System;
using System.IO;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace LivePlaylistsClone.Clients
{
    public class SpotifyClient : IDisposable
    {
        private Page _view;
        private Browser _browser;

        private const string userName = "";
        private const string passWord = "";

        public SpotifyClient()
        {
            InitBrowser().Wait();
        }

        private async Task InitBrowser()
        {
            _browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false,
                DefaultViewport = new ViewPortOptions { DeviceScaleFactor = 1.0 },
                Args = new[] { "--disable-blink-features=AutomationControlled" },
                ExecutablePath = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe"
            });

            _view = await _browser.NewPageAsync();
        }

        public async void Dispose()
        {
            await _browser.CloseAsync();
        }

        public async Task Login()
        {
            await _view.GoToAsync("https://accounts.spotify.com/en/login");

            var userNameForm = await _view.XPathAsync("//input[@ng-model='form.username']");
            var passWordForm = await _view.XPathAsync("//input[@ng-model='form.password']");

            await userNameForm[0].TypeAsync(userName);
            await passWordForm[0].TypeAsync(passWord);

            await Task.WhenAll(
                _view.ClickAsync("#login-button"),
                _view.WaitForNavigationAsync()
            );
        }

        public async Task<string> GetToken()
        {
            await _view.GoToAsync("https://developer.spotify.com/console/post-playlist-tracks/");

            var btnRequestToken = await _view.XPathAsync("//*[@id=\"console-form\"]/div[4]/div/span/button");
            await btnRequestToken[0].ClickAsync();

            await _view.WaitForTimeoutAsync(2000);

            await _view.ClickAsync("#scope-playlist-modify-public");
            await _view.ClickAsync("#scope-playlist-modify-private");

            await _view.WaitForTimeoutAsync(1000);

            var btnAgree = await _view.XPathAsync("//div[@id='onetrust-close-btn-container']/button");
            await btnAgree[0].ClickAsync();

            await _view.WaitForTimeoutAsync(750);

            await _view.ClickAsync("#oauthRequestToken");

            await _view.WaitForTimeoutAsync(1000);

            return await _view.EvaluateExpressionAsync<string>("document.getElementById('oauth-input').value");
        }
    }
}