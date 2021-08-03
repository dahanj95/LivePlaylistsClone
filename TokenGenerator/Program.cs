using PuppeteerSharp;
using System;
using System.IO;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace TokenGenerator
{
    class Program
    {
        private static Page _view;
        private const string userName = "SPOTIFY_USERNAME";
        private const string passWord = "SPOTIFY_PASSWORD";

        static async Task Main(string[] args)
        {
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                DefaultViewport = new ViewPortOptions { DeviceScaleFactor = 1.0 },
                Args = new[] { "--disable-blink-features=AutomationControlled" },
                ExecutablePath = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe"
            });

            _view = await browser.NewPageAsync();

            await Login();
            string token = await GetToken();

            await browser.CloseAsync();

            File.WriteAllText(".\\net5.0\\token.txt",token);
        }

        static async Task Login()
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

        static async Task<string> GetToken()
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
