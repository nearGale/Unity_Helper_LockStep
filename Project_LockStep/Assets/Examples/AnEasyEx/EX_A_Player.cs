using System.Linq;
using UnityEngine;
using Utils.Extensions;

namespace Mirror.EX_A
{
    public class EX_A_Player : NetworkBehaviour
    {
        #region Server
        /// <summary>
        /// �� NetworkBehaviour �����ڷ������ϴ��ڻ״̬ʱ������ô˺�����
        /// ����������������
        /// ���������NetworkServer�����ġ�Listen�������ڳ����еĶ��󣬻�ͨ��NetworkServer��Spawn�������ڶ�̬�����Ķ���
        /// �⽫���ڡ��������ϵĶ����Լ�ר�÷������ϵĶ���
        /// </summary>
        public override void OnStartServer()
        {
            base.OnStartServer();

            var playerId = EX_A_Server.Instance.AddPlayer(netIdentity);

            Msg_Join_Rsp msgRsp = new Msg_Join_Rsp()
            {
                playerId = playerId
            };
            netIdentity.connectionToClient.Send(msgRsp);

            var ids = EX_A_Server.Instance.playerDict.Keys.ToList();
            Msg_Join_Ntf msg = new Msg_Join_Ntf()
            {
                playerIds = ids
            };

            NetworkServer.SendToAll(msg);
            EX_A_TextLog.instance.AppendLog($"Server: OnPlayerJoinReq:{playerId}");
            EX_A_TextLog.instance.AppendLog($"Server: DoPlayerJoinNtf:{ids.GetString()}");
        }
        #endregion
    }
}