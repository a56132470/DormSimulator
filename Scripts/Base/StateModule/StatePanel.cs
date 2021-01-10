using UnityEngine;
using UnityEngine.UI;

public class StatePanel : MonoBehaviour
{
    public State state;
    private Text m_Title;
    private Text m_Effect;
    private Text m_RemainRound;

    private void Awake()
    {
        m_Title = transform.Find("Title").GetComponent<Text>();
        m_Effect = transform.Find("Effect").GetComponent<Text>();
        m_RemainRound = transform.Find("RemainRound").GetComponent<Text>();
    }

    public void SetState(State st, string ef)
    {
        state = st;
        m_Title.text = st.Name;
        if(st.StType==StateType.Temporarily)
            m_RemainRound.text = "剩余" + st.RemainTime.ToString() + "回合";
        m_Effect.text = ef;
    }
}