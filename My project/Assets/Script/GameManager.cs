using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public Camera mainCam;
    public GameObject Player;
    public UI_Manager Ui_Manage;
    public Save_Data My_Save;

    [HideInInspector] public Vector3 TargetPos;
    [HideInInspector] public Obj_Function Obj_Fun;
    [HideInInspector] public User_Info Info; // 유저정보를 가진 오브젝트

    private GameObject Effect_Click;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        TargetPos = Player.transform.position;
        Obj_Fun = GetComponent<Obj_Function>();
        Info = User_Info.Instance;
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
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject()) // 오른쪽 마우스를 클릭하고 클릭한 위치에 UI가 없을때
        {
            Char_function.MousePos(Player.transform.position, ref TargetPos);
            StartCoroutine("Effect_Make", TargetPos);
        }
    }

    private IEnumerator Effect_Make(Vector3 TargetPos)
    {
        Vector3 Position = TargetPos + new Vector3(0, 0.4f, 0);
        if (Effect_Click == null)
        {
            GameObject Effect = Resources.Load<GameObject>("Prefebs/Effect/Click_Effect");
            Effect_Click = Instantiate(Effect, Position, Quaternion.Euler(-90f, 0, 0));
        }
        else
        {
            Effect_Click.SetActive(true);
            Effect_Click.transform.position = Position;
            Effect_Click.GetComponent<ParticleSystem>().Play();
        }
        yield return new WaitForSeconds(1f);
        Effect_Click.GetComponent<ParticleSystem>().Stop();
        Effect_Click.SetActive(false);
    }


}
