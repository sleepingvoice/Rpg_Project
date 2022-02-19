using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


}
