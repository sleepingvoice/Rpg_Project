using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base_Class
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

    [SerializeField]
    public class My_State
    {
        public string Name;    // 닉네임
        public int Exp;        // 경험치
        public int Lv;         // 레벨
        public int total_Exp;  // 총 경험치
        public float Str;      // 힘
        public float Dex;      // 민
        public float Int;      // 지
        public float Luk;      // 운
        public float nowHp;    // 현재 체력
        public float nowMp;    // 현재 마나
        public Vector3 Position; // 현재 위치
    }
}
