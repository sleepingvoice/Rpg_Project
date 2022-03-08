using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    [Header("Player 관련")]
    public Camera mainCam;
    public GameObject Player;

    [Header("UI 관련")]
    public UI_Manager Ui_Manager;
    public SoundManager My_Sound;
    public Npc_Manager npc_manager;

    [HideInInspector] public Vector3 TargetPos;
    [HideInInspector] public Obj_Function Obj_Fun;
    [HideInInspector] public User_Info Info; // 유저정보를 가진 오브젝트
    [HideInInspector] public OutLine_Change OutLine; // 아웃라인 관련 스크립트
    [HideInInspector]public Save_Data My_Save;

    public Button Save;

    private GameObject Effect_Click;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        TargetPos = Player.transform.position;
        Obj_Fun = GetComponent<Obj_Function>();
        OutLine = GetComponent<OutLine_Change>();

        Save.onClick.AddListener(() => Ui_Manager.Manager_Inven.Save_Inventory());
        Save.onClick.AddListener(() => Ui_Manager.Manager_Setting.Save_Setting_Value());

        if (User_Info.Instance != null)
        {
            Info = User_Info.Instance;
            My_Save = Info.save;
            Save.onClick.AddListener(() => My_Save.Save_UserData());
            Save.onClick.AddListener(() => Application.Quit());
        }

        
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

    /// <summary>
    /// TargetPos에 이펙트 발생
    /// </summary>
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
