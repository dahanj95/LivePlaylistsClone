using System.Text;

namespace LivePlaylistsClone.Models
{
    public class Spotify
    {
        public string id { get; set; }
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

    public class AuddioResult
    {
        public Result result { get; set; }
    }
}