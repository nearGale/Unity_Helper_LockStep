using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mirror.EX_A
{
    public static partial class GameHelper_Client
    {
        public static float GetRTT()
        {
            return ClientTimerSystem.Instance.GetRTTValue();
        }
    }
}