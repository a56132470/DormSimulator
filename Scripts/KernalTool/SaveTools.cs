using System;
using DSD.Framework.Singleton;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace DSD.KernalTool
{
    public class SaveTools : Singleton<SaveTools>
    {
        /// <summary>
        /// 存储状态
        /// </summary>
        /// <param name="keyList"></param>
        /// <param name="valueList"></param>
        /// <param name="saveDic"></param>
        public void SaveDic([NotNull] ref List<string> keyList,ref List<State> valueList, Dictionary<string, State> saveDic)
        {
            if (keyList == null) throw new ArgumentNullException(nameof(keyList));
            keyList = new List<string>();
            valueList = new List<State>();
            foreach (KeyValuePair<string, State> keyValue in saveDic)
            {
                keyList.Add(keyValue.Key);
                valueList.Add(keyValue.Value);
            }
        }
        public void LoadDic(List<string> keyList, List<State> valueList,ref  Dictionary<string, State> saveDic)
        {
            saveDic = new Dictionary<string, State>();
            if(keyList!=null)
            {
                for (var i = 0; i < keyList.Count; i++)
                {
                    saveDic.Add(keyList[i], valueList[i]);
                }
            }

        }
    }

}

