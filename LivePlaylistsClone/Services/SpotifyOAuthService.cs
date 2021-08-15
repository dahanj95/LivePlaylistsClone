using FluentScheduler;
using LivePlaylistsClone.Clients;
using System;

namespace LivePlaylistsClone.Services
{
    public sealed class SpotifyOAuthService : Registry, IJob
    {
        public static SpotifyOAuthService Instance = new();

        public event EventHandler<string> OAuthTokenGenerated;

        private SpotifyOAuthService()
        {
            Schedule(Execute).ToRunEvery(55).Minutes();
        }

        public async void Execute()
        {
            using (SpotifyClient client = new SpotifyClient())
            {
                await client.GoToLoginView();
                await client.FillLoginForm();
                await client.SubmitLogin();

                await client.GoToTokenView();
                await client.ShowPrivilegeDialog();
                await client.Sleep(2000);
                await client.FillPrivilegeForm();
                await client.Sleep(1000);
                await client.AgreePolicy();
                await client.Sleep(750);
                await client.SubmitPrivilegeForm();
                await client.Sleep(1000);

                string oauth_token = await client.GetOAuthToken();
                OAuthTokenGenerated?.Invoke(this, oauth_token);
            }
        }
    }
}