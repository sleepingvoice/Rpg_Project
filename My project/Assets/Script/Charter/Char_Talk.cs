using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Talk : MonoBehaviour
{
    private GameObject nearNPC;         // 클릭한 NPC
    public GameObject NPC { get { return nearNPC; } }
    private GameManager myGameManager;

    private void Awake()
    {
        nearNPC = null;
        myGameManager = GameManager.Instance;
    }

    public void Talk_NPC()
    {
        nearNPC = Char_function.Click_Obj(GameManager.Instance.TargetPos, "NPC"); // 클릭한 지점의 Npc 라는 tag를 가진 오브젝트를 반환
        if (nearNPC != null)
        {
            Vector3 dir = myGameManager.Player.transform.position - nearNPC.transform.position; // npc로부터 플레이어의 방향
            dir = dir.normalized; // 정규화
            myGameManager.TargetPos = nearNPC.transform.position + dir*0.5f; // npc로부터 0.5f만큼 떨어진 위치를 타겟팅한다.
            myGameManager.OutLine.Change_OutLine(nearNPC, 150f); // 공격하는 몬스터의 테두리를 생성한다.
            myGameManager.OutLine.Add_OutLineList(nearNPC);
            myGameManager.npc_manager.Npc_box_setting(nearNPC.name);
        }
    }
}
