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
    public bool now_fight;          // 전투중인지 아닌지 판단

    public GameObject Monster { get { return nearMonster; } }

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

    #region 공격

    /// <summary>
    /// 플레이어의 공격 유무를 체크
    /// </summary>
    public bool Player_AttackCheck()
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
    
    /// <summary>
    /// 플레이어가 공격했을때의 행동
    /// </summary>
    public void Player_Attack()
    {
        Attack_delay = 0f;        // 공격속도를 초기화한다.
        Delay += delayTime;       // action 공격 시간을 측정하는 함수를 추가한다.
        GameManager.Instance.Obj_Fun.Attack(nearMonster, transform.gameObject);
        nearMonster.GetComponent<Mon_Attack>().Player = transform.gameObject;
        nearMonster.GetComponent<Mon_Move>().AttackAble = true;
        nearMonster.GetComponent<Mon_Attack>().FirstDelay();
        GameManager.Instance.My_Sound.SE_Sound_Change("Attack").Play(); // 공격하는 사운드를 출력한다.

        if (GetComponent<Char_Move>().Attack_num < 3)
            GetComponent<Char_Move>().Attack_num++;
        else
            GetComponent<Char_Move>().Attack_num = 1;
    }

    /// <summary>
    /// 공격 대기 시간
    /// </summary>
    private void delayTime()
    {
        if (Attack_delay <= my_state.Atk_Time)
            Attack_delay += Time.deltaTime;
    }

    #endregion
}
