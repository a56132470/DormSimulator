using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 利用ScriptableObject创建资源文件
/// </summary>
public class BuildAsset : Editor
{
    [MenuItem("BuildAsset/Build Scriptable Asset")]
    public static void ExcuteBuild()
    {
        ActionHolder holder = ScriptableObject.CreateInstance<ActionHolder>();

        // 查询excel表中数据，赋值给asset文件
        List<CharacterAction>[] characterActions = XMLManager.Instance.characterActionArray;
        holder.StudyActions = characterActions[0];
        holder.AmusementActions = characterActions[1];
        holder.LaborActions = characterActions[2];
        string path = "Assets/Resources/characterActions.asset";
        AssetDatabase.CreateAsset(holder, path);
        AssetDatabase.Refresh();
        Debug.Log("BuildAsset Success");
    }
}