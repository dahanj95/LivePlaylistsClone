using System.Threading.Tasks;

namespace LivePlaylistsClone.Contracts
{
    public interface ITokenRequester
    {
        Task GoToTokenView();
        Task ShowPrivilegeDialog();
        Task FillPrivilegeForm();
        Task AgreePolicy();
        Task<bool> IsPolicyPresent();
        Task SubmitPrivilegeForm();
        Task<string> GetOAuthToken();
    }
}