using System.Threading.Tasks;

namespace LivePlaylistsClone.Contracts
{
    public interface ILoginable
    {
        Task GoToLoginView();
        Task FillLoginForm();
        Task SubmitLogin();
    }
}