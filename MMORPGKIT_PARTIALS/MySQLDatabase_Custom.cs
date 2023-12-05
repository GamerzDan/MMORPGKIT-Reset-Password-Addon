#if UNITY_STANDALONE && !CLIENT_BUILD
using MySqlConnector;
using System.Collections.Generic;
using LiteNetLibManager;
using System;
using System.IO;
using Cysharp.Threading.Tasks;

using UnityEngine;

namespace MultiplayerARPG.MMO
{
    public partial class MySQLDatabase : BaseDatabase
    {
        public override async UniTask UpdateUserLogin(string username, string password, string email)
        {
            await ExecuteNonQuery("UPDATE userlogin SET password=@password WHERE username=@username",
                new MySqlParameter("@username", username),
                new MySqlParameter("@password", password.PasswordHash()));
        }
    }

}
#endif