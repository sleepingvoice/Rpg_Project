using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Threading.Tasks;

[SerializeField]
public class My_State
{
    public string ID_Code; // 아이디 코드
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
public class Save_Data : MonoBehaviour
{
    My_State state = new My_State();
    Obj_State Play_State;

    private void Start()
    {
        Play_State = GameManager.Instance.Player.GetComponent<Obj_State>();
    }

    private async void InventoryLoding()
    {
        await Task.Delay(1000);
        while (true)
        {
            UI_Inventory myInven = GameManager.Instance.Ui_Manage.Manager_Inven.GetComponent<UI_Inventory>();
            Load_State();
            myInven.Load_Inventory();
            myInven.reload_Inventory();
            await Task.Delay(60000); //60초마다 세이브 로드를 반복
            myInven.Save_Inventory();
            Save_State();
        }
    }

    private void Load_State()
    {

    }

    private void Save_State()
    {
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

        File.WriteAllText(Application.dataPath + "/Resources/State.json", JsonUtility.ToJson(state));
        string json = JsonUtility.ToJson(state);
        byte[] jsonByte = Encoding.UTF8.GetBytes(json);             // 제이슨을 바이트열로 변환
        string json_convert = Encoding.Default.GetString(jsonByte); // 바이트열을 제이스로 변환
        Debug.LogFormat("json is {0}, jsonbyte length is {1}", json, jsonByte.Length);
        
    }

}
