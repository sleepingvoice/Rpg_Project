using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Base_Class
{
    [Serializable]
    public class Data
    {
        public string Item_Code;
        public string name;
        public string explain;
        public string function;
        public string PrefabName;
    }

    [Serializable]
    public class Database
    {
        public List<Data> DataArray = new List<Data>();
    }

    [Serializable]
    public class Inventory_Item
    {
        public int Order;
        public string Item_Code;
        public int volume;
    }
    [Serializable]
    public class Inventory_Items
    {
        public List<Inventory_Item> Inventory = new List<Inventory_Item>();
    }

    [Serializable]
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

    [Serializable]
    public class Equip
    {
        public string Part;
        public string Item_Code;
    }

    [Serializable]
    public class Equipes
    {
        public List<Equip> myEquip = new List<Equip>();
    }

    [Serializable]
    public class Setting_Value
    {
        public float SE;
        public float BGM;
        public bool Window_Mode;
        public int Window_Width;
        public int Window_Height;
        public int dropbox_value;
    }
        
}
