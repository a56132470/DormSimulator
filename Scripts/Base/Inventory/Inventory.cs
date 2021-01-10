﻿using System.Collections.Generic;
using UnityEngine;

namespace Base.Inventory
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/New Inventory")]
    public class Inventory : ScriptableObject
    {
        public List<Item> itemList;
    }
}