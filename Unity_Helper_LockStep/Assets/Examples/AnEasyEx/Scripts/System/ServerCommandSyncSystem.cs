using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Extensions;

namespace Mirror.EX_A
{
    /// <summary>
    /// 服务端 下发指令消息
    /// </summary>
    public class ServerCommandSyncSystem : Singleton<ServerCommandSyncSystem>, IServerSystem
    {
        /// <summary> 上一次同步的帧号 </summary>
        private ulong _lastSyncedFrame = 0;

        /// <summary>
        /// 在这一帧的持续时间中，收到的指令，在进入下一帧后，缓存下来
        /// 保证和 timerSystem 的时序
        /// </summary>
        private List<CommandDetail> _cachedCommandThisFrame = new();

        /// <summary>
        /// 监听帧号变更
        /// </summary>
        private ulong _lastFrameTick = 0;

        /// <summary>
        /// 缓存了从上次同步到下次同步之间的帧指令
        /// 用完后放到整体指令存储中 -> ServerCommandStorageSystem._allCommands
        /// 
        /// 字典：帧号 -> 这一帧的指令集合
        /// 
        /// kvPair.value 已池化，清空使用：ClearCommandsAndRecycle()
        /// </summary>
        private Dictionary<ulong, List<CommandDetail>> _cachedCommands = new();



        #region system func
        public void OnStartServer()
        {
            ClearData();
        }

        public void OnStopServer()
        {
        }

        public void Start()
        {
        }

        public void Update()
        {
        }
        #endregion

        public void LogicUpdate()
        {
            if (!GameFacade.isServer) return;

            if (!GameHelper_Server.IsInBattleRoom()) return; // 进入房间后同时重置帧号，开始使用帧同步
                                                             // 因为在大厅进入时间不同，不使用帧同步，使用Mirror原生同步即可

            MoveLastFrameCommandsIntoNtfList();

            var battleServerTick = GameHelper_Server.GetBattleServerTick();

            if (battleServerTick - _lastSyncedFrame >= ConstVariables.CommandSetSyncIntervalFrames)
            {
                SyncCommands();
                _lastSyncedFrame = battleServerTick;
            }
        }

        private void ClearData()
        {
            _lastSyncedFrame = 0;
            ClearCommandsAndRecycle();
        }

        private void ClearCommandsAndRecycle()
        {
            foreach(var kvPair in _cachedCommands)
            {
                kvPair.Value.RecycleListToPool();
            }
            _cachedCommands.Clear();
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
        /// 缓存客户端指令，等待时机下发指令集合
        /// </summary>
        public void CacheClientCommand(uint playerId, ECommand eCommand)
        {
            var modifyDetail = new CommandDetail()
            {
                playerId = playerId,
                eCommand = eCommand
            };

            // 放进 cache，在 frameId++ 后，放入指令集合
            // 避免这帧已经发出去后，再堆积当帧指令，造成客户端时序错乱
            _cachedCommandThisFrame.Add(modifyDetail);
        }

        /// <summary>
        /// 把上一帧收到的指令，一起放到下发通知的集合中
        /// </summary>
        private void MoveLastFrameCommandsIntoNtfList()
        {
            var battleServerTick = GameHelper_Server.GetBattleServerTick();

            if (_lastFrameTick == battleServerTick) return; // 进入下一帧后才移入缓存中
            _lastFrameTick = battleServerTick;

            if (_cachedCommandThisFrame.Count == 0) return;

            if (_cachedCommands.ContainsKey(battleServerTick))
            {
                var err = $"ERR!!!! ServerCommandSyncSystem has servertick:{battleServerTick} already!!!";
                GameHelper_Common.UIErr(err);
            }

            List<CommandDetail> lst = ObjectPool.Instance.Get<List<CommandDetail>>();
            lst.Clear();
            lst.AddRange(_cachedCommandThisFrame);

            _cachedCommands.Add(battleServerTick, lst);
            _cachedCommandThisFrame.Clear();

            GameHelper_Common.UILog($"Server: Rcv cmd at {battleServerTick} x{lst.Count}");
        }

        private void SyncCommands()
        {
            var battleServerTick = GameHelper_Server.GetBattleServerTick();
            List<oneFrameCommands> lst = ObjectPool.Instance.Get<List<oneFrameCommands>>();
            
            List<ulong> frames = ObjectPool.Instance.Get<List<ulong>>(); // 缓存同步了哪些帧
            foreach (var kvPair in _cachedCommands)
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

                // 放到整体存储仓库中
                ServerCommandStorageSystem.Instance.StoreCommand(kvPair.Key, kvPair.Value);
            }

            ClearCommandsAndRecycle();

            Msg_Command_Ntf msg = new Msg_Command_Ntf()
            {
                curBattleServerTick = battleServerTick,
                commandsSet = lst,
            };
            NetworkServer.SendToAll(msg);

            if (lst.Count > 0)
            {
                frames.Sort();
            }
            GameHelper_Common.UILog($"Server: SyncCommands at {battleServerTick} frames:[{frames.GetString()}]");

            lst.RecycleListToPool();
            frames.RecycleListToPool();
        }
    }
}