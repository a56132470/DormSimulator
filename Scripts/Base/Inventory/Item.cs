using UnityEngine;
using UnityEngine.Serialization;

namespace Base.Inventory
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]
    public class Item : ScriptableObject
    {
        [FormerlySerializedAs("ItemName")] public string itemName;
        [FormerlySerializedAs("ItemImage")] public Sprite itemImage;
        [FormerlySerializedAs("ItemHeld")] public int itemHeld;
    
        // =9999 则为非卖
        [FormerlySerializedAs("ItemPrice")] public int itemPrice;

        [FormerlySerializedAs("ItemEffect")] [TextArea]
        public string itemEffect;

        [TextArea]
        public string ItemCaption;
    }
}