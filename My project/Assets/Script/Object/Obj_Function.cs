using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_Function:MonoBehaviour
{
    public void Attack(GameObject Target,GameObject Attacker)
    {
        Obj_State State = Target.GetComponent<Obj_State>();
        if (State != null)
        {
            State.Hp -= Attacker.GetComponent<Obj_State>().Atk;
            State.Health_Map["Hp"] = State.Hp;
            if (State.Hp <= 0)
            {
                StartCoroutine(Die(Target));
                Kill(Attacker,Target);
            }
            else
                StartCoroutine(Demaged(Target));
        }
    }

    IEnumerator Die(GameObject Target)
    {
        if (Target.GetComponent<Animator>() == null)
            yield return null;
        Target.GetComponent<Animator>().SetTrigger("Die");
        Target.GetComponent<Obj_State>().Alive = false;
        yield return new WaitForSeconds(2f);
        if (Target.tag == "Monster")
            Target.GetComponentInParent<Mon_Spawn>().StartCoroutine("Die_Respawn",Target);
    }
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

    public void Kill(GameObject Attacker,GameObject Target)
    {
        if(Attacker.tag == "Player")
        {
            Attacker.GetComponent<Char_Move>().Monster_Kill();
            Attacker.GetComponent<Obj_State>().Exp += Target.GetComponent<Obj_State>().Exp;
        }
        else if(Attacker.tag == "Monster")
        {
            Attacker.GetComponent<Mon_Attack>().Player = null;
            Attacker.GetComponent<Mon_Attack>().Attack_delay = 0f;
            Attacker.GetComponent<Mon_Move>().AttackAble = false;
            Attacker.GetComponent<Obj_State>().spawn();
        }
    }

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


}
