using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Extensions;

namespace Mirror.EX_A
{
    /// <summary>
    /// 服务端 存放全部指令消息
    /// </summary>
    public class ServerCommandStorageSystem : Singleton<ServerCommandStorageSystem>, IServerSystem
    {
        /// <summary>
        /// 缓存了从 第1帧 开始的指令，所有的指令
        /// 
        /// 字典：帧号 -> 这一帧的指令集合
        /// kvPair.value : 已池化，清空使用：ClearCommandsAndRecycle()
        /// </summary>
        private Dictionary<ulong, List<CommandDetail>> _allCommands = new();

        #region system func
        public void OnStartServer()
        {
            ClearData();
        }

        public void OnStopServer()
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
        }

        #endregion

        private void ClearData()
        {
            ClearCommandsAndRecycle();
        }

        private void ClearCommandsAndRecycle()
        {
            foreach (var kvPair in _allCommands)
            {
                kvPair.Value.RecycleListToPool();
            }
            _allCommands.Clear();
        }

        /// <summary>
        /// 战斗房间开始时
        /// </summary>
        public void StartBattle()
        {
            ClearData();
        }

        /// <summary>
        /// 战斗房间结束时
        /// </summary>
        public void StopBattle()
        {
            ClearData();
        }

        /// <summary>
        /// 将一帧的指令集合长期存储，用于后续断线重连，全量重发
        /// </summary>
        /// <param name="frame">帧号</param>
        /// <param name="commands">这一帧的指令集合</param>
        public void StoreCommand(ulong frame, List<CommandDetail> commands)
        {
            List<CommandDetail> cmd = ObjectPool.Instance.Get<List<CommandDetail>>();
            cmd.Clear();

            cmd.AddRange(commands);
            _allCommands.Add(frame, cmd);
        }

        /// <summary>
        /// 将从战斗开始的所有的指令同步给客户端
        /// TODO: 大的话考虑分包
        /// </summary>
        public void SyncAllCommands(NetworkConnectionToClient conn)
        {
            var battleServerTick = GameHelper_Server.GetBattleServerTick();
            List<oneFrameCommands> lst = ObjectPool.Instance.Get<List<oneFrameCommands>>();
            lst.Clear();

            List<ulong> frames = ObjectPool.Instance.Get<List<ulong>>(); // 缓存同步了哪些帧
            frames.Clear();

            foreach (var kvPair in _allCommands)
            {
                // key: 帧号 (ulong)
                // value: 这一帧的指令集合 (List<CommandDetail>)

                frames.Add(kvPair.Key);

                // 塞到协议中
                List<CommandDetail> commandDetails = new(kvPair.Value); // CommandDetail 是 struct 值类型
                oneFrameCommands cmd = new()
                {
                    serverTick = kvPair.Key,
                    details = commandDetails
                };
                lst.Add(cmd);
            }

            frames.Sort();


            var syncFrameMax = frames.Count > 0 ? frames[frames.Count - 1] : 0;

            Msg_CommandAll_Rsp msg = new Msg_CommandAll_Rsp()
            {
                syncedBattleServerTick = syncFrameMax,
                commandsSet = lst,
            };
            conn.Send(msg);

            if (lst.Count > 0)
            {
                GameHelper_Common.UILog($"Server: SyncCommands_All at {battleServerTick} frames:0->{syncFrameMax} {frames.GetString()}");
            }

            lst.RecycleListToPool();
            frames.RecycleListToPool();
        }
    }
}