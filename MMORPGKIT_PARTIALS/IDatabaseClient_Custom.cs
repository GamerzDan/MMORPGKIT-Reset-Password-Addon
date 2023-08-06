using Cysharp.Threading.Tasks;
using LiteNetLibManager;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
#if !NET && !NETCOREAPP
using UnityRestClient;
#endif

namespace MultiplayerARPG.MMO
{
    public partial interface IDatabaseClient
    {
        UniTask<DatabaseApiResult> UpdateUserLoginAsync(CreateUserLoginReq request);
    }


    //IMPLEMENT METHOD FOR THE RESTDATABASECLIENT CLASS HERE
    public partial class RestDatabaseClient : RestClient, IDatabaseClient
    {
        public async UniTask<DatabaseApiResult> UpdateUserLoginAsync(CreateUserLoginReq request)
        {
            return await SendRequest(request, GetUrl(apiUrl, DatabaseApiPath.UpdateUserLoginAsync), nameof(UpdateUserLoginAsync));
        }
    }
    public partial class DatabaseApiPath
    {
        public const string UpdateUserLoginAsync = "update-user-login";
    }
 }