﻿using FluentScheduler;
using LivePlaylistsClone.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using LivePlaylistsClone.Clients;

namespace LivePlaylistsClone.Channels
{
    public abstract class BaseChannel : Registry, IJob
    {
        protected string StreamUrl;
        protected string ChannelName;
        protected string PlaylistId;

        private string auddio_token = ""; // https://dashboard.audd.io/
        private string spotify_token; // https://developer.spotify.com/console/post-playlist-tracks/?playlist_id=&position=&uris=

        public BaseChannel()
        {
            // scheduled
            Schedule(Execute).ToRunNow().AndEvery(30).Seconds();

            // run now
            Schedule(ReadSpotifyToken).ToRunNow();
            Schedule(CreateWorkingFolder).ToRunNow();
        }

        public void Execute()
        {
            SaveChunkToFile($".\\{ChannelName}\\sample.mp3");
            var root = UploadChunkToAPI($".\\{ChannelName}\\sample.mp3");

            if (root.result?.spotify == null)
            {
                File.AppendAllText(".\\log.txt", root.result?.ToString());
                // if we got here, this means there wasn't a song playing
                // reasons: traffic highlights, breaking news, or some talk show
                // or
                // there's no Spotify recognition id for the current song
                return;
            }

            Track track = ExtractTrack(root.result);
            Track dbTrack = ReadTrackFromDatabase(ChannelName);

            if (dbTrack != null)
            {
                if (track.Equals(dbTrack))
                {
                    // we matched the same song in the same timeframe (different offset)
                    // so we exit from the subroutine until there's a
                    // new match.
                    // to-do: implement a potential manual search logic on Spotify
                    return;
                }
            }

            AddSongToPlaylistByTrackId(track.Id);
            SaveTrackToDatabase(track, ChannelName);

            string rootContent = root.result.ToString();

            Console.WriteLine(rootContent);
            File.WriteAllText($".\\{ChannelName}\\log.txt", rootContent);
        }

        protected void SetOAuthToken(string oauth_token)
        {
            lock (spotify_token)
            {
                spotify_token = oauth_token;
            }
        }

        private void CreateWorkingFolder()
        {
            if (!Directory.Exists($".\\{ChannelName}\\"))
            {
                Directory.CreateDirectory($".\\{ChannelName}\\");
            }
        }

        private void ReadSpotifyToken()
        {
            lock (spotify_token)
            {
                spotify_token = File.ReadAllText(".\\token.txt");
            }
        }

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
            formData.Add("return", "spotify, apple_music");

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
                values.Add("position", "0");

                webClient.QueryString = values;

                string endpoint = $"https://api.spotify.com/v1/playlists/{PlaylistId}/tracks";
                string response = webClient.UploadString(endpoint, "");
            }
        }

        protected Track ExtractTrack(Result result)
        {
            return new Track
            {
                Id = result.spotify.id, 
                Title = result.title
            };
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