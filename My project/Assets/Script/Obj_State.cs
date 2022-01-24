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

    [Header("레벨")]
    public int Exp;        // 경험치
    public int Lv;         // 레벨

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
    }

    public void spawn()
    {
        Hp = MaxHp;
        Mp = MaxMp;
    }
}
