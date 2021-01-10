﻿using UnityEngine;

namespace DSD.KernalTool
{
    public class LoadPrefabs : MonoBehaviour
    {
        private static LoadPrefabs m_Instance;           // 脚本实例

        /// <summary>
        /// 类实例
        /// </summary>
        /// <returns></returns>
        public static LoadPrefabs GetInstance()
        {
            if (m_Instance == null)
            {
                m_Instance = new LoadPrefabs();
            }
            return m_Instance;
        }

        /// <summary>
        /// 加载预制体
        /// </summary>
        /// <param name="prefabsPathAndName"></param>
        /// <returns></returns>
        public GameObject GetLoadPrefab(string prefabsPathAndName)
        {
            // 把资源加载到内存中
            Object go = Resources.Load("Prefabs/" + prefabsPathAndName, typeof(GameObject));
            // 用加载得到的资源对象，实例化游戏对象，实现游戏物体的动态加载
            GameObject loadPrefab = Instantiate(go) as GameObject;

            return loadPrefab;
        }
    }
}