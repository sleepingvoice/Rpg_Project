using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_Function:MonoBehaviour
{
    public void Attack(GameObject Target,int Damge)
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
        Target.GetComponent<Animator>().SetBool("Die", true);
        Target.GetComponent<Obj_State>().Alive = false;
        yield return new WaitForSeconds(2f);
        if(Target.tag != "Player")
            Destroy(Target);
    }
    IEnumerator Demaged(GameObject Target)
    {
        if (Target.GetComponent<Animator>() == null)
            yield return null;
        Target.GetComponent<Animator>().SetBool("Def", true);
        Target.GetComponent<Obj_State>().Atk_Time = 0f;
        yield return new WaitForSeconds(0.5f);
        Target.GetComponent<Animator>().SetBool("Def", false);
    }
}
