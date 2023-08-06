using Cysharp.Threading.Tasks;
using LiteNetLibManager;

namespace MultiplayerARPG.MMO
{
    public partial class DatabaseNetworkManager
    {
        public async UniTask<DatabaseApiResult> UpdateUserLoginAsync(CreateUserLoginReq request)
        {
            return await SendRequest(request, DatabaseRequestTypes.RequestUpdateUserLogin, nameof(UpdateUserLoginAsync));
        }
    }
}