using UnityEngine;

public class PlotManager : MonoBehaviour
{
    public static PlotManager instance;
    public GameObject mainFlowchart;
    public GameObject branchFlowchart;
    private Topic CurTopic;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void SetTopic(Topic tc)
    {
        CurTopic = tc;
    }

    public void ShowMain()
    {
        mainFlowchart.SetActive(true);
    }

    public void ShowBranch()
    {
        branchFlowchart.SetActive(true);
    }

    public void AddStateToPlayer(string StateName)
    {
        if (GlobalVariable.instance.player.stateDic.ContainsKey(StateName))
        {
            GlobalVariable.instance.player.stateDic.GetValue(StateName).Duration +=
             GlobalVariable.instance.player.stateDic.GetValue(StateName).RemainTime;
        }
        else
        {
            GlobalVariable.instance.player.AddState(StateName);
        }
    }

    public void AddSlotToPlayer(params string[] SlotName)
    {
    }

    public void AddPropertyToPlayer(int logic, int talk, int athletics, int creativity, int money)
    {
    }

    public bool CheckPlotIsLock(int index)
    {
        if (CurTopic.plots[index].isLock)
            return false;
        else
            return true;
    }

    public void ExitPlace(int index)
    {
        CurTopic.plots[index].isFinish = true;
        UIPanelManager.Instance.PopPanel();
    }
    public string GetVariable(string variableName)
    {
        switch(variableName)
        {
            case VariableName.PlayerName:
                return GlobalVariable.instance.player.Name;
            default:
                return null;
        }
    }
}