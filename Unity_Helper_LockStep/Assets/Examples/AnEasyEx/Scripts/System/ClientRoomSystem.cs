using Edgegap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.EX_A
{
    public enum ERoomState
    {
        UnConnected,
        Lobby,
        InBattle,
    }

    /// <summary>
    /// 此处放置 客户端 运行逻辑
    /// </summary>
    public class ClientRoomSystem : Singleton<ClientRoomSystem>, IClientSystem
    {
        /// <summary> 在服务器中的房间状态 </summary>
        private ERoomState _eRoomState;

        /// <summary> 战斗房间是否暂停 </summary>
        public bool battlePause;

        /// <summary> 在服务器中的 playerId（本次服务器启动时） </summary>
        public uint playerId;

        #region system func
        public void OnClientConnect()
        {
        }

        public void OnClientDisconnect()
        {
            ClearData();
        }

        public void Start()
        {
            _eRoomState = ERoomState.UnConnected;
        }

        public void Update()
        {
        }

        public void LogicUpdate()
        {
        }
        #endregion

        private void ClearData()
        {
            _eRoomState = ERoomState.UnConnected;
            battlePause = false;
            playerId = 0;
        }

        /// <summary>
        /// 服务器 id 验证后
        /// </summary>
        public void OnPlayerIdentified(Msg_PlayerIdentify_Rsp msg)
        {
            if (msg.result == EIdentifyResult.Failed) // failed 会进入断开连接流程，在流程中把 roomState 设为 unConnected
            {
                return;
            }

            playerId = msg.playerId;
            _eRoomState = ERoomState.Lobby;
        }

        /// <summary>
        /// 进入战斗房间时
        /// </summary>
        public void BattleStart()
        {
            _eRoomState = ERoomState.InBattle;

            battlePause = false;

            if (GameFacade.enableCommandSnapshot)
            {
                var logStr = $"[BattleStart] " +
                    $"{GameHelper_Client.GetLocalPlayerName()}";
                GameHelper_Common.FileLog(GameFacade.commandSnapshotLogName, logStr);
            }
        }

        /// <summary>
        /// 战斗房间结束时
        /// </summary>
        public void BattleStop()
        {
            _eRoomState = ERoomState.Lobby;

            battlePause = false;
        }

        /// <summary>
        /// 判断服务器的战斗房间，是不是开始了并且运行中（未暂停）
        /// </summary>
        public bool IsBattleRoomRunning()
        {
            return _eRoomState == ERoomState.InBattle && !battlePause;
        }

        /// <summary>
        /// 拿到服务器连接的状态
        /// </summary>
        /// <returns></returns>
        public ERoomState GetClientInRoomState()
        {
            return _eRoomState;
        }
    }
}