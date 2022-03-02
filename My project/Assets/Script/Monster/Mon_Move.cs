using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon_Move : MonoBehaviour
{
    public float Speed_rotate;  // 회전속도
    public float Speed_move;    // 이동속도


    public float Wait_Time;     // 회전 후 대기시간

    [HideInInspector] public bool AttackAble = false;    // 공격이 가능한지 아닌지 체크

    private Vector3 TargetMove; // 이동할 지점
    private float Time_Check;   // 대기시간 체크

    private void Awake()
    {
        TargetMove = GetComponentInParent<Mon_Spawn>().Return_RandomPos();
        Time_Check = 0f;
    }

    private void Update()
    {
        if (!AttackAble)
            Mob_Move_Idle();
        else
            Mob_Move_Fight();
    }

    /// <summary>
    /// 몬스터의 이동 기본상태
    /// </summary>
    private void Mob_Move_Idle()
    {
        float dis = Vector3.Distance(transform.position, TargetMove);
        if (dis < 0.01f)
        {
            TargetMove = transform.position;
            Time_Check += Time.deltaTime;
            if (Time_Check >= Wait_Time)
                TargetMove = GetComponentInParent<Mon_Spawn>().Return_RandomPos();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetMove, Time.deltaTime * Speed_move);
            GameManager.Instance.Obj_Fun.Rotate_Target(TargetMove, transform.gameObject, Speed_rotate);
        }
    }

    /// <summary>
    /// 몬스터가 싸우는 중의 이동
    /// </summary>
    private void Mob_Move_Fight()
    {
        Mon_Attack myAttack = GetComponent<Mon_Attack>();
        if (myAttack == null)
            return;
        else if (myAttack.Player == null)
            return;

        float dis = Vector3.Distance(transform.position, myAttack.Player.transform.position);
        if(dis > 0.5f)
        {
            TargetMove = myAttack.Player.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, TargetMove, Time.deltaTime * Speed_move);
            GameManager.Instance.Obj_Fun.Rotate_Target(TargetMove, transform.gameObject, Speed_rotate);
        }
        else
        {
            myAttack.Monster_Attack();
            GameManager.Instance.Obj_Fun.Rotate_Target(TargetMove, transform.gameObject, Speed_rotate);
        }
    }



}
