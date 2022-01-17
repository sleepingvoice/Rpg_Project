using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Char_function
{ 
    public static void MousePos(int MouseButton, ref Vector3 TargetPos)
    {
        if (Input.GetMouseButtonDown(MouseButton))
        {
            Ray ray = GameManager.Instance.mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000f))
                TargetPos = hit.point;
        }
    }
}
