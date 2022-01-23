using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_function : MonoBehaviour
{
    public static void MousePos(Vector3 StartPos, ref Vector3 TargetPos)
    {
        int layerMask = 1 << LayerMask.NameToLayer("Wall");
        Ray ray = GameManager.Instance.mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10000f, layerMask))
        {
            TargetPos = PosCorrect(StartPos, hit.point);
        }
    }

    private static Vector3 PosCorrect(Vector3 StartPos ,Vector3 TargetPos)
    {
        TargetPos.y = StartPos.y;
        Vector3 Dir = TargetPos - StartPos;
        Dir.y = 1f;
        float Dis = Vector3.Distance(TargetPos, StartPos);
        Dir = Dir.normalized;
        RaycastHit hit;

        if (Physics.Raycast(StartPos, Dir, out hit, Dis))
        {
            if (hit.transform.gameObject.layer == 10)
            {
                return CorrectPos(StartPos, hit.transform.position);
            }
        }
        return TargetPos;
    }

    private static Vector3 CorrectPos(Vector3 firstPos,Vector3 HitPos)
    {
        Vector3 correct_pos;
        HitPos.y = firstPos.y;
        Vector3 dir = HitPos - firstPos;
        dir *= 0.9f;
        correct_pos = firstPos + dir;

        return correct_pos;
    }

    public static GameObject Click_Obj(Vector3 TargetPos , string Target_Tag)
    {
        Collider[] colls = Physics.OverlapSphere(TargetPos, 0.5f);
        GameObject nearObj = null;
        float dis = 2f;
        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].gameObject.tag == Target_Tag)
            {
                float TargetDis = Vector3.Distance(colls[i].transform.position, GameManager.Instance.Player.transform.position);
                if (TargetDis < dis)
                {
                    dis = TargetDis;
                    nearObj = colls[i].gameObject;
                }
            }
        }
        return nearObj;
    }

}
