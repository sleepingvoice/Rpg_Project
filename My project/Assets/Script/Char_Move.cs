using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Move : MonoBehaviour
{
    public float Speed_move;
    public float Speed_rotate;

    private Vector3 TargetPos;
    private Rigidbody rigid;
    private Animator ani;
    private bool walk;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        TargetPos = transform.position;
    }

    public void Update()
    {
        Char_function.MousePos(1, transform.position ,ref TargetPos);

        Walk_To_Target(TargetPos);
    }

    public void Walk_To_Target(Vector3 TargetPos)
    {
        float dis = Vector3.Distance(TargetPos, transform.position);
        walk = false;
        if (dis>=0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPos, Time.deltaTime*Speed_move);
            walk = true;

            Rotate_Target(TargetPos, this.gameObject);
        }
        Change_Ani();
    }

    public void Change_Ani()
    {
        if (walk)
            ani.SetBool("Walk", true);
        else
            ani.SetBool("Walk", false);
    }

    public void Rotate_Target(Vector3 targetPos, GameObject Obj)
    {
        Vector3 dir = targetPos - Obj.transform.position;
        dir.y = 0f;
        Quaternion targetRot = Quaternion.LookRotation(dir);
        Obj.GetComponent<Rigidbody>().rotation = Quaternion.RotateTowards(Obj.transform.rotation, targetRot, Speed_rotate * Time.deltaTime);
    }

}
