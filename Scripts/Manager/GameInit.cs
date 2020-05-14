using UnityEngine;

public class GameInit : MonoBehaviour
{
    public static GameInit Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(this);
    }

    private void Start()
    {
        // 初始化UI管理器，加入游戏开始界面
        UIPanelManager.Instance.PushPanel(UIPanelType.GameStart);
    }

    public void Init(string PlayerName, int Week, int Count)
    {
        InitPlayer(PlayerName, Week, Count);
        InitRoommates();
    }

    private void InitPlayer(string PlayerName, int Week, int Count)
    {
        GlobalVariable.instance.player = new Player(PlayerName, 50, 50, 50, 50, 100);
        GlobalVariable.instance.SaveID = Count;
        GlobalVariable.instance.player.CurWeek = Week;
        GlobalVariable.instance.MyBag.itemList.Clear();
        GlobalVariable.instance.Store.itemList.Clear();
    }

    private void InitRoommates()
    {
        string[] RoommateStr = new string[3] { "林青水", "方橙", "杜时节" };
        for (int i = 0; i < 3; i++)
        {
            GlobalVariable.instance.roommates[i] = new Roommate(RoommateStr[i],
                Random.Range(30, 80),
                Random.Range(30, 80),
                Random.Range(30, 80),
                Random.Range(30, 80),
                Random.Range(30, 80),
                Random.Range(30, 80),
                0, i);
        }
    }
}