using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.EX_A
{
    /// <summary>
    /// 此处放置 客户端 随机数逻辑
    /// </summary>
    public class ClientRandomSystem : Singleton<ClientRandomSystem>, ISystem
    {
        /// <summary> 随机数种子 </summary>
        public int randomSeed;

        public void Start()
        {
        }

        public void Update()
        {
        }

        public void LogicUpdate()
        {
        }

        /// <summary>
        /// 进入战斗房间时
        /// </summary>
        /// <param name="seed">随机数种子</param>
        public void BattleStart(int seed)
        {
            randomSeed = seed;
            Random.InitState(seed);
            GameHelper_Common.UILog($"RandomSeed:{seed}");
        }

        /// <summary>
        /// 战斗房间结束时
        /// </summary>
        public void BattleStop()
        {
            randomSeed = 0;
        }

        public int GetRandomInt(int min, int max)
        {
            var randomInt = Random.Range(min, max);
            GameHelper_Common.UILog($"RandomInt:{randomInt}");

            return randomInt;
        }
    }
}