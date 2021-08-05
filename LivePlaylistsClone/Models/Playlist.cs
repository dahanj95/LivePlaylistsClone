using System.Collections.Generic;
using System;

namespace LivePlaylistsClone.Models
{
    public class RemoteTrack
    {
        public string id { get; set; }
    }

    public class Item
    {
        public RemoteTrack track { get; set; }
    }

    public class Playlist
    {
        public List<Item> items { get; set; }
    }


}