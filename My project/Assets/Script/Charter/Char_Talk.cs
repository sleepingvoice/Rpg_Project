using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Talk : MonoBehaviour
{
    private GameObject nearNPC;         // 클릭한 NPC
    public GameObject NPC { get { return nearNPC; } }

    private void Awake()
    {
        nearNPC = null;
    }

    public void Talk_NPC()
    {
        nearNPC = Char_function.Click_Obj(GameManager.Instance.TargetPos, "NPC"); // 클릭한 지점의 Npc 라는 tag를 가진 오브젝트를 반환
        if (nearNPC != null)
        {
            GameManager.Instance.TargetPos = nearNPC.transform.position;
            GameManager.Instance.OutLine.Change_OutLine(nearNPC, 150f); // 공격하는 몬스터의 테두리를 생성한다.
            GameManager.Instance.OutLine.Add_OutLineList(nearNPC);
        }
    }
}
