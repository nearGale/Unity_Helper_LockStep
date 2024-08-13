using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utils.Extensions;

namespace Mirror.EX_A
{
    /// <summary>
    /// �ͻ�����Ϣ�� + ����
    /// </summary>
    public class EX_A_MsgHandler : Singleton<EX_A_MsgHandler>
    {
        public void SetupClient()
        {
            NetworkClient.RegisterHandler<Msg_Join_Rsp>(OnJoinRsp);
            NetworkClient.RegisterHandler<Msg_Join_Ntf>(OnJoinNtf);
        }

        private void OnJoinRsp(Msg_Join_Rsp msg)
        {
            EX_A_TextLog.instance.AppendLog($"Client: OnPlayerJoinRsp:{msg.playerId}");
        }

        private void OnJoinNtf(Msg_Join_Ntf msg)
        {
            EX_A_TextLog.instance.AppendLog($"Client: OnPlayerJoinNtf:{msg.playerIds.GetString()}");
        }
    }
}