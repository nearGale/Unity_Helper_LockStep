using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mirror.EX_A
{
    public static partial class GameHelper_Client
    {
        public static ulong GetClientTick()
        {
            return ClientTimerSystem.Instance.clientTick;
        }

        public static ulong GetBattleServerTick()
        {
            return ClientTimerSystem.Instance.battleServerTick;
        }

        public static ulong GetGameServerTick()
        {
            return ClientTimerSystem.Instance.gameServerTick;
        }

    }
}