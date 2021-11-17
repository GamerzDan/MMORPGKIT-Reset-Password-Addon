using System.Net;
using System.Net.Security;
using UnityEngine;
using LiteNetLib;
using LiteNetLibManager;
using LiteNetLib.Utils;
using Cysharp.Threading.Tasks;

namespace MultiplayerARPG.MMO
{
    public partial class MMOClientInstance : MonoBehaviour
    {
        public void RequestPasswordReset(string username, string password, ResponseDelegate<ResponseUserRegisterMessage> callback)
        {
            centralNetworkManager.RequestPasswordReset(username, password, callback);
        }
    }
}