using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.EX_A
{
    public static partial class GameHelper_Server
    {
        /// <summary>
        /// 判断是否正在局内
        /// </summary>
        public static bool IsInBattleRoom()
        {
            return ServerLogicSystem.Instance.eRoomState == EServerRoomState.InBattle;
        }

        /// <summary>
        /// 获取服务器当前帧号
        /// </summary>
        public static ulong GetGameServerTick()
        {
            return ServerTimerSystem.Instance.gameServerTick;
        }

        /// <summary>
        /// 获取服务器 局内当前帧号
        /// </summary>
        public static ulong GetBattleServerTick()
        {
            return ServerTimerSystem.Instance.battleServerTick;
        }

        /// <summary>
        /// 拿到 战斗内随机数种子
        /// </summary>
        /// <returns></returns>
        public static int GetRandomSeed()
        {
            return ServerLogicSystem.Instance.randomSeed;
        }

        /// <summary>
        /// 发消息通知客户端战斗开始
        /// conn 为空时广播
        /// </summary>
        /// <param name="conn">通知的客户端</param>
        public static void NotifyBattleStart(NetworkConnectionToClient conn = null)
        {
            Msg_BattleStart_Ntf msg = new Msg_BattleStart_Ntf()
            {
                randomSeed = GetRandomSeed()
            };

            if (conn != null)
            {
                conn.Send(msg);
            }
            else
            {
                NetworkServer.SendToAll(msg);
            }
        }

        public static void NotifyBattleStop()
        {
            Msg_BattleStop_Ntf msgRsp = new();
            NetworkServer.SendToAll(msgRsp);
        }
    }
}