using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLine_Change : MonoBehaviour
{
    public void Change_OutLine(GameObject obj, float value)
    {
        if (obj.GetComponentInChildren<SkinnedMeshRenderer>().materials[1] == null)
            return;
        Debug.Log(obj.GetComponentInChildren<SkinnedMeshRenderer>().materials[1].name);
        Color OutLineColor = obj.GetComponentInChildren<SkinnedMeshRenderer>().materials[1].GetColor("_OutlineColor");
        OutLineColor.a = value / 255;
        obj.GetComponentInChildren<SkinnedMeshRenderer>().materials[1].SetColor("_OutlineColor", OutLineColor);
    }
}
