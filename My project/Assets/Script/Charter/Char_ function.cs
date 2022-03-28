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
        int layerMask = 1 << LayerMask.NameToLayer("Floor");
        Ray ray = GameManager.Instance.mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 10000f, layerMask)) // 카메라로부터 레이를 쏴서 땅이 맞는다면
        {
            TargetPos = PosCorrect(StartPos, hit.point);
        }
    }
    
    /// <summary>
    /// 클릭한 지점과 플레이어 사이에 물체가 있으면 물체의 앞으로 위치를 보정하는 함수
    /// </summary>
    private static Vector3 PosCorrect(Vector3 StartPos, Vector3 TargetPos)
    {
        float tmpY = StartPos.y; // 원래 높이를 저장

        TargetPos.y = 0.5f; // 원래 높이로 레이를 쏘게되면 바닥으로 쏘게 되기 때문에 플레이어의 중심점 높이로 보정시켜준다.
        StartPos.y = 0.5f;
        Vector3 Dir = TargetPos - StartPos;
        Dir = Dir.normalized;
        float Dis = Vector3.Distance(TargetPos, StartPos);
        int layerMask = ((1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("Monster"))); // 바닥을 제외한 나머지 레이어
        layerMask = ~layerMask;
        if (Physics.Raycast(StartPos, Dir, out RaycastHit hit, Dis, layerMask)) {
            // 플레이어의 위치에서 목표한 지점 방향으로 ray를 쏴서 다른 물체를 탐색한다.
            Debug.Log(hit.collider.gameObject.name);
            Vector3 Dir_hit = StartPos - hit.point;
            Dir_hit = Dir_hit.normalized * 0.2f;
            Vector3 tmpVector = hit.point + Dir_hit;
            tmpVector.y = tmpY;
            return tmpVector;
        }
        TargetPos.y = tmpY;
        return TargetPos; // 만약 레이를 쏴서 맞는 물체가 없다면 원래 값에서 Y를 보정한 값을 반환한다.

    }

    #endregion

    #region 물체 확인

    /// <summary>
    /// 클릭한 지점에 있는 물체중 Tag를 가지고 있는 물체를 반환하는 함수
    /// </summary>
    public static GameObject Click_Obj(Vector3 TargetPos , string Target_Tag)
    {
        Collider[] colls = Physics.OverlapSphere(TargetPos, 0.1f);
        GameObject nearObj = null;
        float dis = 2f;
        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].gameObject.CompareTag(Target_Tag))
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
