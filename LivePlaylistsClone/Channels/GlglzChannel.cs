using LivePlaylistsClone.Clients;
using LivePlaylistsClone.Contracts;
using LivePlaylistsClone.Models;
using System;
using System.IO;

namespace LivePlaylistsClone.Channels
{
    public class GlglzChannel : BaseChannel, ITokenGenerator
    {
        public GlglzChannel()
        {
            ChannelName = "glglz";
            StreamUrl = "https://glzwizzlv.bynetcdn.com/glglz_mp3";
            PlaylistId = "5mLHWcR8C3ObKYdKxTyzyY";

            Schedule(GenerateToken).ToRunEvery(55).Minutes();
        }

        // this method generates a new token every 55 minutes
        public async void GenerateToken() 
        {
            using (SpotifyClient client = new SpotifyClient())
            {
                await client.Login();
                string token = await client.GetToken();
                SetToken(token);
                File.WriteAllText(".\\token.txt", token);
            }
        }
    }
}