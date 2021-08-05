using System.IO;
using System.Linq;
using LivePlaylistsClone.Models;
using Newtonsoft.Json;

namespace LivePlaylistsClone.DAL
{
    public class PlaylistRepository
    {
        private readonly string _channelName;

        private readonly Playlist _playlist;

        public int Count => _playlist.tracks.items.Count;
        public Item Last => _playlist.tracks.items.Last();

        public PlaylistRepository(string channelName)
        {
            _channelName = channelName;

            _playlist = ReadPlaylistFromDatabase();
        }

        private Playlist ReadPlaylistFromDatabase()
        {
            string raw = File.ReadAllText($".\\{_channelName}\\playlist.json");
            return JsonConvert.DeserializeObject<Playlist>(raw);
        }

        public void SavePlaylistToDatabase(Playlist playlist)
        {
            string raw = JsonConvert.SerializeObject(playlist);
            File.WriteAllText($".\\{_channelName}\\playlist.json", raw);
        }

        public void RemoveTrackFromLocalPlaylist(Item item)
        {
            _playlist.tracks.items.Remove(item);
        }
    }
}