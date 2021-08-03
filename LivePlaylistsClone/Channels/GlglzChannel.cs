using LivePlaylistsClone.Models;
using System;
using System.IO;

namespace LivePlaylistsClone.Channels
{
    public class GlglzChannel : BaseChannel
    {
        public GlglzChannel()
        {
            ChannelName = "glglz";
            StreamUrl = "https://glzwizzlv.bynetcdn.com/glglz_mp3";
        }
    }
}