using System.Collections;
using UnityEngine;

public class Obj_Function:MonoBehaviour
{
    #region 주요함수

    /// <summary>
    /// Target을 몬스터가 공격하는 함수
    /// </summary>
    public void Attack(GameObject Target,GameObject Attacker)
    {
        Obj_State State = Target.GetComponent<Obj_State>();
        if (State != null) // 만약 타겟에게 스텟이 있다면
        {
            State.Hp -= Attacker.GetComponent<Obj_State>().Atk; // 타겟의 체력을 공격자의 공격력만큼 뺀다
            State.Health_Map["Hp"] = State.Hp;                  // 타겟의 체력을 딕셔너리에 추가한다.
            if (State.Hp <= 0)                                  // 만약 타겟의 체력이 0보다작다면
            {
                StartCoroutine(Die(Target));
                Kill(Attacker,Target);
            }
            else
                StartCoroutine(Demaged(Target)); // 만약 체력이 0보다 크다면 타겟에게 데미지를 주는 코루틴을 실행한다.
        }
    }
    
    #endregion

    #region 보조함수

    /// <summary>
    /// Tagert에게 데미지를 추가해주는 코루틴
    /// </summary>
    IEnumerator Demaged(GameObject Target)
    {
        if (Target.GetComponent<Animator>() == null)
            yield return null;
        Target.GetComponent<Animator>().SetTrigger("Def");
        GameManager.Instance.My_Sound.SE_Sound_Change("Damaged").Play(); // 공격받는 소리를 추가한다.
    }

    /// <summary>
    /// 몬스터가 죽을때 작용
    /// </summary>
    IEnumerator Die(GameObject Target)
    {
        if (Target.GetComponent<Animator>() == null)       // 만약 애니메이터가 없다면 반환한다.
            yield return null;
        Target.GetComponent<Animator>().SetTrigger("Die"); // Die트리거를 실행한다.
        Target.GetComponent<Obj_State>().Alive = false;    // 스탯에 생존유무를 체크한다.
        yield return new WaitForSeconds(2f);               // Die 애니메이션이 실행하는동안 기다린후
        if (Target.tag == "Monster")
            Target.GetComponentInParent<Mon_Spawn>().StartCoroutine("Die_Respawn", Target); // Target의 죽은후 리스폰을 실행시킨다.
    }

    /// <summary>
    /// 몬스터를 잡았을경우 플레이어의 경험치를 추가해주는 함수
    /// 만약 몬스터가 플레이어를 죽였을경우 몬스터의 타겟팅을 취소하고 체력을 모두 채워준다.
    /// </summary>
    public void Kill(GameObject Attacker,GameObject Target)
    {
        if(Attacker.tag == "Player")
        {
            Attacker.GetComponent<Char_Move>().Monster_Kill();
            Attacker.GetComponent<Obj_State>().Exp += Target.GetComponent<Obj_State>().Exp;
            GameManager.Instance.Ui_Manager.Manager_Inven.AddMoney(100);
        }
        else if(Attacker.tag == "Monster")
        {
            Attacker.GetComponent<Mon_Attack>().Player = null;
            Attacker.GetComponent<Mon_Attack>().Attack_delay = 0f;
            Attacker.GetComponent<Mon_Move>().AttackAble = false;
            Attacker.GetComponent<Obj_State>().spawn();
        }
    }

    /// <summary>
    /// 타겟의 방향으로 회전시켜주는 함수
    /// </summary>
    public void Rotate_Target(Vector3 targetPos, GameObject Obj, float Speed_rotate) // 타겟 방향으로 회전
    {
        Vector3 dir = targetPos - Obj.transform.position;
        dir.y = 0f;
        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            Obj.transform.rotation = Quaternion.RotateTowards(Obj.transform.rotation, targetRot, Speed_rotate * Time.deltaTime);
        }
    }

    #endregion
}
