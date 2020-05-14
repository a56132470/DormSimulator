using System;
using UnityEngine;

[Serializable]
public class GlobalVariable : MonoBehaviour
{
    [SerializeField]
    public static GlobalVariable instance;

    // 玩家类
    public Player player;

    // 初始化舍友1，2,3
    public Roommate[] roommates;

    public Inventory MyBag;
    public Inventory Store;

    // 当前存档位
    public int SaveID;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        roommates = new Roommate[3];
        SaveID = 0;

        MyBag = Resources.Load("Inventory/Inventories/BagInventory") as Inventory;
        Store = Resources.Load("Inventory/Inventories/StoreInventory") as Inventory;
    }
}