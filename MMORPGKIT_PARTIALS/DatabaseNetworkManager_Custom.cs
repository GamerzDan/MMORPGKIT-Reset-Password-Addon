using System.Linq;
using UnityEngine;
using LiteNetLibManager;
using ConcurrentCollections;
using System.Collections.Concurrent;
using System;
using System.Collections.Generic;

namespace MultiplayerARPG.MMO
{
    public partial class DatabaseNetworkManager : LiteNetLibManager.LiteNetLibManager
    {
        [DevExtMethods("RegisterMessages")]
        protected void DevExtRegisterMessages()
        {
            RegisterRequestToServer<CreateUserLoginReq, EmptyMessage>(DatabaseRequestTypes.RequestCreateUserLogin, CreateUserLogin);
        }
    }
}
