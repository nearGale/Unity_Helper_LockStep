using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Mirror.EX_A
{
    /// <summary>
    /// 此处放置 客户端 运行逻辑
    /// </summary>
    public class ClientLogicSystem : Singleton<ClientLogicSystem>, IClientSystem
    {
        /// <summary> 一个测试用例，用于计算帧同步的结果 </summary>
        public Int64 val;

        /// <summary> 
        /// 上一次执行时的客户端逻辑帧号，变大了才执行下一次
        /// </summary>
        private ulong _lastProcessClientTick;

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
            var clientTick = GameHelper_Client.GetClientTick();
            var serverTick = GameHelper_Client.GetBattleServerTick();

            if (!ClientRoomSystem.Instance.IsBattleRoomRunning() && clientTick > serverTick) return; // 小于等于 serverTick 可以继续模拟到服务器帧号

            if (clientTick - _lastProcessClientTick > 1)
            {
                var err = $"ERR!!!! ClientLogicSystem [clientTick - lastProcessTick > 1] {clientTick} {_lastProcessClientTick}";
                GameHelper_Common.UIErr(err);
            }

            if (clientTick > _lastProcessClientTick)
            {
                val += 1;
                _lastProcessClientTick = clientTick;

                SnapshotBattleState();
            }
        }
        #endregion

        private void ClearData()
        {
            val = 0;
            _lastProcessClientTick = 0;
        }

        /// <summary>
        /// 战斗房间开始时，重置所有的游戏数据
        /// </summary>
        public void BattleStart()
        {
            ClearData();
        }

        /// <summary>
        /// 战斗房间结束时，重置所有的游戏数据
        /// </summary>
        public void BattleStop()
        {
            ClearData();
        }

        public void ProcessCommand(CommandDetail detail)
        {
            SnapshotAroundCommand(detail, true);
            
            switch (detail.eCommand)
            {
                case ECommand.Modify:
                    var mode = ClientRandomSystem.Instance.GetRandomInt(0, 4);
                    var param = ClientRandomSystem.Instance.GetRandomInt(2, 15);
                    switch (mode)
                    {
                        case 0:
                        case 1:
                        case 2:
                            // Multiply
                            val *= param;
                            break;
                        case 3:
                            // Divide
                            val = (int)((float)val / param);
                            break;
                    }
                    break;
            }

            SnapshotAroundCommand(detail, false);
        }


        #region snapshot 逻辑快照

        /// <summary>
        /// 对指令执行前/后，记录战斗状态快照
        /// </summary>
        /// <param name="detail">执行的指令</param>
        /// <param name="isBefore">在指令之前 / 之后</param>
        private void SnapshotAroundCommand(CommandDetail detail, bool isBefore)
        {
            if (!GameFacade.enableCommandSnapshot) return;

            var preffix = $"player:{detail.playerId} " +
                $"command:{detail.eCommand} " +
                $"before:{isBefore} ";

            SnapshotBattleState(preffix);
        }


        /// <summary>
        /// 给当前战斗状态进行快照，存到指定文件中
        /// </summary>
        /// <param name="preffix">记录log的前缀</param>
        private void SnapshotBattleState(string preffix = null)
        {
            if (!GameFacade.enableCommandSnapshot) return;

            var snapshot = GetSnapshot();
            var clientTick = GameHelper_Client.GetClientTick();

            var logStr = $"t:{clientTick} " +
                $"{preffix} " +
                $"\n====snapshot====\n{snapshot}\n==============\n " +
                $"\n";
            GameHelper_Common.FileLog(GameFacade.commandSnapshotLogName, logStr);

        }

        /// <summary>
        /// 获取战斗状态快照（string）
        /// </summary>
        /// <returns>战斗状态快照</returns>
        private string GetSnapshot()
        {
            var snapshot = $"{val}";
            return snapshot;
        }

        #endregion
    }
}