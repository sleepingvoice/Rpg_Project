using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item_Code
{
    [System.Serializable]
    public class Data
    {
        public string Item_Code;
        public string name;
        public string explain;
        public string function;
        public string PrefabName;
    }

    [System.Serializable]
    public class Database
    {
        public List<Data> DataArray = new List<Data>();
    }

    [System.Serializable]
    public class Inventory_Item
    {
        public int Order;
        public string Item_Code;
        public int volume;
    }
    [System.Serializable]
    public class Inventory_Items
    {
        public List<Inventory_Item> Inventory = new List<Inventory_Item>();
    }
}
