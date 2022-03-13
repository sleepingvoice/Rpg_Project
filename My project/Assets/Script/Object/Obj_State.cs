using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_State : MonoBehaviour
{
     public float Hp; // 체력
     public float Mp; // 마력
    [HideInInspector] public float Atk;// 공격
    [HideInInspector] public float Def;// 방어
    [HideInInspector] public float Atk_Speed;//공속
    [HideInInspector] public float Atk_Time; //공격시간
    [HideInInspector] public bool Alive;     //생존유무
                                             
    private float MaxHp;
    private float MaxMp;
    
    [HideInInspector] public Dictionary<string, float> Health_Map;

    [Header("이름")]
    public string Name;

    [Header("레벨")]
    public int Exp;        // 경험치
    public int Lv;         // 레벨
    public int total_Exp;

    [Header("능력치")]
    public float Str;      // 힘
    public float Dex;      // 민
    public float Int;      // 지
    public float Luk;      // 운

    public int Lest_Plus_stats; //남은 추가 스탯
    [HideInInspector] public float Plus_Atk; // 장비로 인해 올라가는 데미지
    [HideInInspector] public float Plus_Def; // 장비로 인해 올라가는 방어력
    private void Awake()
    {
        Plus_Atk = 0f;
        Plus_Def = 0f;
        Alive = true;
        Lest_Plus_stats = 0;
        State_renewal();
        Mapping();
    }

    private void State_renewal()
    {
        MaxHp = Str * 50;
        MaxMp = Int * 50;
        Atk = Str * 5 + Plus_Atk;
        Def = Luk * 5 + Plus_Def;
        Atk_Speed = Dex * 10;
        total_Exp = Lv * 100;
    }

    private void Mapping() // 맵핑함수
    {
        Health_Map = new Dictionary<string, float>();
        Health_Map.Add("MaxHp", MaxHp);
        Health_Map.Add("MaxMp", MaxMp);
        Health_Map.Add("Hp", Hp);
        Health_Map.Add("Mp", Mp);
        Health_Map.Add("Str", Str);
        Health_Map.Add("Dex", Dex);
        Health_Map.Add("Int", Int);
        Health_Map.Add("Luk", Luk);
        Health_Map.Add("MaxExp", total_Exp);
        Health_Map.Add("Exp", Exp);
        Health_Map.Add("Plus_Atk", Plus_Atk);
        Health_Map.Add("Plus_Def", Plus_Def);
        Health_Map.Add("Lv", Lv);
        Health_Map.Add("LestPlusStats", Lest_Plus_stats);
    }

    /// <summary>
    /// 스탯을 갱신할때 사용하는 함수
    /// </summary>
    public void Roboot() 
    {
        State_renewal();
        Mapping();
    }

    /// <summary>
    /// 맵변수의 값을 실제값에 넣어준다.
    /// </summary>
    public void inputValue()
    {
        Lv = (int)Health_Map["Lv"];
        Exp = (int)Health_Map["Exp"];
        Str = Health_Map["Str"];
        Dex = Health_Map["Dex"];
        Int = Health_Map["Int"];
        Luk = Health_Map["Luk"];
        Lest_Plus_stats = (int)Health_Map["LestPlusStats"];
    }
    public void spawn() // 몬스터 스폰시 체력을 채워주고 값을 넣어준다.
    {
        Hp = MaxHp;
        Mp = MaxMp;
        State_renewal();
        Mapping();
    }

    /// <summary>
    /// 레벨업시 함수
    /// </summary>
    public void LevelUp()
    {
        Exp -= total_Exp;
        Lv++;
        Lest_Plus_stats++;
        total_Exp += 100;
        Hp = MaxHp;
        Mp = MaxMp;
        Roboot();
        GameManager.Instance.Ui_Manager.Manager_State.Plus_Stats_Btk_OnOff(true); // 스탯창에 스탯을 추가할수있는 버튼을 킨다.
    }
}
