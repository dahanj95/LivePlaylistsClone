using LivePlaylistsClone.Clients;
using LivePlaylistsClone.Contracts;
using System.IO;

namespace LivePlaylistsClone.Channels
{
    // it's important to mark only a single class as ITokenGenerator so execution will be once
    public class GlglzChannel : BaseChannel, ITokenGenerator
    {
        public GlglzChannel()
        {
            ChannelName = "glglz";
            StreamUrl = "https://glzwizzlv.bynetcdn.com/glglz_mp3";
            PlaylistId = "5mLHWcR8C3ObKYdKxTyzyY";

            Schedule(GenerateToken).ToRunEvery(55).Minutes();
        }

        // this method generates a new oauth token every 55 minutes
        public async void GenerateToken()
        {
            using (SpotifyClient client = new SpotifyClient())
            {
                await client.GoToLoginView();
                await client.FillLoginForm();
                await client.SubmitLogin();

                await client.GoToTokenView();
                await client.ShowPrivilegeDialog();
                await client.FillPrivilegeForm();
                await client.AgreePolicy();
                await client.SubmitPrivilegeForm();

                string oauth_token = await client.GetOAuthToken();

                SetOAuthToken(oauth_token);
                await File.WriteAllTextAsync(".\\token.txt", oauth_token);
            }
        }
    }
}