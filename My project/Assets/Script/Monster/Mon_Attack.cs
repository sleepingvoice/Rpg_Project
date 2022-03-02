using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mon_Attack : MonoBehaviour
{
     public GameObject Player;                   // 몬스터를 때린 플레이어
    
    public GameObject Range_Obj;                 // 공격 거리 체크 오브젝트
    public bool FirstAttack;                     // 선공 여부

    [HideInInspector]public float Attack_delay;                   // 공격 후 딜레이시간
    private Action Delay;                        // 딜레이 업데이트에 돌리는용도
    private Obj_State my_state;

    private void Awake()
    {
        Player = null;
        Attack_delay = 0f;
        my_state = GetComponent<Obj_State>();
    }

    private void Update()
    {
        if (Delay != null)
        {
            Delay();
            if (Attack_delay >= my_state.Atk_Time)
                Delay = null;
        }
        Attack_Check();
    }

    /// <summary>
    /// 몬스터의 공격
    /// </summary>
    public void Monster_Attack()
    {
        if (Player == null)
            return;

        if (Attack_delay < my_state.Atk_Time || Vector3.Distance(transform.position,Player.transform.position) > 0.5f)
            return;

        GameManager.Instance.Obj_Fun.Attack(Player, transform.gameObject);
        GetComponent<Animator>().SetTrigger("Attack");
        Delay += Deley_Time;
        Attack_delay = 0f;
    }


    /// <summary>
    /// 몬스터의 공격범위안에 있을경우 플레이어를 공격하고 
    /// 밖에 있을경우 어그로를 푸는 함수
    /// </summary>
    private void Attack_Check()
    {
        if (Range_Obj == null)
            Debug.LogError(transform.name + "의 공격범위 콜라이더 오브젝트가 빠졌습니다!");

        Mon_AttackRange Range = Range_Obj.GetComponent<Mon_AttackRange>();

        //선공 체크
        if (FirstAttack)
        {
            if (Range.PlayerOnOff)
            {
                Player = Range.Player;
                GetComponent<Mon_Move>().AttackAble = true;
            }
        }

        //플레이어가 범위밖으로 나갔을때
        if (Player != null && !Range.PlayerOnOff)
        {
            Player = null;
            GetComponent<Mon_Move>().AttackAble = false;
        }
    }

    public void FirstDelay()
    {
        Delay += Deley_Time;
    }

    private void Deley_Time()
    {
        if (Attack_delay <= my_state.Atk_Time)
            Attack_delay += Time.deltaTime;
    }


}
