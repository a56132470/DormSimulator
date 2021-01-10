using UnityEngine;
public class GameInit : MonoBehaviour
{
    public static GameInit Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    private void Start()
    {
        // 初始化UI管理器，加入游戏开始界面
        UIPanelManager.Instance.PushPanel(UIPanelType.GameStart);

        EventCenter.AddListener<string, int, int>(EventType.GAME_INIT, InitRoles);
    }
#if UNITY_EDITOR
    private void Update()
    {
        if (GlobalManager.Instance.player != null)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                GlobalManager.Instance.player.CurRound += 1;
                EventCenter.Broadcast(EventType.UPDATE_ROUND_TXT);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                GlobalManager.Instance.player.CurRound -= 1;
                EventCenter.Broadcast(EventType.UPDATE_ROUND_TXT);
            }
        }
    }
#endif
    private void InitRoles(string playerName, int week, int count)
    {
        GlobalManager.Instance.player = new Player(playerName,
            50 + (week - 1) * 10,
            50 + (week - 1) * 10,
            50 + (week - 1) * 10,
            50 + (week - 1) * 10,
            100);
        GlobalManager.Instance.saveId = count;
        GlobalManager.Instance.player.CurWeek = week;
        GlobalManager.Instance.myBag.itemList.Clear();
        GlobalManager.Instance.store.itemList.Clear();

        string[] roommateStr = { "林青水", "方橙", "杜时节" };
        for (var i = 0; i < 3; i++)
        {
            GlobalManager.Instance.roommates[i] = new Roommate(roommateStr[i],
                Random.Range(30, 80),
                Random.Range(30, 80),
                Random.Range(30, 80),
                Random.Range(30, 80),
                Random.Range(30, 80),
                Random.Range(30, 80),
                0, i);
        }
        // 杜时节特批
        GlobalManager.Instance.roommates[2].SelfControl = 70;
        DSD.KernalTool.Widget.RemoveInivitationState();
    }
}