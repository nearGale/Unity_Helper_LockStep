using System.Linq;
using UnityEngine;
using Utils.Extensions;

namespace Mirror.EX_A
{
    public class EX_A_Player : NetworkBehaviour
    {
        #region Server
        /// <summary>
        /// 当 NetworkBehaviour 对象在服务器上处于活动状态时，会调用此函数。
        /// （仅服务器触发）
        /// 这可能是由NetworkServer触发的。Listen（）用于场景中的对象，或通过NetworkServer。Spawn（）用于动态创建的对象
        /// 这将用于“主机”上的对象以及专用服务器上的对象
        /// </summary>
        public override void OnStartServer()
        {
            base.OnStartServer();

            Msg_PlayerConnect_Rsp msgRsp = new();
            netIdentity.connectionToClient.Send(msgRsp);

            GameHelper_Common.UILog($"Server: OnPlayerConnect");

            // 检测到有客户端连接，接下来进入ID验证环节
            // 验证通过：
            //    => 广播给所有人，有新人登录服务器 Msg_Join_Ntf

        }
        #endregion
    }
}