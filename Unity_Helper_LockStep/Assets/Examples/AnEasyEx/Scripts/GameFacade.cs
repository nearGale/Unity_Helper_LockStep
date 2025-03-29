using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.EX_A
{
    public static class GameFacade
    {
        public static bool isServer = false;

        public static DateTime startTime;

        /// <summary>
        /// 这里放了所有的 system，一帧中运行按照这里的顺序执行
        /// </summary>
        public static List<IServerSystem> serverSystems = new()
        {
            // 服务器端系统
            // 执行
            //     Start()
            //     Update()
            //     LogicUpdate()
            // 服务器端专属
            //     OnStartServer()
            //     OnStopServer()
            
            ServerTimerSystem.Instance,
            ServerPlayerSystem.Instance,
            ServerMessageSystem.Instance,
            ServerLogicSystem.Instance,
            ServerCommandStorageSystem.Instance,
            ServerCommandSyncSystem.Instance,
        };

        public static List<IClientSystem> clientSystems = new()
        {
            // 客户端系统
            // 执行
            //     Start()
            //     Update()
            //     LogicUpdate()

            ClientMessageSystem.Instance,
            ClientRoomSystem.Instance,
            
            // ==================================
            // 战斗中系统，加速追帧就跑这几个系统
            // 追帧：GameHelper_Client.ChasingOneFrame()
            ClientFrameSyncSystem.Instance, // 执行指令
            ClientLogicSystem.Instance, // 逻辑更新
            ClientTimerSystem.Instance, // 帧数更新
            // ==================================

            ClientChasingFrameSystem.Instance,
        };

        /// <summary>
        /// 是否启用指令执行快照（记录指令执行前、后，整个战斗场景的状态）
        /// 记录到的文件路径：PersistentDataPath/commandSnapshotLogName
        /// </summary>
        public static bool enableCommandSnapshot = true;

        /// <summary>
        /// 指令快照文件，存储路径
        /// 基于 PersistentDataPath
        /// </summary>
        public static string commandSnapshotLogName = "commandSnapshot";

        /// <summary>
        /// 报错内容文件，存储路径
        /// </summary>
        public static string exceptionLogName = "exception";

        /// <summary>
        /// log文件，存储路径
        /// </summary>
        public static string normalLogName = "normal";
    }
}