using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Item_Code;

public class UI_Equip : MonoBehaviour
{
    public RawImage[] Equip_Icons; // 장비창 아이콘
    public GameObject[] Name;      // 장비창 이름
    public Texture Blank_tex;      // 빈칸 텍스쳐

    [HideInInspector]public Dictionary<string, RawImage> Equip_Icons_Name; // 장비창 아이콘의 이름을 저장하는 맵변수


    private void Awake()
    {
        Mapping();
    }

    private void Mapping() // 맵변수에 값을 추가해준다.
    {
        Equip_Icons_Name = new Dictionary<string, RawImage>();
        Equip_Icons_Name.Add("Head", Equip_Icons[0]);
        Equip_Icons_Name.Add("Body", Equip_Icons[1]);
        Equip_Icons_Name.Add("Foot", Equip_Icons[2]);
        Equip_Icons_Name.Add("Weapon", Equip_Icons[3]);
        Equip_Icons_Name.Add("Gloves", Equip_Icons[4]);
    }

    /// <summary>
    /// 이름 없애는 함수
    /// </summary>
    public void Check_Name()
    {
        for(int i=0;i<Equip_Icons.Length;i++)
        {
            if(Equip_Icons[i].texture != Blank_tex)
                Name[i].SetActive(false);
            else
                Name[i].SetActive(true);
        }
    }


    /// <summary>
    /// 아이템 코드에 따라 맞는 장비창의 이미지를 불러준다.
    /// </summary>
    public RawImage Change_Equip_Item(string Item_Code)
    {
        if(Item_Code[0] == 'W')
        {
            return Equip_Icons_Name["Weapon"];
        }
        else if(Item_Code[0] == 'A')
        {
            switch(Item_Code[2])
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
    public void Plus_Item_State(string Texture_name,int PlusMinus)
    {
        UI_Manager my_UI = GameManager.Instance.Ui_Manage;
        string Code = my_UI.Find_Item_Code(Texture_name);
        Data my_Item = my_UI.FindItem(Code);
        if (my_Item.function != "")
        {
            List<string> item_function = Slide_String_function(my_Item.function);
            int value = int.Parse(item_function[1]);
            value *= PlusMinus;
            Obj_State my_state = GameManager.Instance.Player.GetComponent<Obj_State>();
            if (item_function[0] == "Atk")
                my_state.Plus_Atk += value;
            else if (item_function[0] == "Def")
                my_state.Plus_Def += value;

            my_state.Roboot();
        }
    }

    private List<string> Slide_String_function(string s)
    {
        List<string> my_string = new List<string>();
        string tmp = "";
        for(int i=0;i<s.Length;i++)
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
        return my_string;
    }

}
