using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon_Move : MonoBehaviour
{
    public float Speed_rotate;  // 회전속도
    public float Speed_move;    // 이동속도

    public float ResapwnTime;   // 리젠시간
    public float Wait_Time;     // 회전 후 대기시간

    private Vector3 TargetMove; // 이동할 지점
    private float Time_Check;   // 대기시간 체크

    private void Awake()
    {
        TargetMove = GetComponentInParent<Mon_Spawn>().Return_RandomPos();
        Time_Check = 0f;
    }

    private void Update()
    {
        Mob_Move();
    }

    private void Mob_Move()
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

    public void Die_Active()
    {
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        gameObject.SetActive(false);

        yield return new WaitForSeconds(ResapwnTime);

        GetComponent<Animator>().SetTrigger("Spawn");
        transform.position = GetComponentInParent<Mon_Spawn>().Return_RandomPos(); // 생성위치를 랜덤하게 바꿈
        gameObject.SetActive(true);
        GetComponent<Obj_State>().spawn();
    }


}
