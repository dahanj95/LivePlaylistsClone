using System.Collections.Generic;

namespace LivePlaylistsClone.Models
{
    public class RootUris
    {
        public List<string> uris { get; set; }

        public RootUris()
        {
            uris = new List<string>();
        }

        public void AddUri(string uri)
        {
            uris.Add(uri);
        }
    }
}