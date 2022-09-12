#if UNITY_STANDALONE && !CLIENT_BUILD
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

using UnityEngine;

namespace MultiplayerARPG.MMO
{
    public abstract partial class BaseDatabase : MonoBehaviour
    {
#if UNITY_STANDALONE && !CLIENT_BUILD
        public abstract UniTask UpdateUserLogin(string username, string password, string email);
#endif
    }
}
#endif