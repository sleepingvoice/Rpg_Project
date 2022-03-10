using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Base_Class;
using System.IO;

public class UI_Equip : MonoBehaviour
{
    public RawImage[] Equip_Icons; // 장비창 아이콘
    public Text[] Icons_Name;      // 장비창 아이콘 위의 이름

    [HideInInspector]public Dictionary<string, RawImage> Equip_Icons_Name; // 장비창 아이콘의 이름을 저장하는 맵변수
    private Equipes my_Equip;


    public void Start_Equip()
    {
        Mapping();
        Equip_Load();
    }

    public void Start_Equip_Save()
    {
        Equip_Save();
    }


    #region 장비창 UI관련

    /// <summary>
    /// 아이템 코드에 따라 맞는 장비창의 이미지를 불러준다.
    /// </summary>
    public RawImage Change_Equip_Item(string Item_Code)
    {
        if (Item_Code == "")
            return null;
        if (Item_Code[0] == 'W')
        {
            return Equip_Icons_Name["Weapon"];
        }
        else if (Item_Code[0] == 'A')
        {
            switch (Item_Code[2])
            {
                case 'B':
                    return Equip_Icons_Name["Body"];
                case 'H':
                    return Equip_Icons_Name["Head"];
                case 'F':
                    return Equip_Icons_Name["Foot"];
                case 'G':
                    return Equip_Icons_Name["Gloves"];
            }
        }
        return null;
    }

    /// <summary>
    /// 장비창에 있는 아이템의 능력치를 추가해주는 함수(int가 1일시 더해주는것이고 -1일시 빼주는것이다.)
    /// </summary>
    public void Plus_Item_State(string Item_Code, int PlusMinus)
    {
        UI_Manager my_UI = GameManager.Instance.Ui_Manager;
        Data my_Item = my_UI.FindItem(Item_Code);
        if (my_Item.function != "") // 만약 아이템의 효과가 비어있지 않다면
        {
            List<string> item_function = Slide_String_function(my_Item.function); // 아이템 효과를 나눠준다(위치와 값)
            int value = int.Parse(item_function[1]);
            value *= PlusMinus; // 더하거나 빼는것을 모두 처리하기 위함
            Obj_State my_state = GameManager.Instance.Player.GetComponent<Obj_State>();
            if (item_function[0] == "Atk") // 효과가 공격에 들어갈시
                my_state.Plus_Atk += value;
            else if (item_function[0] == "Def") // 효과가 방어에 들어갈시
                my_state.Plus_Def += value;

            my_state.Roboot(); // 추가된 데미지나 방어력을 갱신시켜준다.
        }
    }

    /// <summary>
    /// 장비창의 무기에 따라 캐릭터가 들고있는 무기를 다르게 해주는 함수
    /// 아이템 코드와 들어오는 장비창의 이름을 받는다.
    /// </summary>
    public void Change_Weapons(string Item_Code, string Equip_name)
    {
        if (Equip_name != "Weapon")
            return;

        UI_Manager my_UI = GameManager.Instance.Ui_Manager;
        Data myWeapons = my_UI.FindItem(Item_Code);
        Debug.Log(myWeapons.PrefabName);
        foreach (GameObject obj in GameManager.Instance.Player.GetComponent<Char_Weapons>().myWeapons)
        {
            obj.SetActive(false);
            if (obj.name == myWeapons.PrefabName)
            {
                obj.SetActive(true);
            }
        }
    }

    #endregion

    #region 장비창 이름 관련

    /// <summary>
    /// 아이콘의 이름을 넣으면 그에 해당하는 아이콘의 이름을 활성화시키는 함수
    /// </summary>
    public void Hide_Icon_Name(string Icon_Name, bool b)
    {
        for (int i = 0; i < Equip_Icons.Length; i++)
        {
            if (Equip_Icons[i].name == Icon_Name)
                Icons_Name[i].gameObject.SetActive(b);
        }
    }

    #endregion

    #region 장비창 세이브 로드


    /// <summary>
    /// 장비창 세이브 함수
    /// </summary>
    private void Equip_Save()
    {
        my_Equip = new Equipes();
        foreach (var name in Equip_Icons_Name)
        {
            Equip tmp = new Equip();
            tmp.Item_Code = GameManager.Instance.Ui_Manager.Find_Item_Code(name.Value.texture.name);
            tmp.Part = name.Key;
            Debug.Log(tmp.Item_Code + " " +tmp.Part);
            my_Equip.myEquip.Add(tmp);
        }
        File.WriteAllText(Application.persistentDataPath + "/Equip.json", JsonUtility.ToJson(my_Equip));
    }

    /// <summary>
    /// 장비창 로드 함수
    /// </summary>
    private void Equip_Load()
    {
        string str = File.ReadAllText(Application.persistentDataPath + "/Equip.json");
        my_Equip = JsonUtility.FromJson<Equipes>(str);

        foreach(var equip in my_Equip.myEquip)
        {
            RawImage raw = Equip_Icons_Name[equip.Part];
            if(equip.Item_Code!="")
            {
                raw.texture = GameManager.Instance.Ui_Manager.Find_Item_Img(equip.Item_Code);
            }
        }
    }


    #endregion

    #region 보조함수
    private void Mapping() // 맵변수에 값을 추가해준다.
    {
        Equip_Icons_Name = new Dictionary<string, RawImage>();
        Equip_Icons_Name.Add("Head", Equip_Icons[0]);
        Equip_Icons_Name.Add("Body", Equip_Icons[1]);
        Equip_Icons_Name.Add("Foot", Equip_Icons[2]);
        Equip_Icons_Name.Add("Weapon", Equip_Icons[3]);
        Equip_Icons_Name.Add("Gloves", Equip_Icons[4]);
    }

    private List<string> Slide_String_function(string s)
    {
        List<string> my_string = new List<string>();
        string tmp = "";
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] != '_')
            {
                tmp += s[i];
            }
            else
            {
                my_string.Add(tmp);
                tmp = "";
            }
        }
        my_string.Add(tmp);
        return my_string;
    }
    #endregion





}
