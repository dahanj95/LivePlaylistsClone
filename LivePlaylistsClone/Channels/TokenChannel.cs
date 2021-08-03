using FluentScheduler;
using System.Diagnostics;
using System.IO;

namespace LivePlaylistsClone.Channels
{
    public class TokenChannel : BaseChannel, IJob
    {
        public TokenChannel()
        {
            Schedule(Execute).ToRunEvery(57).Minutes();
        }

        public new void Execute()
        {
            var p = Process.Start(".\\net5.0\\tokengenerator.exe");
            p.WaitForExit();

            spotify_token = File.ReadAllText(".\\net5.0\\token.txt");
        }
    }
}