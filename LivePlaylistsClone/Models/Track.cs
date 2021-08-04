namespace LivePlaylistsClone.Models
{
    public class Track
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public override bool Equals(object? obj) => Id.Equals(((Track)obj).Id);
    }
}