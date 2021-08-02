using FluentScheduler;
using LivePlaylistsClone.Models;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace LivePlaylistsClone.Channels
{
    public abstract class BaseChannel : Registry, IJob
    {
        protected string StreamUrl;

        private const string auddio_token = "bb69028c3a890fb949361f676519cb02"; // https://dashboard.audd.io/
        private const string spotify_token = "BQClIPvQX-zLm4TjtipaCL2cElwXMg2-i6rBZUN1AVH4FPh5q4EsE62cWComEQavpFEXdJ9vvXb79nCc_DG9j9FCA7eaaSy20BWj3aJp2yDe1XCLCs1mRLCz_3OlD94NBDOYrLYMYN2LA75H-WTmBUQXncyVZqg2-B-6Y7bhyZ-nA982MF6--PyT1Is3LQL-Jvn5JWSTtuJGx1Kmi2whxM61h5hDT8o4iJJwe1ZhUDmquaZHpR6PwqdBPC-lctdgeTYPKDtfa0aiH8NP3kPdjHk5TDEpQr7-9a-O-TQLKS0w"; // https://developer.spotify.com/console/post-playlist-tracks/?playlist_id=&position=&uris=

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

        protected void AddSongToPlaylistByTrackId(string trackId)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Headers.Add(HttpRequestHeader.Accept, "application/json");
                webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                webClient.Headers.Add(HttpRequestHeader.Authorization, $" Bearer {spotify_token}");

                NameValueCollection values = new NameValueCollection();
                values.Add("uris", $"spotify:track:{trackId}");

                webClient.QueryString = values;

                string endpoint = "https://api.spotify.com/v1/playlists/5mLHWcR8C3ObKYdKxTyzyY/tracks";

                string response = webClient.UploadString(endpoint, "");
            }
        }

        protected Track ExtractTrack(string song_link)
        {
            using (WebClient webClient = new WebClient())
            {
                string content = webClient.DownloadString(song_link);
                var match = Regex.Match(content, "data-uri=\"spotify://track/([0-9a-zA-Z]+)\"");

                if (match.Success)
                {
                    return new Track { Id = match.Groups[1].Value };
                }

                return new Track();
            }
        }

        protected Track ReadTrackFromDatabase(string channelName)
        {
            if (File.Exists($".\\{channelName}\\database.json"))
            {
                string raw = File.ReadAllText($".\\{channelName}\\database.json");
                return JsonConvert.DeserializeObject<Track>(raw);
            }

            return null;
        }

        protected void SaveTrackToDatabase(Track track, string channelName)
        {
            string raw = JsonConvert.SerializeObject(track);
            File.WriteAllText($".\\{channelName}\\database.json", raw);
        }
    }
}