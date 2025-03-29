using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.EX_A
{
    /// <summary>
    /// 此处放置 客户端 同步帧数据，当帧模拟
    /// </summary>
    public class ClientFrameSyncSystem : Singleton<ClientFrameSyncSystem>, IClientSystem
    {
        /// <summary>
        /// 字典：帧 -> 这一帧的指令集合
        /// </summary>
        public Dictionary<ulong, oneFrameCommands> frameCommandDict = new();

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
        }

        public void Update()
        {
        }

        public void LogicUpdate()
        {
            ProcessTick();
        }
        #endregion

        private void ClearData()
        {
            frameCommandDict.Clear();
        }

        /// <summary>
        /// 战斗房间开始时
        /// </summary>
        public void BattleStart()
        {
            ClearData();
        }

        /// <summary>
        /// 战斗房间结束时
        /// </summary>
        public void BattleStop()
        {
            ClearData();
        }

        private void ProcessTick()
        {
            var clientTick = GameHelper_Client.GetClientTick();
            var battleServerTick = GameHelper_Client.GetBattleServerTick();

            if (frameCommandDict.Remove(clientTick, out var oneFramCommands))
            {
                foreach (var cmd in oneFramCommands.details)
                {
                    ProcessCommand(clientTick, cmd);
                }
            }
        }

        public void OnSyncCommands(Msg_Command_Ntf msg)
        {
            ClientTimerSystem.Instance.battleServerTick = msg.curBattleServerTick;
            DoSyncCommand(msg.curBattleServerTick, msg.commandsSet);

            if (GameFacade.enableCommandSnapshot)
            {
                GameHelper_Common.FileLog(GameFacade.commandSnapshotLogName, $"OnSyncCommands serverTick:{msg.curBattleServerTick}");
            }
        }

        public void OnSyncCommandsAll(Msg_CommandAll_Rsp msg)
        {

            ClientTimerSystem.Instance.battleServerTick = msg.syncedBattleServerTick;
            DoSyncCommand(msg.syncedBattleServerTick, msg.commandsSet);

            if (GameFacade.enableCommandSnapshot)
            {
                GameHelper_Common.FileLog(GameFacade.commandSnapshotLogName, $"OnSyncCommandsAll serverTick:{msg.syncedBattleServerTick}");
            }
        }

        /// <summary>
        /// 将同步指令存下
        /// </summary>
        /// <param name="curBattleServerTick">同步到的战斗服务器最大帧</param>
        /// <param name="commandsSet">指令集合</param>
        private void DoSyncCommand(ulong curBattleServerTick, List<oneFrameCommands> commandsSet)
        {
            //GameHelper_Common.UILog($"Client: Rcv: {clientTick} {Time.time}");

            var clientTick = GameHelper_Client.GetClientTick();

            foreach (var command in commandsSet) // command 是一帧的指令集合
            {
                GameHelper_Common.UILog($"Client: Rcv: {command.serverTick} at {clientTick}/{GameHelper_Client.GetBattleServerTick()}");
                if (!frameCommandDict.ContainsKey(command.serverTick))
                {
                    frameCommandDict.Add(command.serverTick, command);
                }
                else
                {
                    Debug.LogError("ERR!!! 有重复帧的指令");
                }
            }
        }

        private void ProcessCommand(ulong tick, CommandDetail detail)
        {
            ClientLogicSystem.Instance.ProcessCommand(detail);
            GameHelper_Common.UILog($"Client: ProcessCommand:{detail.playerId} {detail.eCommand} at tick:{tick}");
        }
    }
}