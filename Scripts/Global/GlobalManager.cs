using System;
using Base.Inventory;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class GlobalManager : MonoBehaviour
{
    [SerializeField]
    public static GlobalManager Instance;

    // 玩家类
    public Player player;

    // 初始化舍友1，2,3
    public Roommate[] roommates;

    private int m_Invitation = 0;

    [FormerlySerializedAs("MyBag")] public Inventory myBag;
    [FormerlySerializedAs("Store")] public Inventory store;

    // 当前存档位
    [FormerlySerializedAs("SaveID")] public int saveId;

    public int Invitation
    {
        get => m_Invitation;
        set => m_Invitation = value;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        roommates = new Roommate[3];
        saveId = 0;

        myBag = Resources.Load("Inventory/Inventories/BagInventory") as Inventory;
        store = Resources.Load("Inventory/Inventories/StoreInventory") as Inventory;
    }
}