using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Move : MonoBehaviour
{
    public float Speed_move;
    public float Speed_rotate;

    [HideInInspector]public int Attack_num;      // 공격 애니메이션 번호

    private float dis;          // 타겟 위치와 플레이어의 거리
    private Animator ani;       // 플레이어 애니메이션
    private bool walk;          // 걷는중 유무체크
    private bool Atk_now;       // 싸우는중 유무체크

    private Char_Attack myAtk;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        myAtk = GetComponent<Char_Attack>();
        Atk_now = false;
        Attack_num = 0;
    }

    public void Update()
    {
        Walk_To_Target(GameManager.Instance.TargetPos);
    }

    public void Walk_To_Target(Vector3 TargetPos) // 타겟방향으로 이동
    {
        if (TargetPos == transform.position)
            return;
        dis = Vector3.Distance(TargetPos, transform.position);
        walk = false;
        Atk_now = myAtk.Player_AttackCheck();

        if (dis>0.5f || (dis >= 0.1f && !myAtk.now_fight))
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPos, Time.deltaTime * Speed_move);
            walk = true;

            GameManager.Instance.Obj_Fun.Rotate_Target(TargetPos, this.gameObject, Speed_rotate);
        }
        else if(myAtk.now_fight && Atk_now)
        {
            myAtk.Player_Attack();
        }
        

        Change_Ani();
    }

    public void Monster_Kill()
    {
        Atk_now = false;
        myAtk.now_fight = false;
        walk = false;
        GameManager.Instance.TargetPos = transform.position;
    }

    private void Change_Ani()  // 애니메이션 변경
    {
        WalkCheck();
        Attack_Check();
    }

    private void WalkCheck()  // 걷는 애니메이션 체크
    {
        if (walk && Attack_num == 0)
            ani.SetBool("Walk", true);
        else
            ani.SetBool("Walk", false);
    }

    private void Attack_Check() //공격 애니메이션 체크
    {
        if (myAtk.now_fight)
            ani.SetInteger("Attack", Attack_num);
        else
        {
            Attack_num = 0;
            ani.SetInteger("Attack", 0);
        }
    }

}
