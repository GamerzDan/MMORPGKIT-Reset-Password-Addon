using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using LiteNetLib;
using LiteNetLibManager;
using UnityEngine;

using Mono.Data.Sqlite;
using MySqlConnector;

namespace MultiplayerARPG.MMO
{
    public partial class MMOClientInstance : MonoBehaviour
    {
        public void RequestPasswordReset(string username, string password, ResponseDelegate<ResponseUserRegisterMessage> callback)
        {
            centralNetworkManager.RequestPasswordReset(username, password, callback);
        }
    }

    public static partial class MMORequestTypes
    {
        public const ushort RequestPasswordReset = 5003;
    }

    public abstract partial class BaseDatabase : MonoBehaviour
    {
        #if (UNITY_STANDALONE && !CLIENT_BUILD) || UNITY_EDITOR
        public abstract UniTask UpdateUserLogin(string username, string password, string email);
        #endif
    }

    public partial interface IDatabaseClient
    {
        UniTask<AsyncResponseData<EmptyMessage>> UpdateUserLoginAsync(CreateUserLoginReq request) => new UniTask<AsyncResponseData<EmptyMessage>>();
    }

    public static partial class DatabaseRequestTypes
    {
        public const ushort RequestUpdateUserLogin = 5002;
    }

    public partial class DatabaseNetworkManager : LiteNetLibManager.LiteNetLibManager
    {
        [DevExtMethods("RegisterMessages")]
        protected void DevExtRegisterMessages()
        {
            RegisterRequestToServer<CreateUserLoginReq, EmptyMessage>(DatabaseRequestTypes.RequestUpdateUserLogin, UpdateUserLogin);
        }
        protected async UniTaskVoid UpdateUserLogin(RequestHandlerData requestHandler, CreateUserLoginReq request, RequestProceedResultDelegate<EmptyMessage> result)
        {
#if UNITY_STANDALONE && !CLIENT_BUILD
            if (!DisableDatabaseCaching)
            {
                // Cache username, it will be used to validate later
                await DatabaseCache.AddUsername(request.Username);
            }
            await Database.UpdateUserLogin(request.Username, request.Password, request.Email);
            result.Invoke(AckResponseCode.Success, EmptyMessage.Value);
#endif
        }
        public async UniTask<AsyncResponseData<EmptyMessage>> UpdateUserLoginAsync(CreateUserLoginReq request)
        {
            var result = await Client.SendRequestAsync<CreateUserLoginReq, EmptyMessage>(DatabaseRequestTypes.RequestUpdateUserLogin, request);
            if (!result.IsSuccess)
                Logging.LogError(nameof(DatabaseNetworkManager), $"Cannot {nameof(CreateUserLoginAsync)} status: {result.ResponseCode}");
            return result;
        }
    }

    public partial class SQLiteDatabase : BaseDatabase
    {
        public override async UniTask UpdateUserLogin(string username, string password, string email)
        {
#if UNITY_STANDALONE && UNITY_SERVER
            await UniTask.Yield();
            ExecuteNonQuery("UPDATE userlogin SET password=@password WHERE username=@username",
                new SqliteParameter("@username", username),
                new SqliteParameter("@password", password.PasswordHash()));
#endif
        }
    }

    public partial class MySQLDatabase : BaseDatabase
    {
        public override async UniTask UpdateUserLogin(string username, string password, string email)
        {
 #if UNITY_STANDALONE && UNITY_SERVER
            await ExecuteNonQuery("UPDATE userlogin SET password=@password WHERE username=@username",
                new MySqlParameter("@username", username),
                new MySqlParameter("@password", password.PasswordHash()));
#endif
        }
    }

    public partial class CentralNetworkManager : LiteNetLibManager.LiteNetLibManager
    {
        [DevExtMethods("RegisterMessages")]
        protected void DevExtRegisterMessages()
        {
            RegisterRequestToServer<RequestUserRegisterMessage, ResponseUserRegisterMessage>(MMORequestTypes.RequestPasswordReset, HandleRequestPasswordReset);
        }

        public bool RequestPasswordReset(string username, string password, ResponseDelegate<ResponseUserRegisterMessage> callback)
        {
            return ClientSendRequest(MMORequestTypes.RequestPasswordReset, new RequestUserRegisterMessage()
            {
                username = username,
                password = password,
            }, responseDelegate: callback);
        }

        protected async UniTaskVoid HandleRequestPasswordReset(
            RequestHandlerData requestHandler,
            RequestUserRegisterMessage request,
            RequestProceedResultDelegate<ResponseUserRegisterMessage> result)
        {
#if UNITY_STANDALONE && !CLIENT_BUILD
            UITextKeys message = UITextKeys.NONE;
            string username = request.username;
            string password = request.password;
            string email = request.email;
            DatabaseApiResult<FindUsernameResp> findUsernameResp = await DatabaseClient.FindUsernameAsync(new FindUsernameReq()
            {
                Username = username
            });
            if (findUsernameResp.Response.FoundAmount < 1)
                message = UITextKeys.UI_ERROR_INVALID_USERNAME_OR_PASSWORD;
            else if (string.IsNullOrEmpty(username) || username.Length < minUsernameLength)
                message = UITextKeys.UI_ERROR_USERNAME_TOO_SHORT;
            else if (username.Length > maxUsernameLength)
                message = UITextKeys.UI_ERROR_USERNAME_TOO_LONG;
            else if (string.IsNullOrEmpty(password) || password.Length < minPasswordLength)
            {
                message = UITextKeys.UI_ERROR_PASSWORD_TOO_SHORT;
            }
            else
            {
                await DatabaseClient.UpdateUserLoginAsync(new CreateUserLoginReq()
                {
                    Username = username,
                    Password = password,
                    Email = email,
                });
            }
            // Response
            result.Invoke(
                message == UITextKeys.NONE ? AckResponseCode.Success : AckResponseCode.Error,
                new ResponseUserRegisterMessage()
                {
                    message = message,
                });
#endif
        }
    }

}
