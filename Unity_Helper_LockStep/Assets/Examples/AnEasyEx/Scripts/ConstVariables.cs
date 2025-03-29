using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.EX_A
{
    public static class ConstVariables 
    {
        /// <summary> 服务器下发指令集合，相隔多少帧同步一下 </summary>
        public const ulong CommandSetSyncIntervalFrames = 5;

        /// <summary>
        /// 逻辑帧间隔(秒）
        /// </summary>
        public const float LogicFrameIntervalSeconds = 1 / 15f;
        //public const float LogicFrameIntervalSeconds = 1;

        /// <summary>
        /// 追帧时，每个逻辑帧，追多少帧
        /// </summary>
        public const int CommandChasingPerFrame = 100;
    }
}