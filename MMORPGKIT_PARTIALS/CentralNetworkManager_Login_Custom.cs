using Cysharp.Threading.Tasks;
using LiteNetLib.Utils;
using LiteNetLibManager;
using System.Text.RegularExpressions;

namespace MultiplayerARPG.MMO
{
    public partial class CentralNetworkManager
    {
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
            FindUsernameResp findUsernameResp = await DbServiceClient.FindUsernameAsync(new FindUsernameReq()
            {
                Username = username
            });
            if (findUsernameResp.FoundAmount < 1)
                message = UITextKeys.UI_ERROR_INVALID_USERNAME_OR_PASSWORD;
            else if (string.IsNullOrEmpty(username) || username.Length < minUsernameLength)
                message = UITextKeys.UI_ERROR_USERNAME_TOO_SHORT;
            else if (username.Length > maxUsernameLength)
                message = UITextKeys.UI_ERROR_USERNAME_TOO_LONG;
            else if (string.IsNullOrEmpty(password) || password.Length < minPasswordLength)
                message = UITextKeys.UI_ERROR_PASSWORD_TOO_SHORT;
            else
            {
                await DbServiceClient.UpdateUserLoginAsync(new CreateUserLoginReq()
                {
                    Username = username,
                    Password = password
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