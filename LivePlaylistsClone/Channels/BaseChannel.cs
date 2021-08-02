using System.Collections.Generic;
using FluentScheduler;
using LivePlaylistsClone.Models;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using SpotifyAPI.Web;

namespace LivePlaylistsClone.Channels
{
    public abstract class BaseChannel : Registry, IJob
    {
        protected string StreamUrl;

        private const string auddio_token = "bb69028c3a890fb949361f676519cb02"; // get your own token at https://dashboard.audd.io/

        private const string spotify_token =
            "BQB3004GXatLUIC_00UIZSxgHEV7qe8wttJPCbptt9cafu7MhrdiKORPTy3EUGP7zRqm_ohssgsX95u80ns"; // https://github.com/JMPerez/spotify-web-api-token

        private readonly SpotifyClient _spotify;

        public BaseChannel()
        {
            _spotify = new SpotifyClient(spotify_token);
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

        protected async void SaveSongToPlaylist(string song_link)
        {
            using (WebClient webClient = new WebClient())
            {
                string content = webClient.DownloadString(song_link);
                var match = Regex.Match(content, "data-uri=\"spotify://track/([0-9a-zA-Z]+)\"");

                if (match.Success)
                {
                    string trackId = match.Groups[1].Value;

                    RootUris uris = new RootUris();
                    uris.AddUri($"spotify:track:{trackId}");

                    string json = JsonConvert.SerializeObject(uris);

                    var item = new PlaylistAddItemsRequest(new List<string> {json});
                    await _spotify.Playlists.AddItems("5mLHWcR8C3ObKYdKxTyzyY", item);
                }
            }
        }
    }
}