using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mirror.EX_A
{
    public static partial class GameHelper_Client
    {
        /// <summary>
        /// 加速追帧
        /// </summary>
        public static void ChasingOneFrame()
        {
            ClientFrameSyncSystem.Instance.LogicUpdate();
            ClientLogicSystem.Instance.LogicUpdate();
            ClientTimerSystem.Instance.LogicUpdate_FrameChasing();
        }
    }
}