using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Base_Class;

public class Save_Data : MonoBehaviour
{
    private string Normal_Url = "http://localhost/Test/";
    private Dictionary<string, string> Init_Json;
    private Encoding_Base _EncodingComponent;

    private string User_Id;
    private string User_PassWord;

    public Dictionary<string, string> Player_Data;

    private void Start()
    {
        Init_Dictionary();
        StartCoroutine(Init_ItemCode());
    }

    public void Load_UserData(string Id_text, string Pass_text)
    {
        User_Id = Id_text;
        User_PassWord = Pass_text;

        StartCoroutine(Load_User_Info(User_Id, User_PassWord));
    }

    public void Save_UserData()
    {
        StartCoroutine(Save_User_Info(User_Id, User_PassWord));
    }


    #region 서버에 저장,로드

    /// <summary>
    /// 서버에 저장된 유저의 데이터를 불러오는 함수
    /// </summary>
    private IEnumerator Load_User_Info(string Id, string Pass)
    {
        WWWForm my_State = new WWWForm();
        my_State.AddField("ID_Post", Id);
        my_State.AddField("PassWord_Post", Pass);
        UnityWebRequest CheckCode = UnityWebRequest.Post(Normal_Url + "Load_UserInfo.php", my_State);

        yield return CheckCode.SendWebRequest();

        List<string> json = divide_Data(CheckCode.downloadHandler.text); // echo로 나타나는 값은 여러개가 합쳐져있기에 나누어 준다.

        Init_List(json);
        KeyCheck(json);

        Player_Data = new Dictionary<string, string>();
        Player_Data.Add("State", json[0]);
        Player_Data.Add("Inven", json[1]);
        Player_Data.Add("Equip", json[2]);


        SaveData_To_Sever();
    }


    /// <summary>
    /// 유저의 데이터를 서버에 저장하는 함수
    /// </summary>
    private IEnumerator Save_User_Info(string Id, string Pass)
    {
        string post_state = User_Info.Instance.myEncoding.EncodingText(Player_Data["State"]);
        string post_Inven = User_Info.Instance.myEncoding.EncodingText(Player_Data["Inven"]);
        string post_equip = User_Info.Instance.myEncoding.EncodingText(Player_Data["Equip"]);

        WWWForm myState = new WWWForm();
        myState.AddField("ID_Post", Id);
        myState.AddField("PassWord_Post", Pass);

        myState.AddField("State_Post", post_state);
        myState.AddField("Inven_Post", post_Inven);
        myState.AddField("Equip_Post", post_equip);

        myState.AddField("KeyCheck_Post",User_Info.Instance.myEncoding.EncodingText("KeyCheck"));

        UnityWebRequest SaveState = UnityWebRequest.Post(Normal_Url + "Save_UserInfo.php", myState);

        yield return SaveState.SendWebRequest();

        if (SaveState.error != null)
            Debug.LogError("저장안됨");
        else
            Debug.Log("세이브!");
    }

    /// <summary>
    /// 플레이어의 데이터를 서버에 10초마다 저장하는 비동기식 함수
    /// </summary>
    private async void SaveData_To_Sever()
    {
        await Task.Delay(10000);
        while (true)
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying == false)
                return;
#endif

            Save_UserData();
            Load_UserData(User_Id, User_PassWord);
            await Task.Delay(10000); //10초마다 데이터를 서버에 저장
        }
    }

    #endregion

    #region 보조함수

    /// <summary>
    /// 서버에서 보낸 값을 나누어주는 함수
    /// </summary>
    private List<string> divide_Data(string s)
    {
        List<string> arr = new List<string>();
        string tmp = "";
        for(int i=0;i<s.Length; i++)
        {
            if(s[i]==' ')
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
    /// 나누어진 값을 체크한 후 값이 없다면 초기화된 값을 넣어주고 값이 있다면 복호화후 돌려준다
    /// </summary>
    private List<string> Init_List(List<string> json)
    {
        json[0] = Init_Check(User_Info.Instance.myEncoding.DecodingText(json[0]), "State");
        json[1] = Init_Check(User_Info.Instance.myEncoding.DecodingText(json[1]), "Inven");
        json[2] = Init_Check(User_Info.Instance.myEncoding.DecodingText(json[2]), "Equip");
        json[3] = Init_Check(User_Info.Instance.myEncoding.DecodingText(json[3]), "KeyCheck");
        return json;
    }

    /// <summary>
    /// json값을 받아서 비어있을경우 초기화시켜주는 함수
    /// json값과 초기화시켜줄 문서의 이름을 받는다.
    /// </summary>
    private string Init_Check(string json,string init_name)
    {
        if(json == string.Empty)
        {
            Debug.Log(Init_Json[init_name]);
            return Init_Json[init_name];
        }
        return json;
    }

    /// <summary>
    /// 키값이 제대로 들어왔는지 체크하는 함수
    /// </summary>
    private List<string> KeyCheck(List<string> json)
    {
        if (json[3] != "KeyCheck")
        {
            Debug.LogError("잘못된 키값입니다.");

            json[0] = Init_Json["State"];
            json[1] = Init_Json["Inven"];
            json[2] = Init_Json["Equip"];
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
        my_inven.Money = 0;
        return JsonUtility.ToJson(my_inven);
    }

    private void Init_Dictionary()
    {
        Init_Json = new Dictionary<string, string>();
        Init_Json.Add("State", init_State());
        Init_Json.Add("Equip", Init_Equip());
        Init_Json.Add("Inven", init_Inventory());
        Init_Json.Add("KeyCheck", "KeyCheck");
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
    }

    private IEnumerator Save_itemCode()
    {
        WWWForm myData = new WWWForm();
        myData.AddField("Name_Post", "ItemCode");


        yield return null;
    }

    #endregion
}
