using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Utils.Extensions;

namespace Mirror.EX_A
{
    /// <summary>
    /// 服务端消息处理
    /// </summary>
    public class ServerMessageSystem : Singleton<ServerMessageSystem>, IServerSystem
    {
        #region system func

        public void OnStartServer()
        {
            RegisterMessageHandler();
        }

        public void OnStopServer()
        {
            UnRegisterMessageHandler();
        }

        public void Start() { }

        public void Update() { }

        public void LogicUpdate() { }

        #endregion

        /// <summary>
        /// 注册消息回调
        /// </summary>
        public void RegisterMessageHandler()
        {
            NetworkServer.RegisterHandler<Msg_PingPong_Req>(OnPingPongReq);

            NetworkServer.RegisterHandler<Msg_PlayerIdentify_Req>(OnPlayerIdentifyReq);
            NetworkServer.RegisterHandler<Msg_BattleStart_Req>(OnBattleStartReq);
            NetworkServer.RegisterHandler<Msg_BattleStop_Req>(OnBattleStopReq);

            NetworkServer.RegisterHandler<Msg_BattlePause_Req>(OnBattlePauseReq);
            NetworkServer.RegisterHandler<Msg_BattleResume_Req>(OnBattleResumeReq);
            NetworkServer.RegisterHandler<Msg_Command_Req>(OnCommandReq);
        }

        public void UnRegisterMessageHandler()
        {
            NetworkServer.UnregisterHandler<Msg_PlayerIdentify_Req>();
            NetworkServer.UnregisterHandler<Msg_BattleStart_Req>();
            NetworkServer.UnregisterHandler<Msg_BattleStop_Req>();

            NetworkServer.UnregisterHandler<Msg_BattlePause_Req>();
            NetworkServer.UnregisterHandler<Msg_BattleResume_Req>();
            NetworkServer.UnregisterHandler<Msg_Command_Req>();
        }

        #region 消息回调

        private void OnPingPongReq(NetworkConnectionToClient conn, Msg_PingPong_Req msg)
        {
            Msg_PingPong_Rsp msgRsp = new();
            conn.Send(msgRsp);
        }

        private void OnPlayerIdentifyReq(NetworkConnectionToClient conn, Msg_PlayerIdentify_Req msg)
        {
            var (eResult, playerId) = ServerPlayerSystem.Instance.TryAddPlayer(conn, msg.playerName);

            Msg_PlayerIdentify_Rsp msgRsp = new()
            {
                result = eResult,
                playerId = playerId
            };
            conn.Send(msgRsp);

            if (eResult == EIdentifyResult.Failed)
            {
                Msg_ClientWillDisconnect_Rsp msgDisconnect = new();
                conn.Send(msgDisconnect);
                return;
            }

            // 如果在战斗中，向客户端同步所有消息
            if (GameHelper_Server.IsInBattleRoom())
            {
                GameHelper_Server.NotifyBattleStart(conn);
                ServerCommandStorageSystem.Instance.SyncAllCommands(conn);
            }

            var ids = ServerPlayerSystem.Instance.playerId2Info.Keys.ToList();
            Msg_Join_Ntf msgNtf = new Msg_Join_Ntf()
            {
                playerIds = ids
            };

            NetworkServer.SendToAll(msgNtf);
            GameHelper_Common.UILog($"Server: DoPlayerJoinNtf:{ids.GetString()}");
        }

        private void OnBattleStartReq(NetworkConnectionToClient conn, Msg_BattleStart_Req msg)
        {
            if (ServerLogicSystem.Instance.eRoomState == EServerRoomState.InBattle)
                return;

            ServerTimerSystem.Instance.StartBattle();
            ServerCommandStorageSystem.Instance.StartBattle();
            ServerCommandSyncSystem.Instance.StartBattle();
            ServerLogicSystem.Instance.StartBattle();

            GameHelper_Server.NotifyBattleStart();
        }

        private void OnBattleStopReq(NetworkConnectionToClient conn, Msg_BattleStop_Req msg)
        {
            ServerTimerSystem.Instance.StopBattle();
            ServerCommandStorageSystem.Instance.StopBattle();
            ServerCommandSyncSystem.Instance.StopBattle();
            ServerLogicSystem.Instance.StopBattle();

            GameHelper_Server.NotifyBattleStop();
        }

        private void OnBattlePauseReq(NetworkConnectionToClient conn, Msg_BattlePause_Req msg)
        {
            ServerTimerSystem.Instance.battlePause = true;
            Msg_BattlePause_Rsp msgRsp = new()
            {
                isPause = true
            };
            NetworkServer.SendToAll(msgRsp);
        }

        private void OnBattleResumeReq(NetworkConnectionToClient conn, Msg_BattleResume_Req msg)
        {
            ServerTimerSystem.Instance.battlePause = false;

            Msg_BattlePause_Rsp msgRsp = new()
            {
                isPause = false
            };
            NetworkServer.SendToAll(msgRsp);
        }

        private void OnCommandReq(NetworkConnectionToClient conn, Msg_Command_Req msg)
        {
            var playerId = ServerPlayerSystem.Instance.GetPlayerIdByConnectionId(conn.identity.netId);
            if (playerId == 0) return;

            ServerCommandSyncSystem.Instance.CacheClientCommand(playerId, msg.eCommand);
        }

        #endregion


    }
}