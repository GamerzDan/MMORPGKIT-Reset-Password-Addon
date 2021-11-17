using Cysharp.Threading.Tasks;
using LiteNetLibManager;

namespace MultiplayerARPG.MMO
{
    public partial class DatabaseNetworkManager
    {
        public async UniTask<EmptyMessage> UpdateUserLoginAsync(CreateUserLoginReq request)
        {
            var result = await Client.SendRequestAsync<CreateUserLoginReq, EmptyMessage>(DatabaseRequestTypes.RequestUpdateUserLogin, request);
            if (result.ResponseCode != AckResponseCode.Success)
                return EmptyMessage.Value;
            return result.Response;
        }
    }
}
