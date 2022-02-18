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
    public Sprite Black;


    private Database Big_Data;

    private void Start()
    {
        Chekc_Code();

    }

    private void Chekc_Code()
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

    /// <summary>
    /// 아이템 코드를 활용하여 스프라이트를 불러오는 함수
    /// </summary>
    private Sprite Find_Item_Img(string Code)
    {
        Data myItem = FindItem(Code);
        string Route = Find_Item_Route(myItem.Item_Code);
        Route += myItem.PrefabName;
        Debug.Log(Route);
        return Resources.Load<Sprite>("Icon" + Route);
    }

}
