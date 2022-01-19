using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_State : MonoBehaviour
{
    public float Hp; // 체력
    public float Mp; // 마력
    public float Atk;// 공격
    public float Def;// 방어
    public float Atk_Speed;//공속
    public float Atk_Time; //공격시간
    public bool Alive;     //생존유무

    private float MaxHp;
    private float MaxMp;

    public float Str;// 힘
    public float Dex;// 민
    public float Int;// 지
    public float Luk;// 운

    private void Awake()
    {
        MaxHp = Str * 50;
        MaxMp = Int * 50;
        Atk = Str * 5;
        Def = Luk * 5;
        Atk_Speed = Dex * 10;
        Alive = true;
    }
}
