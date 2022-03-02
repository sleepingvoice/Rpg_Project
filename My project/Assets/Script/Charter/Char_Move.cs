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
    private AudioSource Walk_Sound; // 걷는 소리

    private Char_Attack myAtk;


    private void Awake()
    {
        ani = GetComponent<Animator>();
        myAtk = GetComponent<Char_Attack>();
        Atk_now = false;
        Attack_num = 0;
    }

    private void Start()
    {
        Walk_Sound = GameManager.Instance.My_Sound.SE_Sound_Change("Walk");
    }

    public void Update()
    {
        Walk_To_Target(GameManager.Instance.TargetPos);
    }

    #region 움직임

    /// <summary>
    /// TagetPos로 이동하는 함수
    /// 일정거리이상이 되고 공격할 대상이 있다면 공격함수로 넘어간다.
    /// </summary>
    public void Walk_To_Target(Vector3 TargetPos)
    {
        if (TargetPos == transform.position)
            return;
        dis = Vector3.Distance(TargetPos, transform.position);
        walk = false;
        Atk_now = myAtk.Player_AttackCheck();

        if (dis>0.5f || (dis >= 0.05f && !myAtk.now_fight)) //거리가 0.5f이상으로 멀거나 거리가 0.05f보다 멀고 공격중이 아닐때
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPos, Time.deltaTime * Speed_move);
            walk = true;

            GameManager.Instance.Obj_Fun.Rotate_Target(TargetPos, this.gameObject, Speed_rotate);
        }
        else if(myAtk.now_fight && Atk_now) // 당장 공격해야할때
        {
            myAtk.Player_Attack();
        }
        else if(dis < 0.05f && !myAtk.now_fight) // 공격중이 아니고 거리가 0.05f보다 가까울때
        {
            transform.position = TargetPos;
        }
        

        Change_Ani();
    }

    /// <summary>
    /// 몬스터가 죽었을 때 행동을 정하는 함수
    /// </summary>
    public void Monster_Kill()
    {
        Atk_now = false;
        myAtk.now_fight = false;
        walk = false;
        GameManager.Instance.TargetPos = transform.position;
    }

    #endregion

    #region 애니메이션

    /// <summary>
    /// 애니메이션을 모아놓은 함수
    /// </summary>
    private void Change_Ani()  // 애니메이션 변경
    {
        WalkCheck();
        Attack_Check();
    }

    /// <summary>
    /// 걷는 애니메이션 체크
    /// </summary>
    private void WalkCheck()
    {
        if (walk && Attack_num == 0)
        {
            ani.SetBool("Walk", true);
            if (!Walk_Sound.isPlaying)
            {
                Walk_Sound.Play();
                Walk_Sound.loop = true;
            }
        }
        else
        {
            ani.SetBool("Walk", false);
            if (Walk_Sound.isPlaying)
            {
                Walk_Sound.Stop();
                Walk_Sound.loop = false;
            }   
        }
    }

    /// <summary>
    /// 공격 애니메이션 체크
    /// </summary>
    private void Attack_Check()
    {
        if (myAtk.now_fight)
            ani.SetInteger("Attack", Attack_num);
        else
        {
            Attack_num = 0;
            ani.SetInteger("Attack", 0);
        }
    }

    #endregion

}
