using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_Function:MonoBehaviour
{
    public void Attack(GameObject Target,float Damge)
    {
        Obj_State State = Target.GetComponent<Obj_State>();
        if (State != null)
        {
            State.Hp -= Damge;
            if (State.Hp <= 0)
                StartCoroutine(Die(Target));
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
            Target.GetComponent<Mon_Move>().Die_Active();
    }
    IEnumerator Demaged(GameObject Target)
    {
        if (Target.GetComponent<Animator>() == null)
            yield return null;
        Target.GetComponent<Animator>().SetBool("Def", true);
        Target.GetComponent<Obj_State>().Atk_Time = 0f;
        yield return new WaitForSeconds(1f);
        Target.GetComponent<Animator>().SetBool("Def", false);
    }

    public void Rotate_Target(Vector3 targetPos, GameObject Obj, float Speed_rotate) // 타겟 방향으로 회전
    {
        Vector3 dir = targetPos - Obj.transform.position;
        dir.y = 0f;
        Quaternion targetRot = Quaternion.LookRotation(dir);
        Obj.transform.rotation = Quaternion.RotateTowards(Obj.transform.rotation, targetRot, Speed_rotate * Time.deltaTime);
    }
}
