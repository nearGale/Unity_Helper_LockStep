using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.EX_A
{
    /// <summary>
     /// 心跳包请求
     /// </summary>
    public struct Msg_PingPong_Req : NetworkMessage
    {
    }

    /// <summary>
    /// 心跳包回包
    /// </summary>
    public struct Msg_PingPong_Rsp : NetworkMessage
    {
    }

    /// <summary>
    /// 客户端连接到服务器后，给这个人下发回包
    /// 让客户端进行 ID 校验流程
    /// </summary>
    public struct Msg_PlayerConnect_Rsp : NetworkMessage
    {
    }

    /// <summary>
    /// 客户端申请 ID 校验
    /// </summary>
    public struct Msg_PlayerIdentify_Req : NetworkMessage
    {
        public string playerName; // 申请登录的playerName TODO: 后续改为uuid之类的
    }

    /// <summary>
    /// ID 校验结果
    /// </summary>
    public enum EIdentifyResult
    {
        Succeed,
        Failed,
        Replace, // 验证通过，有之前登录过的账号，把它顶掉了
    }

    /// <summary>
    /// 加入房间回包，给加入的人，告诉他自己的playerId是多少
    /// </summary>
    public struct Msg_PlayerIdentify_Rsp : NetworkMessage
    {
        public EIdentifyResult result;
        public uint playerId;
    }

    /// <summary>
    /// 告诉客户端即将断开连接
    /// </summary>
    public struct Msg_ClientWillDisconnect_Rsp : NetworkMessage
    {

    }

    /// <summary>
    /// 有玩家加入时，广播给所有人，告诉他们当前房间内有哪些人
    /// </summary>
    public struct Msg_Join_Ntf : NetworkMessage
    {
        public List<uint> playerIds;
    }

    /// <summary>
    /// 战斗开始请求
    /// </summary>
    public struct Msg_BattleStart_Req : NetworkMessage 
    {

    }

    /// <summary>
    /// 战斗开始回包
    /// </summary>
    public struct Msg_BattleStart_Ntf : NetworkMessage
    {
        public int randomSeed;
    }

    /// <summary>
    /// 战斗结束请求
    /// </summary>
    public struct Msg_BattleStop_Req : NetworkMessage 
    {

    }

    /// <summary>
    /// 战斗结束回包
    /// </summary>
    public struct Msg_BattleStop_Ntf : NetworkMessage
    {
    }

    /// <summary>
    /// 战斗服务器暂停始请求
    /// </summary>
    public struct Msg_BattlePause_Req : NetworkMessage
    {

    }

    /// <summary>
    /// 战斗服务器恢复请求
    /// </summary>
    public struct Msg_BattleResume_Req : NetworkMessage
    {

    }

    /// <summary>
    /// 同步战斗暂停状态
    /// </summary>
    public struct Msg_BattlePause_Rsp : NetworkMessage
    {
        public bool isPause; // 当前状态是否是暂停
    }

    /// <summary>
    /// 局内指令请求
    /// </summary>
    public struct Msg_Command_Req : NetworkMessage
    {
        public ECommand eCommand;
    }

    public enum ECommand
    {
        Modify,
    }

    public struct CommandDetail
    {
        public uint playerId;
        public ECommand eCommand;
    }

    /// <summary>
    /// 单帧的指令集合
    /// </summary>
    public struct oneFrameCommands
    {
        /// <summary> 帧号 </summary>
        public ulong serverTick;

        /// <summary> 这帧里的指令集合 </summary>
        public List<CommandDetail> details;
    }

    /// <summary>
    /// 多帧指令集合下发（正常同步）
    /// </summary>
    public struct Msg_Command_Ntf : NetworkMessage
    {
        public ulong curBattleServerTick;
        /// <summary> 拥有多帧的指令集合 </summary>
        public List<oneFrameCommands> commandsSet;
    }

    /// <summary>
    /// 多帧指令集合下发（全量同步，用于断线重连）
    /// </summary>
    public struct Msg_CommandAll_Rsp : NetworkMessage
    {
        /// <summary> 同步包的最大帧，非服务器最大帧 </summary>
        public ulong syncedBattleServerTick;

        /// <summary> 拥有多帧的指令集合 </summary>
        public List<oneFrameCommands> commandsSet;
    }

}