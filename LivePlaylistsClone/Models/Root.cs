using System.Collections.Generic;
using System.Text;

namespace LivePlaylistsClone.Models
{
    public class ExternalUrls
    {
        public string spotify { get; set; }
    }

    public class Artist
    {
        public string name { get; set; }
        public string id { get; set; }
        public string uri { get; set; }
        public string href { get; set; }
        public ExternalUrls external_urls { get; set; }
    }

    public class Image
    {
        public int height { get; set; }
        public int width { get; set; }
        public string url { get; set; }
    }

    public class Album
    {
        public string name { get; set; }
        public List<Artist> artists { get; set; }
        public string album_group { get; set; }
        public string album_type { get; set; }
        public string id { get; set; }
        public string uri { get; set; }
        public object available_markets { get; set; }
        public string href { get; set; }
        public List<Image> images { get; set; }
        public ExternalUrls external_urls { get; set; }
        public string release_date { get; set; }
        public string release_date_precision { get; set; }
    }

    public class ExternalIds
    {
        public string isrc { get; set; }
    }

    public class Spotify
    {
        public Album album { get; set; }
        public ExternalIds external_ids { get; set; }
        public int popularity { get; set; }
        public bool is_playable { get; set; }
        public object linked_from { get; set; }
        public List<Artist> artists { get; set; }
        public object available_markets { get; set; }
        public int disc_number { get; set; }
        public int duration_ms { get; set; }
        public bool @explicit { get; set; }
        public ExternalUrls external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string preview_url { get; set; }
        public int track_number { get; set; }
        public string uri { get; set; }
    }

    public class Result
    {
        public string artist { get; set; }
        public string title { get; set; }
        public string album { get; set; }
        public string release_date { get; set; }
        public string label { get; set; }
        public string timecode { get; set; }
        public string song_link { get; set; }
        public Spotify spotify { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Artist: {artist}");
            builder.AppendLine($"Title: {title}");
            builder.AppendLine($"Album: {album}");
            builder.AppendLine($"Release Date: {release_date}");
            builder.AppendLine($"Label: {label}");
            builder.AppendLine($"Timecode: {timecode}");
            builder.AppendLine($"Song Link: {song_link}");

            return builder.ToString();
        }
    }

    public class Warning
    {
        public int error_code { get; set; }
        public string error_message { get; set; }
    }

    public class Root
    {
        public string status { get; set; }
        public Result result { get; set; }
        public Warning warning { get; set; }
    }
}