using System.Collections.Generic;
using DSD.KernalTool;
using UnityEngine;

namespace Base.ActionSystem
{
    public class StrengthManager : MonoBehaviour
    {
        public List<Transform> strengthTrans;
        public static StrengthManager Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                return;
        }

        private void Start()
        {
            strengthTrans = Widget.GetChildCollection(transform.Find("Strengths"));
            ChangeStrength(GlobalManager.Instance.player.Strength);
        }

        /// <summary>
        /// 令体力栏根据当前体力关闭和出现
        /// </summary>
        /// <param name="index"></param>
        public void ChangeStrength(int index)
        {
            for (int i = 0; i < strengthTrans.Count; i++)
            {
                strengthTrans[i].gameObject.SetActive(true);
            }
            for (; index < strengthTrans.Count; index++)
            {
                // 5 - 4 - 1 = 0
                // 5 - 3 - 1 = 1
                strengthTrans[strengthTrans.Count - index - 1].gameObject.SetActive(false);
            }
        }
    }
}