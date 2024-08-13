using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.EX_A
{
    public struct Msg_Join_Ntf : NetworkMessage
    {
        public List<uint> playerIds;
    }

    public struct Msg_Start_Req : NetworkMessage
    {

    }

    public struct Msg_Join_Rsp : NetworkMessage
    {
        public uint playerId;
    }

    public struct Msg_Command_Req : NetworkMessage
    {
        public ECommand eCommand;
    }

    public enum ECommand
    {
        multi,
    }

    public struct CommandDetail
    {
        public uint playerId;
        public ECommand eCommand;
    }

    /// <summary>
    /// ��֡��ָ���
    /// </summary>
    public struct FrameCommands
    {
        /// <summary> ֡�� </summary>
        public ulong serverTick;
        /// <summary> ��֡���ָ��� </summary>
        public List<CommandDetail> details;
    }

    public struct Msg_Command_Ntf : NetworkMessage
    {
        public ulong curServerTick;
        /// <summary> ӵ�ж�֡��ָ��� </summary>
        public List<FrameCommands> oneFrameCommands;
    }
}