using System.Threading.Tasks;

namespace LivePlaylistsClone.Contracts
{
    public interface ISleepable
    {
        Task Sleep(int timeout);
    }
}