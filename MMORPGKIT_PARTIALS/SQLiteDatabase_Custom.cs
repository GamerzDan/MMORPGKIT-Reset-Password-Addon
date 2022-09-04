#if UNITY_STANDALONE && !CLIENT_BUILD
using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.IO;
using LiteNetLibManager;
using MiniJSON;
using System;
using Cysharp.Threading.Tasks;
#endif
using UnityEngine;

namespace MultiplayerARPG.MMO
{
    public partial class SQLiteDatabase : BaseDatabase
    {
        public override async UniTask UpdateUserLogin(string username, string password, string email)
        {
            await UniTask.Yield();
            ExecuteNonQuery("UPDATE userlogin SET password=@password WHERE username=@username",
                new SqliteParameter("@username", username),
                new SqliteParameter("@password", password.PasswordHash()));
        }
    }

}