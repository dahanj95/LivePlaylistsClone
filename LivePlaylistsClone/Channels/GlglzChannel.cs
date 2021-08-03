using LivePlaylistsClone.Models;
using System;
using System.IO;

namespace LivePlaylistsClone.Channels
{
    public class GlglzChannel : BaseChannel
    {
        private const string channel_url = "https://glzwizzlv.bynetcdn.com/glglz_mp3";
        private const int interval_seconds = 30;

        public GlglzChannel()
        {
            StreamUrl = channel_url;
            Schedule(Execute).ToRunNow().AndEvery(interval_seconds).Seconds();

            if (!Directory.Exists(".\\glglz\\"))
            {
                Directory.CreateDirectory(".\\glglz\\");
            }
        }

        public override void Execute()
        {
            SaveChunkToFile(".\\glglz\\sample.mp3");
            var root = UploadChunkToAPI(".\\glglz\\sample.mp3");

            if (root.result == null)
            {
                //if we got here, this means there wasn't a song playing
                // reason: traffic highlights, breaking news, or some talk show
                return;
            }

            Track track = ExtractTrack(root.result.spotify);

            if (track.Id == null)
            {
                // if we got here, this means there's no spotify
                // recognition id for the current song
                return;
            }

            Track dbTrack = ReadTrackFromDatabase("glglz");

            if (dbTrack != null)
            {
                if (track.Id.Equals(dbTrack.Id))
                {
                    // we matched the same song in the same timeframe
                    // so we exit from the subroutine until there's a
                    // new match.
                    return;
                }
            }

            AddSongToPlaylistByTrackId(track.Id);
            SaveTrackToDatabase(track, "glglz");

            string rootContent = root.result.ToString();

            Console.WriteLine(rootContent);
            File.WriteAllText(".\\glglz\\log.txt", rootContent);
        }
    }
}