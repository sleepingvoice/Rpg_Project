using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Base_Class;
using System.IO;

public class UI_State : MonoBehaviour
{
    public Text[] State;
    public Button[] UpBtk;
    public GameObject[] noUpBtk;

    private Obj_State Play_State;
    private List<float> stat;

    private void Start()
    {
        Play_State = GameManager.Instance.Player.GetComponent<Obj_State>();
        UpdateState();
    }

    #region 스탯창

    /// <summary>
    /// 스탯버튼의 기능을 추가
    /// </summary>
    public void State_Btk_Set()
    {
        UpBtk[0].onClick.AddListener(() => UpState("Str"));
        UpBtk[1].onClick.AddListener(() => UpState("Dex"));
        UpBtk[2].onClick.AddListener(() => UpState("Int"));
        UpBtk[3].onClick.AddListener(() => UpState("Luk"));
    }

    /// <summary>
    /// 스탯창의 값을 갱신하는 함수
    /// </summary>
    private void UpdateState()
    {
        stat = new List<float>();
        stat.Add(Play_State.Str);
        stat.Add(Play_State.Dex);
        stat.Add(Play_State.Int);
        stat.Add(Play_State.Luk);

        for(int i=0;i<State.Length;i++)
            State[i].text = stat[i].ToString();
    }

    /// <summary>
    /// 버튼 클릭시 s의 이름을 가진 값의 스탯을 1올려주는 함수
    /// </summary>
    private void UpState(string s)
    {
        Play_State.Health_Map[s] += 1f;
        Play_State.Health_Map["LestPlusStats"]--;
        Play_State.inputValue();
        UpdateState();
        Save_state();
        if (Play_State.Health_Map["LestPlusStats"]<=0)
        {
            Plus_Stats_Btk_OnOff(false);
        }
    }

    public void Plus_Stats_Btk_OnOff(bool b)
    {
        foreach (Button btk in UpBtk)
        {
            btk.gameObject.SetActive(b);
        }
    }
    #endregion

    #region 스탯 저장
    /// <summary>
    /// 스탯 저장 함수
    /// </summary>
    public void Save_state()
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
    public void Load_state()
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
