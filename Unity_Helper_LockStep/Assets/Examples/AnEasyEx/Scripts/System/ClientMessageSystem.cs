using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utils.Extensions;

namespace Mirror.EX_A
{
    /// <summary>
    /// 客户端  消息绑定 + 处理
    /// </summary>
    public class ClientMessageSystem : Singleton<ClientMessageSystem>, IClientSystem
    {
        #region system func
        public void OnClientConnect()
        {
            RegisterMessageHandler();
        }

        public void OnClientDisconnect()
        {
            UnRegisterMessageHandler();
        }

        public void Start()
        {
        }

        public void Update()
        {
        }

        public void LogicUpdate()
        {
        }

        #endregion

        public void RegisterMessageHandler()
        {
            NetworkClient.RegisterHandler<Msg_PingPong_Rsp>(OnPingPongRsp);

            NetworkClient.RegisterHandler<Msg_PlayerConnect_Rsp>(OnPlayerConnectRsp);
            NetworkClient.RegisterHandler<Msg_PlayerIdentify_Rsp>(OnPlayerIdentifyRsp);
            NetworkClient.RegisterHandler<Msg_ClientWillDisconnect_Rsp>(OnClientWillDisconnectRsp);

            NetworkClient.RegisterHandler<Msg_Join_Ntf>(OnJoinNtf);
            NetworkClient.RegisterHandler<Msg_BattleStart_Ntf>(OnBattleStart);
            NetworkClient.RegisterHandler<Msg_BattleStop_Ntf>(OnBattleStop);
            NetworkClient.RegisterHandler<Msg_BattlePause_Rsp>(OnBattlePause);

            NetworkClient.RegisterHandler<Msg_Command_Ntf>(OnCommandNtf);
            NetworkClient.RegisterHandler<Msg_CommandAll_Rsp>(OnCommandAllRsp);
        }

        public void UnRegisterMessageHandler()
        {
            NetworkClient.UnregisterHandler<Msg_PlayerConnect_Rsp>();
            NetworkClient.UnregisterHandler<Msg_PlayerIdentify_Rsp>();
            NetworkClient.UnregisterHandler<Msg_ClientWillDisconnect_Rsp>();

            NetworkClient.UnregisterHandler<Msg_Join_Ntf>();
            NetworkClient.UnregisterHandler<Msg_BattleStart_Ntf>();
            NetworkClient.UnregisterHandler<Msg_BattleStop_Ntf>();
            NetworkClient.UnregisterHandler<Msg_BattlePause_Rsp>();

            NetworkClient.UnregisterHandler<Msg_Command_Ntf>();
            NetworkClient.UnregisterHandler<Msg_CommandAll_Rsp>();
        }

        private void OnPingPongRsp(Msg_PingPong_Rsp msg)
        {
            ClientTimerSystem.Instance.OnRttRsp();
        }


        private void OnPlayerConnectRsp(Msg_PlayerConnect_Rsp msg)
        {
            GameHelper_Common.UILog($"Client: OnPlayerConnectRsp");

            var name = GameHelper_Client.GetLocalPlayerName();
            if(!name.IsNullOrEmpty())
            {
                Msg_PlayerIdentify_Req msgIdentify = new()
                {
                    playerName = GameHelper_Client.GetLocalPlayerName(),
                };
                NetworkClient.Send(msgIdentify);
            }
            else
            {
                NetworkClient.Disconnect();
                GameHelper_Common.UILog("ERR!!! Can't Req Identify: no player name!");
            }
        }

        private void OnPlayerIdentifyRsp(Msg_PlayerIdentify_Rsp msg)
        {
            GameHelper_Common.UILog($"Client: OnPlayerIdentifyRsp playerId:{msg.playerId}");

            ClientRoomSystem.Instance.OnPlayerIdentified(msg);
        }

        private void OnClientWillDisconnectRsp(Msg_ClientWillDisconnect_Rsp msg)
        {
            GameHelper_Common.UILog($"Client: WillDisconnect");

            NetworkClient.Disconnect();
        }

        private void OnJoinNtf(Msg_Join_Ntf msg)
        {
            GameHelper_Common.UILog($"Client: OnPlayerJoinNtf:{msg.playerIds.GetString()}");
        }

        private void OnBattleStart(Msg_BattleStart_Ntf msg)
        {
            var randomSeed = msg.randomSeed;

            ClientRandomSystem.Instance.BattleStart(randomSeed); // 设置随机数种子
            ClientRoomSystem.Instance.BattleStart();
            ClientFrameSyncSystem.Instance.BattleStart();
            ClientTimerSystem.Instance.BattleStart();
            ClientLogicSystem.Instance.BattleStart();
        }

        private void OnBattleStop(Msg_BattleStop_Ntf msg)
        {
            // 战斗结束，回到 lobby 状态
            ClientRandomSystem.Instance.BattleStop(); // 清除随机数种子
            ClientRoomSystem.Instance.BattleStop();
            ClientFrameSyncSystem.Instance.BattleStop();
            ClientTimerSystem.Instance.BattleStop();
            ClientLogicSystem.Instance.BattleStop();
        }

        private void OnBattlePause(Msg_BattlePause_Rsp msg)
        {
            ClientRoomSystem.Instance.battlePause = msg.isPause;
        }

        // TODO: battleRoomId

        private void OnCommandNtf(Msg_Command_Ntf msg)
        {
            ClientFrameSyncSystem.Instance.OnSyncCommands(msg);
        }

        private void OnCommandAllRsp(Msg_CommandAll_Rsp msg)
        {
            ClientFrameSyncSystem.Instance.OnSyncCommandsAll(msg);
        }
    }
}