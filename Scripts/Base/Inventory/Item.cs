using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]
public class Item : ScriptableObject
{
    public string ItemName;
    public Sprite ItemImage;
    public int ItemHeld;

    // =9999 则为非卖
    public int ItemPrice;

    [TextArea]
    public string ItemEffect;

    [TextArea]
    public string ItemCaption;
}