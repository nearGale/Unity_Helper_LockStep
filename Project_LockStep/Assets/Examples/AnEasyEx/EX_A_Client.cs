using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.EX_A
{
    /// <summary>
    /// �˴����� �ͻ��� �����߼�
    /// </summary>
    public class EX_A_Client : Singleton<EX_A_Client>
    {
        public ulong clientTick;
        public ulong serverTick;
        public Dictionary<ulong, FrameCommands> frameCommands = new();

        /// <summary> һ���������������ڼ���֡ͬ���Ľ�� </summary>
        public int val;

        public void Start()
        {
            clientTick = 0;
            frameCommands.Clear();
        }

        public void Update()
        {
        }

        public void LogicUpdate()
        {
            ProcessTick();
        }

        private void ProcessTick()
        {
            if (clientTick > serverTick) return;

            clientTick++;

            if (frameCommands.Remove(clientTick, out var oneFramCommands))
            {
                foreach(var cmd in oneFramCommands.details) 
                {
                    ProcessCommand(cmd);
                }
            }

            if (clientTick % 100 == 1)
            {
                val += 1;
            }
        }

        public void OnSyncCommands(Msg_Command_Ntf msg)
        {
            serverTick = msg.curServerTick;
            foreach(var command in msg.oneFrameCommands) // command ��һ֡��ָ���
            {
                if(!frameCommands.ContainsKey(command.serverTick))
                {
                    frameCommands.Add(command.serverTick, command);
                }
                else
                {
                    Debug.LogError("ERR!!! ���ظ�֡��ָ��");
                }
            }
        }

        private void ProcessCommand(CommandDetail detail)
        {
            val *= 2;
        }
    }
}