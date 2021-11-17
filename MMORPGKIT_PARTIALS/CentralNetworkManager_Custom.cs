using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLibManager;

namespace MultiplayerARPG.MMO
{
   public partial class CentralNetworkManager : LiteNetLibManager.LiteNetLibManager
    {
        [DevExtMethods("RegisterMessages")]
        protected void DevExtRegisterMessages()
        {
            RegisterRequestToServer<RequestUserRegisterMessage, ResponseUserRegisterMessage>(MMORequestTypes.RequestPasswordReset, HandleRequestPasswordReset);
        }
    }

}