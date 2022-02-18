using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Item_Code;
using System.IO;

public class UI_Inventory : MonoBehaviour
{
    [Header("아이템 창")]
    public RawImage[] Icons;

    [Header("빈칸 이미지")]
    public Texture Black;

    private Inventory_Items Inven;
    private Database Big_Data;

    private void Awake()
    {
        Load_Inventory();
        Check_Code();
        reload_Inventory();
    }

    private void Load_Inventory()
    {
        string str = File.ReadAllText(Application.dataPath + "/Resources/Inventory.json");
        Inven = JsonUtility.FromJson<Inventory_Items>(str);
    }

    private void reload_Inventory()
    {
        foreach(Inventory_Item item in Inven.Inventory)
        {
            if (item.Item_Code != "")
            {
                Icons[item.Order].texture = Find_Item_Img(item.Item_Code);
            }
            else
            {
                Icons[item.Order].texture = Black;
            }
        }
    }

    private void Save_Inventory()
    {
        File.WriteAllText(Application.dataPath + "/Resources/Inventory.json", JsonUtility.ToJson(Inven));
    }


    private void Check_Code()
    {
        string str = File.ReadAllText(Application.dataPath + "/Resources/Item_Code.json");
        Big_Data = JsonUtility.FromJson<Database>(str);
    }

    /// <summary>
    /// 아이템 코드를 활용하여 아이템의 Data를 불러오는 함수
    /// </summary>
    public Data FindItem(string Code)
    {
        foreach (Data data in Big_Data.DataArray)
        {
            if (data.Item_Code == Code)
            {
                return data;
            }
        }

        Debug.LogError("없는 아이템 코드입니다.");
        return null;
    }

    /// <summary>
    /// 아이템 코드를 활용하여 텍스쳐를 불러오는 함수
    /// </summary>
    public Texture Find_Item_Img(string Code)
    {
        Data myItem = FindItem(Code);
        string Route = Find_Item_Route(myItem.Item_Code);
        Route += myItem.PrefabName;
        return Resources.Load<Texture>("Icon" + Route);
    }

    private string Find_Item_Route(string Name)
    {
        string s = "";
        switch (Name[0])
        {
            case 'W':
                s += "/Weapon";
                break;
            case 'A':
                s += "/Ammor";
                break;
            case 'M':
                s += "/Material/";
                return s;
            case 'H':
                s += "/Postion/";
                return s;
        }
        switch (Name[2])
        {
            case 'S':
                s += "/Sword/";
                return s;
            case 'A':
                s += "/Axe/";
                return s;
            case 'D':
                s += "/Dagger/";
                return s;
            case 'M':
                s += "/Mace/";
                return s;
            case 'B':
                s += "/Body/";
                return s;
            case 'F':
                s += "/Foot/";
                return s;
            case 'G':
                s += "/Glove/";
                return s;
            case 'H':
                s += "/Head/";
                return s;
        }
        Debug.LogError("경로를 찾지 못했습니다.");
        return null;
    }



}
