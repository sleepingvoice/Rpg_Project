using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;

using Base_Class;

public class Save_Data : MonoBehaviour
{
    [HideInInspector]public string user_Code;           // 유저 코드
    private string Normal_Url = "http://localhost/Test/";
    private Dictionary<string, string> Init_Json;

    private void Start()
    {
        user_Code = "";
        Init_Dictionary();
        StartCoroutine(Init_ItemCode());
    }

    public void Load_UserData()
    {
        StartCoroutine(Load_User_Info());
    }

    public void Save_UserData()
    {
        StartCoroutine(Save_User_Info());
    }


    #region 서버에 저장,로드

    /// <summary>
    /// 서버에 저장된 유저의 json값을 불러오는 함수
    /// </summary>
    private IEnumerator Load_User_Info()
    {
        WWWForm my_State = new WWWForm();
        my_State.AddField("User_Code", user_Code);
        UnityWebRequest CheckCode = UnityWebRequest.Post(Normal_Url + "Load_UserInfo.php", my_State);

        yield return CheckCode.SendWebRequest();

        List<string> json = divide_Base64(CheckCode.downloadHandler.text); // echo로 나타나는 값은 여러개가 합쳐져있기에 나누어 준다.
        for (int i = 0; i < json.Count; i++)
        {
            json[i] = Decoding_Base64(json[i]);
        }

        File.WriteAllText(Application.persistentDataPath + "/State.json", Init_Check(json[0] , "State"));
        File.WriteAllText(Application.persistentDataPath + "/Inventory.json", Init_Check(json[1] , "Inven"));
        File.WriteAllText(Application.persistentDataPath + "/Equip.json", Init_Check(json[2], "Equip"));
    }


    /// <summary>
    /// 유저의 json값을 서버에 저장하는 함수
    /// </summary>
    private IEnumerator Save_User_Info()
    {
        string post_state = Incoding_Base64(File.ReadAllText(Application.persistentDataPath + "/State.json")); // josn값을 서버에 보내기위해 base64형태로 암호화한다.
        string post_Inven = Incoding_Base64(File.ReadAllText(Application.persistentDataPath + "/Inventory.json"));
        string post_equip = Incoding_Base64(File.ReadAllText(Application.persistentDataPath + "/Equip.json"));

        WWWForm myState = new WWWForm();
        myState.AddField("User_code_Post", user_Code);
        myState.AddField("State_Post", post_state);
        myState.AddField("Inven_Post", post_Inven);
        myState.AddField("Equip_Post", post_equip);

        UnityWebRequest SaveState = UnityWebRequest.Post(Normal_Url + "Save_UserInfo.php", myState);

        yield return SaveState.SendWebRequest();

        if (SaveState.error != null)
            Debug.LogError("저장안됨");
        else
            Debug.Log("세이브!");
    }
    #endregion

    #region Base64기반 암복호화

    /// <summary>
    /// string을 base64형태로 인코딩
    /// </summary>
    private string Incoding_Base64(string s)
    {
        byte[] arr = Encoding.UTF8.GetBytes(s);
        return System.Convert.ToBase64String(arr);
    }

    /// <summary>
    /// base64를 string형태로 디코딩
    /// </summary>
    private string Decoding_Base64(string s)
    {
        if (s == "")
            return s;
        string st1 = s.Replace("\n", "");                      
        st1 = st1.Replace("\r", "");                             // 서버에서 날라온 값의 앞에 엔터가 되어있으므로 그것을 제외해준다.
        st1 += new string('=', (4 - st1.Length % 4)%4);              // 값이 유니티의 base64형태의 기준과 맞지 않기에 바꿔준다.
        byte[] arr = Convert.FromBase64String(st1);
        return Encoding.UTF8.GetString(arr);
    }
    #endregion

    #region 보조함수

    /// <summary>
    /// 서버에서 보낸 값을 나누어주는 함수
    /// </summary>
    private List<string> divide_Base64(string s)
    {
        List<string> arr = new List<string>();
        string tmp = "";
        for(int i=0;i<s.Length; i++)
        {
            if(s[i]=='/')
            {
                arr.Add(tmp);
                tmp = "";
            }
            else
            {
                tmp += s[i];
            }
        }
        return arr;
    }

    /// <summary>
    /// json값을 받아서 비어있을경우 초기화시켜주는 함수
    /// json값과 초기화시켜줄 문서의 이름을 받는다.
    /// </summary>
    private string Init_Check(string json,string init_name)
    {
        if(json == "")
        {
            return Init_Json[init_name];
        }
        return json;
    }

    #endregion

    #region 초기화함수

    /// <summary>
    /// 스탯 초기화
    /// </summary>
    private string init_State()
    {
        My_State state = new My_State();
        state.Lv = 1;
        state.Exp = 0;
        state.total_Exp = 100;
        state.Str = 2;
        state.Dex = 2;
        state.Int = 2;
        state.Luk = 2;
        state.nowHp = 100;
        state.nowMp = 100;
        state.Position = new Vector3(0f, 0.22f, 0f);
        return JsonUtility.ToJson(state);
    }

    /// <summary>
    /// 장비창 초기화
    /// </summary>
    private string Init_Equip()
    {
        Equipes my_Equip = new Equipes();
        List<string> icon_name = new List<string>();
        icon_name.Add("Head");
        icon_name.Add("Body");
        icon_name.Add("Foot");
        icon_name.Add("Weapon");
        icon_name.Add("Gloves");

        foreach (string name in icon_name)
        {
            Equip tmp = new Equip();
            tmp.Item_Code = "";
            tmp.Part = name;
            my_Equip.myEquip.Add(tmp);
        }
        return JsonUtility.ToJson(my_Equip);
    }

    /// <summary>
    /// 인벤토리 초기화
    /// </summary>
    private string init_Inventory()
    {
        Inventory_Items my_inven = new Inventory_Items();

        for (int i=0;i<20;i++)
        {
            Inventory_Item item = new Inventory_Item();
            item.volume = 0;
            item.Item_Code = "";
            item.Order = i;
            my_inven.Inventory.Add(item);
        }
        return JsonUtility.ToJson(my_inven);
    }

    private void Init_Dictionary()
    {
        Init_Json = new Dictionary<string, string>();
        Init_Json.Add("State", init_State());
        Init_Json.Add("Equip", Init_Equip());
        Init_Json.Add("Inven", init_Inventory());
    }

    /// <summary>
    /// 아이템 코드 초기화함수
    /// </summary>
    private IEnumerator Init_ItemCode()
    {
        WWWForm my_Data = new WWWForm();
        my_Data.AddField("Name_Post", "ItemCode");
        UnityWebRequest CheckCode = UnityWebRequest.Post(Normal_Url + "Load_GameDataBase.php", my_Data);

        yield return CheckCode.SendWebRequest();

        string s = Decoding_Base64(CheckCode.downloadHandler.text);

        File.WriteAllText(Application.persistentDataPath + "/Item_Code.json", s);
    }

    private IEnumerator Save_itemCode()
    {
        string s = Incoding_Base64(File.ReadAllText(Application.persistentDataPath + "/Item_Code.json"));
        WWWForm myData = new WWWForm();
        myData.AddField("Name_Post", "ItemCode");
        myData.AddField("Value_Post", s);

        UnityWebRequest SaveState = UnityWebRequest.Post(Normal_Url + "Save_GameDataBase.php", myData);

        yield return SaveState.SendWebRequest();
    }

    #endregion
}
