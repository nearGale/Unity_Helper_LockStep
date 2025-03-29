using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.EX_A
{
    public class EX_A_TextLog : MonoSingleton<EX_A_TextLog>
    {
        public Text text;

        private void Start()
        {
            text = GetComponent<Text>();
        }

        public void AppendLog(string log)
        {
            text.text += log + "\n";
        }
    }
}