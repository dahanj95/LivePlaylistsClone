using LivePlaylistsClone.Models;
using System;
using System.IO;

namespace LivePlaylistsClone.Channels
{
    public class Kan88Channel : BaseChannel
    {
        public Kan88Channel()
        {
            ChannelName = "kan88";
            StreamUrl = "https://kanliveicy.media.kan.org.il/icy/749623_mp3";
            PlaylistId = "3SpUq03whlfRMwEHMRulNy";
        }
    }
}