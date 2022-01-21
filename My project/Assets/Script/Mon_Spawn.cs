﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon_Spawn : MonoBehaviour
{
    public float Correct_y;
    public float ReSpawnTime;
    [SerializeField]private string Folder_Name;
    private BoxCollider Range_Col;
    private GameObject[] Monster_Prefeb;
    

    private void Awake()
    {
        Range_Col = GetComponent<BoxCollider>();
        Spawn();
    }


    public Vector3 Return_RandomPos()
    {
        Vector3 OriginPos = transform.position;
        
        float range_X = Range_Col.bounds.size.x;
        float range_Z = Range_Col.bounds.size.z;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
        Vector3 RandomPostion = new Vector3(range_X, Correct_y, range_Z); // 박스콜라이더안의 x와 z 좌표를 대입시켜줌

        Vector3 respawnPosition = OriginPos + RandomPostion; 
        return respawnPosition;
    }

    private void Spawn()
    {
        Monster_Prefeb = Resources.LoadAll<GameObject>("Prefebs/Monster/" + Folder_Name);
        for (int i = 0; i < Monster_Prefeb.Length; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Vector3 Pos = Return_RandomPos();
                GameObject Prefeb = Instantiate(Monster_Prefeb[i], Pos, Quaternion.Euler(0, 180f, 0), transform);
                Prefeb.GetComponent<Mon_Move>().ResapwnTime = ReSpawnTime;
                Prefeb.GetComponent<Obj_State>().spawn();
            }
        }
    }
}
