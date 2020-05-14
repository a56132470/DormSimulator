using System.Collections.Generic;
using UnityEngine;

namespace DSD.KernalTool
{
    public static partial class Widget
    {
        /// <summary>
        /// 获取子对象变换集合
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<Transform> GetChildCollection(this Transform obj)
        {
            List<Transform> list = new List<Transform>();
            for (int i = 0; i < obj.childCount; i++)
            {
                list.Add(obj.GetChild(i));
            }
            return list;
        }

        /// <summary>
        /// 获取子对象集合
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<GameObject> GetChildCollection(this GameObject obj)
        {
            var list = obj.transform.GetChildCollection();
            return list.ConvertAll(T => T.gameObject);
        }

        /// <summary>
        /// 根据标签获取字物体的变换
        /// </summary>
        /// <param name="obj">当前物体变换</param>
        /// <param name="tag">标签</param>
        /// <returns></returns>
        public static List<Transform> GetChildCollectionWithTag(this Transform obj, string tag)
        {
            List<Transform> list = new List<Transform>();
            for (int i = 0; i < obj.childCount; i++)
            {
                if (obj.GetChild(i).CompareTag(tag))
                    list.Add(obj.GetChild(i));
            }
            return list;
        }

        /// <summary>
        /// 根据标签获取子物体
        /// </summary>
        /// <param name="gam">当前物体</param>
        /// <param name="tag">标签</param>
        /// <param name="objList">返回的子物体集合</param>
        public static void GetChildCollectionWithTag(this GameObject gam, string tag, ref List<GameObject> objList)
        {
            List<Transform> list = new List<Transform>();
            list = GetChildCollectionWithTag(gam.transform, tag);

            objList.AddRange(list.ConvertAll(T => T.gameObject));
        }

        /// <summary>
        /// 获取父对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Transform GetRootParent(this Transform obj)
        {
            Transform Root = obj.parent;
            while (Root.parent != null)
            {
                //  Root = Root.root    // transform.root 方法可以直接获取最上父节点
                Root = Root.parent;
            }
            return Root;
        }

        /// <summary>
        /// 把源对象身上的所有组件，添加到目标对象身上
        /// </summary>
        /// <param name="origin">源对象</param>
        /// <param name="target">目标对象</param>
        public static void CopyComponent(GameObject origin, GameObject target)
        {
            var originComs = origin.GetComponents<Component>();
            foreach (var item in originComs)
            {
                target.AddComponent(item.GetType());
            }
        }

        /// <summary>
        /// 改变游戏脚本：目标设为可用，源对象设为不可用
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        public static void ChangeScriptTo(this MonoBehaviour origin, MonoBehaviour target)
        {
            target.enabled = true;
            origin.enabled = false;
        }

        /// <summary>
        /// 从当前对象的子对象中查找，返回一个用tag做标识的活动的游戏物体变换的链表，如果没有找到则为空
        /// <para>从一个父对象进行过递归遍历，如果有子对象的tag和给定tag相符合时，则把该子对象存到链表数组中</para>
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="tag">标签</param>
        /// <param name="transList">结果Transform集合</param>
        public static void FindGameObjectWithTagRecursive(this Transform obj, string tag, ref List<Transform> transList)
        {
            foreach (var item in obj.transform.GetChildCollection())
            {
                // 如果子对象还有子对象，则再对子对象的子对象进行递归遍历
                if (item.childCount > 0)
                {
                    item.FindGameObjectWithTagRecursive(tag, ref transList);
                }

                if (item.tag == tag)
                {
                    transList.Add(item);
                }
            }
        }

        /// <summary>
        /// 从当前对象的子对象中查找，返回一个用tag做标识的活动的游戏物体的链表，如果没有找到则为空
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="tag"></param>
        /// <param name="objList"></param>
        public static void FindGameObjectWithTagRecursive(this GameObject obj, string tag, ref List<GameObject> objList)
        {
            List<Transform> list = new List<Transform>();
            obj.transform.FindGameObjectWithTagRecursive(tag, ref list);

            objList.AddRange(list.ConvertAll(T => T.gameObject));
        }

        /// <summary>
        /// 从父对象中查找组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="com">物体组件</param>
        /// <param name="parentLevel">向上查找的级别，使用1表示与本对象最近的一个级别</param>
        /// <param name="searchDepth">查找深度</param>
        /// <returns>查找成功返回相应组件对象，否则返回null</returns>
        public static T GetComponentInParent<T>(this Component com, int parentLevel = 1, int searchDepth = int.MaxValue) where T : Component
        {
            searchDepth--;

            if (com != null && searchDepth > 0)
            {
                var component = com.transform.parent.GetComponent<T>();
                if (component != null)
                {
                    parentLevel--;
                    if (parentLevel == 0)
                    {
                        return component;
                    }
                }
                return com.transform.parent.GetComponentInParent<T>(parentLevel, searchDepth);
            }
            return null;
        }
    }
}