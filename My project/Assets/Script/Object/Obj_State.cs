using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_State : MonoBehaviour
{
    [HideInInspector] public float Hp; // 체력
    [HideInInspector] public float Mp; // 마력
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

    private void Awake()
    {
        MaxHp = Str * 50;
        MaxMp = Int * 50;
        Atk = Str * 5;
        Def = Luk * 5;
        Atk_Speed = Dex * 10;
        Alive = true;
        Mapping();
    }

    private void Mapping()
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
    }

    public void spawn()
    {
        Hp = MaxHp;
        Mp = MaxMp;
        Mapping();
    }
}
