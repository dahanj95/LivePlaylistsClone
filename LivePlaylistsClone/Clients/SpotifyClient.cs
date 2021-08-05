using LivePlaylistsClone.Contracts;
using PuppeteerSharp;
using System;
using System.Threading.Tasks;
using DotNetEnv;

namespace LivePlaylistsClone.Clients
{
    public class SpotifyClient : IDisposable, ILoginable, ITokenRequester
    {
        private Page _view;
        private Browser _browser;

        private readonly string userName = "";
        private readonly string passWord = "";

        public SpotifyClient()
        {
            InitBrowser().Wait();

            userName = Env.GetString("SPOTIFY_ID");
            passWord = Env.GetString("SPOTIFY_PW");
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

        public async Task GoToLoginView()
        {
            await _view.GoToAsync("https://accounts.spotify.com/en/login");
        }

        public async Task FillLoginForm()
        {
            var userNameForm = await _view.XPathAsync("//input[@ng-model='form.username']");
            var passWordForm = await _view.XPathAsync("//input[@ng-model='form.password']");

            await userNameForm[0].TypeAsync(userName);
            await passWordForm[0].TypeAsync(passWord);
        }

        public async Task SubmitLogin()
        {
            await Task.WhenAll(
                _view.ClickAsync("#login-button"),
                _view.WaitForNavigationAsync()
            );
        }

        public async Task GoToTokenView()
        {
            await _view.GoToAsync("https://developer.spotify.com/console/post-playlist-tracks/");
        }

        public async Task ShowPrivilegeDialog()
        {
            var btnRequestToken = await _view.XPathAsync("//*[@id='console-form']/div[4]/div/span/button");
            await btnRequestToken[0].ClickAsync();
        }

        public async Task FillPrivilegeForm()
        {
            await _view.ClickAsync("#scope-playlist-modify-public");
            await _view.ClickAsync("#scope-playlist-modify-private");
        }

        public async Task AgreePolicy()
        {
            var btnAgree = await _view.XPathAsync("//div[@id='onetrust-close-btn-container']/button");
            await btnAgree[0].ClickAsync();
        }

        public async Task SubmitPrivilegeForm()
        {
            await _view.ClickAsync("#oauthRequestToken");
            await _view.WaitForTimeoutAsync(1000);
        }

        public async Task Sleep(int timeout)
        {
            await _view.WaitForTimeoutAsync(timeout);
        }

        public async Task<string> GetOAuthToken()
        {
            return await _view.EvaluateExpressionAsync<string>("document.getElementById('oauth-input').value");
        }
    }
}