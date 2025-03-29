using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Mirror.EX_A
{
    /// <summary>
    /// 此处放置 客户端 运行逻辑
    /// </summary>
    public class ClientChasingFrameSystem : Singleton<ClientChasingFrameSystem>, IClientSystem
    {
        #region system func
        public void OnClientConnect()
        {
        }

        public void OnClientDisconnect()
        {
        }

        public void Start()
        {
        }

        public void Update()
        {
        }

        public void LogicUpdate()
        {
            var battleServerTick = GameHelper_Client.GetBattleServerTick();
            var clientTick = GameHelper_Client.GetClientTick();

            if (battleServerTick - clientTick > ConstVariables.CommandSetSyncIntervalFrames)
            {
                // 如果相差超过【一次同步的帧间隔数量】
                //   => 客户端加速追帧
                for (int i = 0; i < ConstVariables.CommandChasingPerFrame; i++)
                {
                    GameHelper_Client.ChasingOneFrame();

                    clientTick = GameHelper_Client.GetClientTick();
                    if(battleServerTick - clientTick <= ConstVariables.CommandSetSyncIntervalFrames)
                    {
                        break;
                    }
                }
            }

        }
        #endregion


    }
}