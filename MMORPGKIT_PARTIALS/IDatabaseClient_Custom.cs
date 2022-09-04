using Cysharp.Threading.Tasks;
using LiteNetLibManager;

namespace MultiplayerARPG.MMO
{
    public partial interface IDatabaseClient
    {
        UniTask<AsyncResponseData<EmptyMessage>> UpdateUserLoginAsync(CreateUserLoginReq request);
    }
}
