using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Base_Class;
using System.IO;

public class UI_Manager : MonoBehaviour
{
    public GameObject Monster_Bar;
    public GameObject Bar_Hp;
    public GameObject Bar_Mp;
    public GameObject Bar_Exp;
    [Header("관리하는 매니저")]
    public UI_Equip Manager_Equip; // 장비
    public UI_Inventory Manager_Inven; // 인벤토리
    public UI_State Manager_State; // 스탯창
    public UI_Setting Manager_Setting; // 세팅창
    public UI_Notice Manager_Notice; // 공지사항
    private UI_Mouse Manager_Mouse; // UI 마우스 클릭

    [Header("빈칸 이미지")]
    public Texture Black_Item; // 인벤토리 빈칸 텍스쳐
    public Texture Black_Equip; // 장비창 빈칸 텍스쳐

    private Database Big_Data;

    private void Awake()
    {
        Check_Code();
        Manager_Mouse = GetComponent<UI_Mouse>();
        Awake_Fuction();
    }

    private void Awake_Fuction()
    {
        Manager_Inven.Get_InvenLoding();
        StartCoroutine(Manager_Inven.Inventory_Loding());
        Manager_State.Awake_Fuction();
        StartCoroutine(Manager_Notice.Notice_Text());
    }

    private void Start()
    {
        Manager_State.State_Btk_Set();
        Manager_Equip.Start_Equip();
        Manager_Setting.Load_Setting_Value();
        Manager_State.Load_state();
        Manager_Notice.gameObject.SetActive(true);
    }

    private void Update()
    {
        Bar_Update();
        Manager_Setting.Setting_Update();
        Manager_State.Update_Function();
    }

    private void Bar_Update()
    {
        Bar_ValueChange(Bar_Hp, "Hp");
        Bar_ValueChange(Bar_Mp, "Mp");
        Bar_ValueChange(Bar_Exp, "Exp");
        Monster();
    }

    private void Bar_ValueChange(GameObject Bar,string s)
    {
        Obj_State Player_state = GameManager.Instance.Player.GetComponent<Obj_State>();
        float max = Player_state.Health_Map["Max" + s];
        float now = Player_state.Health_Map[s];
        Bar.GetComponent<UI_BarContent>().Slider.value = now / max;
        Bar.GetComponent<UI_BarContent>().value.text = "<b>" + now + " / " + max + "</b>";
    }

    private void Monster()
    {
        GameObject Monster= GameManager.Instance.Player.GetComponent<Char_Attack>().Monster;
        if (Monster == null)
        {
            Monster_Bar.SetActive(false);
            return;
        }
        Obj_State Monster_state = Monster.GetComponent<Obj_State>();
        float max = Monster_state.Health_Map["MaxHp"];
        float now = Monster_state.Health_Map["Hp"];

        Monster_Bar.GetComponent<UI_BarContent>().Slider.value = now / max;
        Monster_Bar.GetComponent<UI_BarContent>().value.text = "<b>" + now + " / " + max + "</b>";
        Monster_Bar.GetComponent<UI_BarContent>().name.text = Monster_state.Name;
        if (now > 0)
            Monster_Bar.SetActive(true);
        else
            Monster_Bar.SetActive(false);
    }

    private void Check_Code()
    {
        string str = File.ReadAllText(Application.streamingAssetsPath + "/Json/Item_Code.json");
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

    /// <summary>
    /// 아이템 코드로 텍스쳐 경로 찾는 함수
    /// </summary>
    public string Find_Item_Route(string Name)
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
    /// 텍스쳐 이름으로 아이템 코드 찾는 함수
    /// </summary>
    public string Find_Item_Code(string Name)
    {
        if (Name == Black_Item.name || Name == Black_Equip.name)
        {
            return "";
        }
        foreach (Data data in Big_Data.DataArray)
        {
            if (Name == data.PrefabName)
            {
                return data.Item_Code;
            }
        }

        Debug.LogError("찾은 이름의 프리펩이 존재하지 않습니다.");
        return null;
    }
}
