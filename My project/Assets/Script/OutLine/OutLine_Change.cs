using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLine_Change : MonoBehaviour
{
    private List<GameObject> OutLineList = new List<GameObject>();

    /// <summary>
    /// obj의 테두리에 value값을 alpha값으로 가진 검은 테투리가 생성
    /// </summary>
    public void Change_OutLine(GameObject obj, float value)
    {
        if (obj.GetComponentInChildren<SkinnedMeshRenderer>().materials[1] == null) // 만약 테두리가 없는 오브젝트라면 함수를 반환한다.
            return;
        Color OutLineColor = obj.GetComponentInChildren<SkinnedMeshRenderer>().materials[1].GetColor("_OutlineColor"); // 테두리의 색을 가져온다.
        OutLineColor.a = value / 255;
        obj.GetComponentInChildren<SkinnedMeshRenderer>().materials[1].SetColor("_OutlineColor", OutLineColor);
    }

    public void Add_OutLineList(GameObject obj)
    {
        OutLineList.Add(obj);
    }
    
    public void Remove_All_OutLineList()
    {
        OutLineList = new List<GameObject>();
    }

    public void Changed_Value_OutLineList(float value)
    {
        foreach(GameObject obj in OutLineList)
        {
            Change_OutLine(obj, value);
        }
    }
}
