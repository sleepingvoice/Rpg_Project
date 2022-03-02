using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_function : MonoBehaviour
{
    #region 위치보정

    /// <summary>
    /// 플레이어의 이동위치를 보정해주는 함수
    /// 마우스의 클릭위치를 보정하여 반환한다.
    /// </summary>
    public static void MousePos(Vector3 StartPos, ref Vector3 TargetPos)
    {
        int layerMask = 1 << LayerMask.NameToLayer("Wall");
        Ray ray = GameManager.Instance.mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10000f, layerMask)) // 카메라로부터 레이를 쏴서 벽이 맞는다면
        {
            TargetPos = PosCorrect(StartPos, hit.point);
        }
    }

    /// <summary>
    /// 시작 위치와 타겟위치를 판정하여 중간에 걸리는 것이 있다면 
    /// 걸리는 것의 앞으로 이동하는 함수
    /// </summary>
    private static Vector3 PosCorrect(Vector3 StartPos ,Vector3 TargetPos)
    {
        TargetPos.y = StartPos.y;
        Vector3 Dir = TargetPos - StartPos;
        Dir.y = 1f;
        float Dis = Vector3.Distance(TargetPos, StartPos); // 플레이어의 위치에서 타겟의 위치까지의 방향
        Dir = Dir.normalized;
        RaycastHit hit;

        if (Physics.Raycast(StartPos, Dir, out hit, Dis)) // 플레이어의 위치에서 목표한 지점 방향에 물체가 존재한다면
        {
            if (hit.transform.gameObject.layer == 10) // 만약 물체가 Wall 이면
            {
                return CorrectPos(StartPos, hit.transform.position);
            }
        }
        return TargetPos;
    }

    /// <summary>
    /// 클릭한 위치를 보정하는 함수
    /// </summary>
    private static Vector3 CorrectPos(Vector3 firstPos,Vector3 HitPos)
    {
        Vector3 correct_pos;
        HitPos.y = firstPos.y; // y좌표는 유지한다.
        Vector3 dir = HitPos - firstPos;
        dir *= 0.9f;
        correct_pos = firstPos + dir;  // 두 좌표 사이의 거리의 0.9배인 지점을 나타낸다

        return correct_pos;
    }

    #endregion

    #region 물체 확인

    /// <summary>
    /// 클릭한 지점에 있는 물체중 Tag를 가지고 있는 물체를 반환하는 함수
    /// </summary>
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

    #endregion

}
