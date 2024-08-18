using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Mirror.EX_A
{
    /// <summary>
    /// 此处放置 客户端 运行逻辑
    /// </summary>
    public class ClientTimerSystem : Singleton<ClientTimerSystem>, IClientSystem
    {
        /// <summary> 服务器大厅帧号 </summary>
        public ulong gameServerTick;

        /// <summary> 服务器战斗房间内的帧号（最新的） </summary>
        public ulong battleServerTick;

        /// <summary> 客户端当前帧号（战斗房间内的） </summary>
        public ulong clientTick;

        /// <summary> 上一帧逻辑帧的时间 </summary>
        private float _lastTickTime;

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
            if (clientTick >= battleServerTick) return;

            ClientTickGrow(false);
        }

        /// <summary>
        /// 追帧时使用的逻辑，无视帧间隔检查
        /// </summary>
        public void LogicUpdate_FrameChasing()
        {
            ClientTickGrow(true);
        }
        #endregion

        private void ClearData()
        {
            gameServerTick = 0;
            battleServerTick = 0;
            clientTick = 0;
        }

        /// <summary>
        /// 战斗房间开始时
        /// </summary>
        public void BattleStart()
        {
            ClearData();

            _lastTickTime = Time.time;
        }

        /// <summary>
        /// 战斗房间结束时
        /// </summary>
        public void BattleStop()
        {
            ClearData();
        }

        public void ClientTickGrow(bool ignoreIntervalCheck)
        {
            var intervalPass = Time.time - _lastTickTime > ConstVariables.LogicFrameIntervalSeconds;
            // TODO: intervalPass 由 (现在-开始) / 总帧数，判断整体的时间是否是可以进入下一帧（浮动计算）
            if (ignoreIntervalCheck || intervalPass)
            {
                clientTick++;
                _lastTickTime = Time.time;
            }
        }
    }
}