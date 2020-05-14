using UnityEngine;
using UnityEngine.UI;

public class StatePanel : MonoBehaviour
{
    public State state;
    private Text title;
    private Text effect;
    private Text remainRound;

    private void Awake()
    {
        title = transform.Find("Title").GetComponent<Text>();
        effect = transform.Find("Effect").GetComponent<Text>();
        remainRound = transform.Find("RemainRound").GetComponent<Text>();
    }

    public void setState(State st, string ef)
    {
        state = st;
        title.text = st.Name;
        remainRound.text = "剩余" + st.RemainTime.ToString() + "回合";
        effect.text = ef;
    }
}