using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Char_Rotate : MonoBehaviour
{
    private Vector3 TargetPos;
    public float Speed_rotate;

    private void Awake()
    {
        TargetPos = Vector3.zero;
    }

    public void Rotate_Check()
    {
        Char_function.MousePos(1, ref TargetPos);
        Rotate_Target(TargetPos, this.gameObject);
    }

    public void Rotate_Target(Vector3 targetPos,GameObject Obj)
    {
        Vector3 dir = targetPos - Obj.transform.position;
        dir.y = 0f;
        Quaternion targetRot = Quaternion.LookRotation(dir);
        Obj.GetComponent<Rigidbody>().rotation = Quaternion.RotateTowards(Obj.transform.rotation, targetRot, Speed_rotate * Time.deltaTime);
    }
}
