using FluentScheduler;
using LivePlaylistsClone.Models;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using SpotifyAPI.Web;

namespace LivePlaylistsClone.Channels
{
    public abstract class BaseChannel : Registry, IJob
    {
        protected string StreamUrl;

        private const string auddio_token = "bb69028c3a890fb949361f676519cb02"; // get your own token at https://dashboard.audd.io/
        private const string spotify_client_id = "d669b91046f44f16897972105ac88953"; // get your own tokens at https://developer.spotify.com/dashboard
        private const string spotify_client_secret = "ab424e4c2d5e4f218ea67666f130b648";

        private readonly SpotifyClient _spotify;

        public BaseChannel()
        {
            var config = SpotifyClientConfig.CreateDefault();

            var request = new ClientCredentialsRequest(spotify_client_id, spotify_client_secret);
            var response = new OAuthClient(config).RequestToken(request);

            _spotify = new SpotifyClient(config.WithToken(response.Result.AccessToken));
        }

        public abstract void Execute();

        // This method saves a 128KB chunk of the steam to a local file
        protected void SaveChunkToFile(string fileName)
        {
            HttpWebRequest HttpRequest = (HttpWebRequest)WebRequest.Create(StreamUrl);

            HttpWebResponse HttpResponse = (HttpWebResponse)HttpRequest.GetResponse();
            using (Stream ResponseStream = HttpResponse.GetResponseStream())
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead = 0;

                    while (bytesRead < 128000) // 128KB is enough to sample, 0:08 seconds length
                    {
                        ResponseStream.Read(buffer, 0, 4096);
                        fileStream.Write(buffer);
                        bytesRead += 4096;
                    }
                }
            }
        }

        // this method uploads the saved chunk to the api for recognition
        protected Root UploadChunkToAPI(string fileName)
        {
            NameValueCollection formData = new NameValueCollection();
            formData.Add("api_token", auddio_token);
            formData.Add("return", "spotify");

            string jsonResult = ExecuteRequestSendFile("https://api.audd.io/recognize", formData, fileName);
            return JsonConvert.DeserializeObject<Root>(jsonResult);
        }

        private string ExecuteRequestSendFile(string url, NameValueCollection paramters, string filePath)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.QueryString = paramters;
                var responseBytes = webClient.UploadFile(url, filePath);

                string response = Encoding.UTF8.GetString(responseBytes);
                return response;
            }
        }

        protected async void SaveSongToPlaylist(string artist, string title, string album)
        {
            SearchRequest searchRequest = new SearchRequest(SearchRequest.Types.Track, $"{artist} {title}");
            var searchReuslt = await _spotify.Search.Item(searchRequest);

            if (searchReuslt.Tracks.Total < 1)
            {
                return;
            }

            var track = searchReuslt.Tracks.Items[0].Id;
            var playlists = await _spotify.Playlists.CurrentUsers();

            foreach (SimplePlaylist playlist in playlists.Items)
            {
                if (playlist.Name.Equals("Galgalatz - 100 Last Songs"))
                {
                    string id = playlist.Id;
                }
            }

            //PlaylistAddItemsRequest req = new PlaylistAddItemsRequest();

            int i = 0;
        }
    }
}