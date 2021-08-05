using System.Collections.Generic;

namespace LivePlaylistsClone.Models
{
    public class Track1
    {
        public string uri { get; set; }
        public List<int> positions { get; set; }

        public Track1()
        {
            positions = new List<int>();
        }
    }

    public class Root1
    {
        public List<Track1> tracks { get; set; }

        public Root1()
        {
            tracks = new List<Track1>();
        }
    }
}