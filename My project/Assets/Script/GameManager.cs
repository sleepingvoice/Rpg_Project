using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public Camera mainCam;
    public GameObject Player;
    public Vector3 TargetPos;

    [HideInInspector]public Obj_Function Obj_Fun;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        TargetPos = Player.transform.position;
        Obj_Fun = GetComponent<Obj_Function>();
    }
    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Char_function.MousePos(Player.transform.position, ref TargetPos);
        }
    }


}
