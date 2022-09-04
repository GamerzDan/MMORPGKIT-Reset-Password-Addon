using Cysharp.Threading.Tasks;
using LiteNetLibManager;

namespace MultiplayerARPG.MMO
{
    public partial class DatabaseNetworkManager
    {
        public async UniTask<AsyncResponseData<EmptyMessage>> UpdateUserLoginAsync(CreateUserLoginReq request)
        {
            var result = await Client.SendRequestAsync<CreateUserLoginReq, EmptyMessage>(DatabaseRequestTypes.RequestUpdateUserLogin, request);
            if (!result.IsSuccess)
                Logging.LogError(nameof(DatabaseNetworkManager), $"Cannot {nameof(CreateUserLoginAsync)} status: {result.ResponseCode}");
            return result;
        }
    }
}
