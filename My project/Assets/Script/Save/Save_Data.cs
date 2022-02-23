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
    private Obj_State Play_State;      // 플레이어 상태
    public string user_Code;           // 유저 코드
    private string Normal_Url = "http://localhost/Test/";

    private void Start()
    {
        Play_State = GameManager.Instance.Player.GetComponent<Obj_State>();
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

        List<string> json = divide_Base64(CheckCode.downloadHandler.text); // echo로 나타나는 값은 두개가 합쳐져있기에 나누어 준다.
        for(int i=0;i<json.Count;i++)
            json[i] = Decoding_Base64(json[i]);

        File.WriteAllText(Application.dataPath + "/Resources/State.json", json[0]);
        File.WriteAllText(Application.dataPath + "/Resources/Inventory.json", json[1]);
    }


    /// <summary>
    /// 유저의 json값을 서버에 저장하는 함수
    /// </summary>
    private IEnumerator Save_User_Info()
    {
        string post_state = Incoding_Base64(File.ReadAllText(Application.dataPath + "/Resources/State.json")); // josn값을 서버에 보내기위해 base64형태로 암호화한다.
        string post_Inven = Incoding_Base64(File.ReadAllText(Application.dataPath + "/Resources/Inventory.json"));

        WWWForm myState = new WWWForm();
        myState.AddField("User_code_Post", user_Code);
        myState.AddField("State_Post", post_state);
        myState.AddField("Inven_Post", post_Inven);

        UnityWebRequest SaveState = UnityWebRequest.Post(Normal_Url + "Save_UserInfo.php", myState);

        yield return SaveState.SendWebRequest();

        if (SaveState.error != null)
            Debug.LogError("저장안됨");
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
        arr.Add(tmp);
        return arr;
    }

    #endregion

    #region 스탯 저장
    /// <summary>
    /// 스탯 저장 함수
    /// </summary>
    private void Save_state()
    {
        My_State state = new My_State();
        state.Exp = Play_State.Exp;
        state.Lv = Play_State.Lv;
        state.total_Exp = Play_State.total_Exp;
        state.Str = Play_State.Str;
        state.Dex = Play_State.Dex;
        state.Int = Play_State.Int;
        state.Luk = Play_State.Luk;
        state.nowHp = Play_State.Hp;
        state.nowMp = Play_State.Mp;
        state.Position = Play_State.gameObject.transform.position;

        string json = JsonUtility.ToJson(state);
        File.WriteAllText(Application.dataPath + "/Resources/State.json", json);
    }

    /// <summary>
    /// 스탯 로드 함수
    /// </summary>
    private void Load_state()
    {
        My_State state = JsonUtility.FromJson<My_State>(File.ReadAllText(Application.dataPath + "/Resources/State.json"));
        Play_State.Exp = state.Exp;
        Play_State.Lv = state.Lv;
        Play_State.total_Exp = state.total_Exp;
        Play_State.Str = state.Str;
        Play_State.Dex = state.Dex;
        Play_State.Int = state.Int;
        Play_State.Luk = state.Luk;
        Play_State.Hp = state.nowHp;
        Play_State.Mp = state.nowMp;
        Play_State.gameObject.transform.position = state.Position;
    }

    #endregion

}
