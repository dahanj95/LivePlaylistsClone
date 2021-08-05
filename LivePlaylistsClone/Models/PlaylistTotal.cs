using System.Collections.Generic;
using System;

namespace LivePlaylistsClone.Models
{
    public class Tracks
    {
        public int total { get; set; }
    }

    public class PlaylistTotal
    {
        public Tracks tracks { get; set; }
    }
}