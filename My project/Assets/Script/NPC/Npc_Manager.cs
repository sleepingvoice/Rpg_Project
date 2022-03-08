using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using Npc;

public class Npc_Manager : MonoBehaviour
{
    [Header("UI전체")]
    [SerializeField] private GameObject Player_UI;
    [SerializeField] private GameObject Npc_UI;
    
    [Header("Npc 오브젝트")]
    public GameObject[] NPC_obj;

    [Header("대화창 관련 오브젝트")]
    public TextMeshProUGUI CharName;
    public TextMeshProUGUI Line;
    public Button[] Npc_Btk;

    private Npc_States myNPC_state = new Npc_States();
    private Npc_State now_state = new Npc_State();
    private int LineNum;

    private void Awake()
    {
        Loadjson();
    }

    /// <summary>
    /// npc json 로드
    /// </summary>
    private void Loadjson()
    {
        string s = File.ReadAllText(Application.streamingAssetsPath + "/Json/Npc.json"); // npc 파일 위치
        myNPC_state = JsonUtility.FromJson<Npc_States>(s);
        Debug.Log(s);
    }

    #region Npc 초기 세팅

    /// <summary>
    /// 오브젝트 이름으로 Npc박스를 세팅시키는 함수
    /// </summary>
    public void Npc_box_setting(string objName)
    {
        now_state = Find_NpcState_From_ObjName(objName); // 오브젝트 이름의 저장된 내용을 불러온다.
        CharName.text = now_state.Char_name; // 이름을 넣어준다.
        Line.text = now_state.Line[0]; // 첫번째 대화를 불러온다.
        LineNum = 1;
        btk_return();
        Setting_button();
        UI_OnOff(true); // UI를 킨다.
    }

    #endregion

    #region 버튼 함수

    /// <summary>
    /// 버튼을 세팅하는 함수
    /// </summary>
    private void Setting_button()
    {
        if (now_state.Line.Count <= LineNum) // 현재 나와있는 대화가 state에 저장된 대화량과 같거나 많다면
        {
            for (int i = 0; i < now_state.Button_Function.Count; i++)
            {
                btkInsert(Npc_Btk[i], now_state.Button_Function[i]); //state에 저장된 버튼 기능을 불러온다.
                Npc_Btk[i].gameObject.SetActive(true);
            }
        }
        else if (now_state.Line.Count > LineNum) // 현재 나와있는 대화가 state에 저장된 대화량 보다 적다면
        {
            btkInsert(Npc_Btk[0], "Next"); // 다음 대화로 넘어갈수 있는 버튼을 활성화 시킨다.
            Npc_Btk[0].gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// function_name을 이용하여 버튼에 함수를 넣어주는 함수
    /// </summary>
    private void btkInsert(Button btk,string function_name)
    {
        if (function_name == "End")
        {
            btk.onClick.AddListener(() => End());
            btk.GetComponentInChildren<TextMeshProUGUI>().text = "종료";
        }
        else if (function_name == "Shop")
        {
            btk.onClick.AddListener(() => { });
            btk.GetComponentInChildren<TextMeshProUGUI>().text = "상점";
        }
        else if(function_name == "Quest")
        {
            btk.onClick.AddListener(() => { });
            btk.GetComponentInChildren<TextMeshProUGUI>().text = "퀘스트";
        }
        else if(function_name == "Next")
        {
            btk.onClick.AddListener(() => Next());
            btk.GetComponentInChildren<TextMeshProUGUI>().text = "다음으로";
        }
    }

    /// <summary>
    /// 버튼 초기화
    /// </summary>
    private void btk_return()
    {
        foreach(Button tmp in Npc_Btk)
        {
            tmp.onClick.RemoveAllListeners();
        }

    }

    #endregion

    #region 버튼 기능

    private void End()
    {
        for(int i=0;i<Npc_Btk.Length;i++)
        {
            Npc_Btk[i].gameObject.SetActive(false);
        }
        Line.text = "";
        CharName.text = "";
        UI_OnOff(false);
    }

    private void Shop()
    {

    }

    private void Quest()
    {

    }

    private void Next()
    {
        if (now_state.Line.Count > LineNum)
            Line.text = now_state.Line[LineNum];
        LineNum++;
        Setting_button();
    }

    #endregion

    #region 보조함수

    /// <summary>
    /// 오브젝트 이름으로 Npc_state 불러오는 함수
    /// </summary>
    private Npc_State Find_NpcState_From_ObjName(string s)
    {
        foreach(Npc_State Npc_tmp in myNPC_state.Npc)
        {
            if (Npc_tmp.Obj_name == s)
                return Npc_tmp;
        }
        return null;
    }

    /// <summary>
    /// true 일때 NpcUI를 키고 PlayerUI를 끈다
    /// </summary>
    /// <param name="b"></param>
    private void UI_OnOff(bool b)
    {
        Npc_UI.SetActive(b);
        Player_UI.SetActive(!b);
    }

    #endregion
}
