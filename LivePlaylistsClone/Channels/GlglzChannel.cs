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
            //SaveChunkToFile(".\\glglz\\sample.mp3");
            //var root = UploadChunkToAPI(".\\glglz\\sample.mp3");

            //if (root.result == null)
            //{
            //    if we got here, this means there wasn't a song playing
            //     reason: traffic highlights, breaking news, or some talk show
            //    return;
            //}

            AddSongToPlaylist("https://lis.tn/SummerWine");

            //SaveSongToPlaylist(root.result.song_link);

            //string rootContent = root.result.ToString();

            //Console.WriteLine(rootContent);
            //File.WriteAllText(".\\glglz\\log.txt", rootContent);
        }
    }
}