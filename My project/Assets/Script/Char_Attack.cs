using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Char_Attack : MonoBehaviour
{

    private float Attack_delay;     // 공격 가능한가 판단
    private Obj_State my_state;     // 나의 상태창 가져오기
    private Action Delay;           // 딜레이 업데이트에 돌리는용도
    private GameObject nearMonster; // 공격 대상 몬스터
    public bool now_fight;         // 전투중인지 아닌지 판단


    private void Awake()
    {
        Attack_delay = 1000f;
        my_state = GetComponent<Obj_State>();
        nearMonster = null;
        now_fight = false;
    }

    private void Update()
    {

        if (Delay != null)
        {
            Delay();
            if (Attack_delay >= my_state.Atk_Time)
                Delay = null;
        }
    }

    public bool Player_AttackCheck() // 플레이어가 공격 가능해야하는지를 체크
    {
        nearMonster = Char_function.Click_Obj(GameManager.Instance.TargetPos, "Monster");

        if (nearMonster != null)
        {
            now_fight = true;


            GameManager.Instance.TargetPos = nearMonster.transform.position;


            float dis = Vector3.Distance(nearMonster.transform.position, transform.position);
            if(dis <= 0.5f && Attack_delay >= my_state.Atk_Time)
            {
                return true;
            }
            return false;
        }
        now_fight = false;
        return false;
    }
    
    public void Player_Attack()
    {
        Attack_delay = 0f;
        Delay += delayTime;
        GameManager.Instance.Obj_Fun.Attack(nearMonster, transform.gameObject);
        nearMonster.GetComponent<Mon_Attack>().Player = transform.gameObject;
        nearMonster.GetComponent<Mon_Move>().AttackAble = true;
        nearMonster.GetComponent<Mon_Attack>().FirstDelay();

        if (GetComponent<Char_Move>().Attack_num < 3)
            GetComponent<Char_Move>().Attack_num++;
        else
            GetComponent<Char_Move>().Attack_num = 1;
    }

    private void delayTime()
    {
        if (Attack_delay <= my_state.Atk_Time)
            Attack_delay += Time.deltaTime;
    }
}
