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
        nearMonster = Char_function.Click_Obj(GameManager.Instance.TargetPos, "Monster"); // 클릭한 지점의 Monster 라는 tag를 가진 오브젝트를 반환

        if (nearMonster != null) // 만약 오브젝트가 존재한다면
        {
            now_fight = true;

            Vector3 TmpPos = nearMonster.transform.position;
            TmpPos.y = GameManager.Instance.Player.transform.position.y;


            GameManager.Instance.TargetPos = TmpPos;
            GameManager.Instance.OutLine.Change_OutLine(nearMonster, 150f); // 공격하는 몬스터의 테두리를 생성한다.
            GameManager.Instance.OutLine.Add_OutLineList(nearMonster);

            float dis = Vector3.Distance(nearMonster.transform.position, transform.position);

            if(dis <= 0.8f && Attack_delay >= my_state.Atk_Time) // 만약 거리가 가깝고 플레이어가 공격이 가능하다면
                return true;

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
        nearMonster.GetComponent<Mon_Move>().nowState = Mon_Move.State.Fight;
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
