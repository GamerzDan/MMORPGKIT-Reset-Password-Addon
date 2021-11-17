using Cysharp.Threading.Tasks;
using LiteNetLibManager;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MultiplayerARPG.MMO
{
    public partial class DatabaseNetworkManager
    {
        protected async UniTaskVoid UpdateUserLogin(RequestHandlerData requestHandler, CreateUserLoginReq request, RequestProceedResultDelegate<EmptyMessage> result)
        {
#if UNITY_STANDALONE && !CLIENT_BUILD
            cachedUsernames.Add(request.Username);
            await Database.UpdateUserLogin(request.Username, request.Password);
            result.Invoke(AckResponseCode.Success, EmptyMessage.Value);
#endif
        }

    }
}