using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_function : MonoBehaviour
{

    public static void MousePos(int MouseButton, Vector3 StartPos, ref Vector3 TargetPos)
    {
        if (Input.GetMouseButtonDown(MouseButton))
        {
            Ray ray = GameManager.Instance.mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000f))
            {
                TargetPos = PosCorrect(StartPos, hit.point);
            }
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
            return CorrectPos(StartPos, hit.transform.position);
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
}
