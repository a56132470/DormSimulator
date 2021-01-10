using Base.Struct;
using UnityEngine;
using UnityEngine.UI;

namespace Panel
{
    public class MainMenuPanel : BasePanel
    {
        private Image m_Background;
        private Button m_ProfileBtn;
        private Button m_ActionBtn;
        private Button m_NextBtn;
        private Button m_SettingBtn;
        private Button m_MapBtn;
        private Button[] m_RoommateBtns = new Button[3];
        public Sprite[] sprites;
        private Text m_CurRoundTxt;

        public override void Init()
        {
            base.Init();
            m_Background = transform.Find("Background").GetComponent<Image>();
            m_ProfileBtn = transform.Find("ProfilePanel").GetComponent<Button>();
            m_ActionBtn = transform.Find("IconPanel/ActionBtn").GetComponent<Button>();
            m_NextBtn = transform.Find("IconPanel/NextBtn").GetComponent<Button>();
            m_SettingBtn = transform.Find("IconPanel/SettingBtn").GetComponent<Button>();
            m_MapBtn = transform.Find("IconPanel/MapBtn").GetComponent<Button>();
            m_CurRoundTxt = transform.Find("CurRoundTxt").GetComponent<Text>();

            m_RoommateBtns[0] = transform.Find("Roommates/Roommate1").GetComponent<Button>();
            m_RoommateBtns[1] = transform.Find("Roommates/Roommate2").GetComponent<Button>();
            m_RoommateBtns[2] = transform.Find("Roommates/Roommate3").GetComponent<Button>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            m_ProfileBtn.onClick.AddListener(OnProfileBtnClick);
            m_ActionBtn.onClick.AddListener(OnActionBtnClick);
            m_NextBtn.onClick.AddListener(OnNextBtnClick);
            m_SettingBtn.onClick.AddListener(OnSettingBtnClick);
            m_MapBtn.onClick.AddListener(OnMapBtnClick);
            m_RoommateBtns[0].onClick.AddListener(() => { OnRoommateBtnClick(0); });
            m_RoommateBtns[1].onClick.AddListener(() => { OnRoommateBtnClick(1); });
            m_RoommateBtns[2].onClick.AddListener(() => { OnRoommateBtnClick(2); });
            EventCenter.AddListener(EventType.NEXT_ROUND, NextRound);
            EventCenter.AddListener<string>(EventType.CHANGE_SKIN, SetSkin);
            EventCenter.AddListener(EventType.UPDATE_ACTIONPANEL_EVENT, SetCurRoundTxt);
            EventCenter.AddListener<bool>(EventType.CONTROLL_UI_ON_OFF, ControllUIShowAndFade);
            m_CurRoundTxt.text = "当前回合：" + GlobalManager.Instance.player.CurRound.ToString();
        
        }

        public override void OnExit()
        {
            base.OnExit();
            m_ProfileBtn.onClick.RemoveAllListeners();
            m_ActionBtn.onClick.RemoveAllListeners();
            m_NextBtn.onClick.RemoveAllListeners();
            m_SettingBtn.onClick.RemoveAllListeners();
            m_MapBtn.onClick.RemoveAllListeners();
            m_RoommateBtns[0].onClick.RemoveAllListeners();
            m_RoommateBtns[1].onClick.RemoveAllListeners();
            m_RoommateBtns[2].onClick.RemoveAllListeners();
            EventCenter.RemoveListener(EventType.NEXT_ROUND, NextRound);
            EventCenter.RemoveListener<string>(EventType.CHANGE_SKIN, SetSkin);
            EventCenter.RemoveListener(EventType.UPDATE_ACTIONPANEL_EVENT, SetCurRoundTxt);
            EventCenter.RemoveListener<bool>(EventType.CONTROLL_UI_ON_OFF, ControllUIShowAndFade);
        }

        private void OnProfileBtnClick()
        {
            UIPanelManager.Instance.PushPanel(UIPanelType.Player);
        }
        private void SetCurRoundTxt()
        {
            m_CurRoundTxt.text = "当前回合：" + GlobalManager.Instance.player.CurRound.ToString();
        }
        private void OnNextBtnClick()
        {
            EventCenter.Broadcast(EventType.NEXT_ROUND);
        }
        private void NextRound()
        {
            // 若玩家没有行动
            for (int i = 0; i < 5; i++)
            {
                if (GlobalManager.Instance.player.records[GlobalManager.Instance.player.CurRound - 1, i] == null)
                {
                    GlobalManager.Instance.player.records[GlobalManager.Instance.player.CurRound - 1, i] = "发呆了一天，啥也没干";
                }
            }
            // 这里执行Roommate的下一回合行动
            for (int i = 0; i < 3; i++)
            {
                if (i != GlobalManager.Instance.Invitation - 1)
                {
                    GlobalManager.Instance.roommates[i].AddFiveRecordAction();
                }
                else
                {
                    for (int j = 0; j < 5; j++)
                    {
                        GlobalManager.Instance.roommates[i].records[GlobalManager.Instance.player.CurRound - 1, j] =
                            GlobalManager.Instance.player.records[GlobalManager.Instance.player.CurRound - 1, j];
                    }
                }
            }
            // 当前回合加1
            GlobalManager.Instance.player.CurRound += 1;
            m_CurRoundTxt.text = "当前回合：" + GlobalManager.Instance.player.CurRound.ToString();
            GlobalManager.Instance.player.Strength = 5;
            foreach (System.Collections.Generic.KeyValuePair<string, State> k in GlobalManager.Instance.player.stateDic)
            {
                k.Value.RemainTime -= 1;
            }
            if (GlobalManager.Instance.player.stateDic.ContainsKey(StateName.Help3))
            {
                GlobalManager.Instance.player.propertyStruct += new PropertyStruct(1,1,1,1);
                GlobalManager.Instance.player.Strength += 1;
            }
            DSD.KernalTool.Widget.RemoveInivitationState();
            if (GlobalManager.Instance.player.CurRound >= 24)
            {
                UIPanelManager.Instance.PushPanel(UIPanelType.End);
                PlotManager.Instance.ExecuteMainBlock("Game_End");
            }
        }
        private void OnActionBtnClick()
        {
            UIPanelManager.Instance.PushPanel(UIPanelType.Action);
        }

        private void OnRoommateBtnClick(int id)
        {
            UIPanelManager.Instance.PushPanel(UIPanelType.Roommate);
            EventCenter.Broadcast(EventType.REFRESH_ROOMMATE, id);
        }

        private void OnSettingBtnClick()
        {
            UIPanelManager.Instance.PushPanel(UIPanelType.Setting);
        }

        private void OnMapBtnClick()
        {
            UIPanelManager.Instance.PushPanel(UIPanelType.Map);
        }

        public void SetSkin(string p)
        {
            switch (p)
            {
                case PlaceType.Classroom:
                    m_Background.sprite = sprites[0];
                    break;

                case PlaceType.Dorm:
                    m_Background.sprite = sprites[1];
                    break;
                case PlaceType.Playground:
                    m_Background.sprite = sprites[2];
                    break;
            }
        }
        private void ControllUIShowAndFade(bool flag)
        {
            for(int i = 0;i<transform.childCount;i++)
            {
                if(!transform.GetChild(i).name.Equals("Background"))
                {
                    transform.GetChild(i).gameObject.SetActive(flag);
                }
                if (transform.GetChild(i).name.Equals("SwitchRoundPanel"))
                    transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}