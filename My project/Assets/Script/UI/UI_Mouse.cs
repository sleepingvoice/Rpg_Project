using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UI_Mouse : MonoBehaviour
{
    public RawImage Empty_Icon; // 클릭시 마우스를 따라다닐 이미지


    private UI_Manager myManager;
    private Action Chasing_Icon;    // 아이콘이 따라다니는 액션
    private Action Chasing_Windows; // 윈도우가 따라다니는 액션
    private GraphicRaycaster gr;
    private bool Chasing_Icon_now; // 아이콘이 마우스를 따라다니는 중인지 여부파악
    private Texture Empty_texture; // 클릭시 인벤토리 창을 바꿔줄 텍스쳐
    private Vector3 mousePos;      // 처음 클릭시 마우스의 위치

    private void Awake()
    {
        gr = GetComponent<GraphicRaycaster>();
        Chasing_Icon_now = false;
        myManager = GameManager.Instance.Ui_Manage;
        Empty_texture = GameManager.Instance.Ui_Manage.Black_Item;
    }

    private void Update()
    {
        Click_Event();
        Chasing();
    }
    
    private void Chasing()
    {
        if (Chasing_Icon != null)
            Chasing_Icon();
        if (Chasing_Windows != null)
            Chasing_Windows();
    }

    private void Click_Event()
    {
        Click_left_Inventory();
        Click_Right_Inventory();
        Holding_Left_Mouse();
    }

    #region 왼쪽마우스클릭{
    /// <summary>
    /// 인벤토리가 비어있지 않은상태에서 마우스 왼쪽 클릭시 아이템이 마우스를 따라다니게되는 함수
    /// </summary>
    private void Click_left_Inventory()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Click_GUI_Check() == null)
                return;
            foreach (RaycastResult result in Click_GUI_Check())
            {
                if (result.gameObject.name == "Icon") // 만약 부딪힌 GUI에서 Icon이 존재할시에
                {
                    if (Chasing_Icon_now) // 아이콘이 마우스를 따라다니는 중이라면
                    {
                        if(result.gameObject.GetComponent<RawImage>().texture != Empty_texture) // 만약 클릭한 지점이 비어있는 상태가 아니라면
                        {
                            Texture tmp = Empty_Icon.texture;
                            Empty_Icon.texture = result.gameObject.GetComponent<RawImage>().texture;
                            result.gameObject.GetComponent<RawImage>().texture = tmp;               //클릭한 지점의 아이콘과 따라다니는 아이콘의 텍스쳐를 교환해준다.
                        }
                        else // 비어있는 상태라면
                        {
                            result.gameObject.GetComponent<RawImage>().texture = Empty_Icon.texture; // 비어있는 아이콘에 따라다니는 아이콘의 텍스쳐를 넣어준다
                            Empty_Icon.texture = Empty_texture;
                            Empty_Icon.gameObject.SetActive(false);
                            Chasing_Icon = null; //아이콘이 마우스를 따라다니는 것을 멈춘다.
                            Chasing_Icon_now = false;
                        }
                    }
                    else //따라다니는 중이 아니라면
                    {
                        Empty_Icon.texture = result.gameObject.GetComponent<RawImage>().texture; // 마우스를 따라다닐 아이콘의 텍스쳐를 바꿔준다.
                        result.gameObject.GetComponent<RawImage>().texture = Empty_texture;      // 인벤토리의 아이콘을 바꿔준다.
                        Empty_Icon.gameObject.SetActive(true);
                        Chasing_Icon = () => { Empty_Icon.rectTransform.position = Input.mousePosition; }; //아이콘이 마우스를 따라다니도록 Action에 추가해준다.
                        Chasing_Icon_now = true;
                    }
                }
            }
        }
    }

    /// <summary>
    /// UI를 마우스 왼쪽버튼을 꾹누를시 UI창이 따라가게 만드는 함수
    /// </summary>
    private void Holding_Left_Mouse()
    {
        if (Input.GetMouseButton(0))
        {
            if (Click_GUI_Check() == null)
                return;
            foreach (RaycastResult result in Click_GUI_Check()) //만약 아이콘을 클릭한 것이라면 함수를 빠져나온다.
            {
                if (result.gameObject.name == "Icon")
                    return;
            }
            foreach (RaycastResult result in Click_GUI_Check()) // 아이콘을 클릭하지 않았을경우
            {
                if ((result.gameObject == myManager.Manager_Equip.gameObject || result.gameObject == myManager.Manager_Inven.gameObject)
                    && Chasing_Windows == null) // 인벤토리나 장비창의 최상단을 잡고있고 창이 마우스를 따라다니지 않는 경우
                {
                    Debug.Log("들어왔다!");
                    RectTransform myWindow = result.gameObject.GetComponent<RectTransform>();
                    mousePos = Input.mousePosition;
                    Vector3 WindowPos = myWindow.position;
                    Chasing_Windows = () =>
                    {
                        Vector3 mousePlus = Input.mousePosition - mousePos;
                        myWindow.position = WindowPos + mousePlus;
                    };
                    return;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Chasing_Windows = null;
        }
    }

    #endregion

    #region 오른쪽 마우스 클릭
    /// <summary>
    /// 인벤토리의 아이템을 오른쪽 클릭했을때 아이템이 장비창에 장착되게 하는 함수
    /// </summary>
    private void Click_Right_Inventory()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Click_GUI_Check() == null)
                return;
            foreach (RaycastResult result in Click_GUI_Check())
            {
                if (result.gameObject.name == "Icon" && result.gameObject.GetComponent<RawImage>().texture != Empty_texture)
                //인벤토리의 아이템을 클릭할때
                //만약 오브젝트의 이름이 "Icon" 이고 클릭된 아이콘이 비어있지 않을경우
                {
                    string s = myManager.Find_Item_Code(result.gameObject.GetComponent<RawImage>().texture.name); // Icon에 있는 텍스쳐의 Item_code를 찾아 넣는다.
                    RawImage Equip_Img = myManager.Manager_Equip.Change_Equip_Item(s);                            // Item_code를 통해 알맞은 장비창의 아이콘을 찾는다.
                    if (Equip_Img != null) // 만약 알맞은 장비창이 존재할때
                    {
                        myManager.Manager_Equip.Change_Weapons(s,Equip_Img.name);

                        Texture Tmp = Equip_Img.texture;
                        Equip_Img.texture = result.gameObject.GetComponent<RawImage>().texture;
                        myManager.Manager_Equip.Plus_Item_State(s, 1); // 아이템의 능력치를 추가해준다.
                        if (Tmp != myManager.Black_Equip) // 장비창이 빈아이콘이 아닐때
                        {
                            result.gameObject.GetComponent<RawImage>().texture = Tmp;
                            s = myManager.Find_Item_Code(Tmp.name);
                            myManager.Manager_Equip.Plus_Item_State(s, -1); // 있던 아이템의 능력치를 빼준다.
                        }
                        else // 장비창이 빈 아이콘일때
                        {
                            result.gameObject.GetComponent<RawImage>().texture = myManager.Black_Item;
                            myManager.Manager_Equip.Hide_Icon_Name(Equip_Img.name, false); // 장비창 아래의 이름을 지워준다
                        }

                    }
                    return;
                }
                Equip_Out(result.gameObject);
            }
        }
    }
    #endregion

    #region 보조기능들
    /// <summary>
    /// 마우스 위치의 GUI를 알고 반환해주는 함수
    /// </summary>
    private List<RaycastResult> Click_GUI_Check()
    {
        var ped = new PointerEventData(null);
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);//GUI에 ray를 쏴서 부딪힌 GUI들을 results에 저장한다.

        if (results.Count <= 0) return null;
        return results;
    }


    /// <summary>
    /// 장비창에 있는 장비를 뺄때 사용하는 함수
    /// </summary>
    private void Equip_Out(GameObject Target)
    {
        foreach (RawImage Img in myManager.Manager_Equip.Equip_Icons)
        {
            if (Img.name == Target.name && Img.texture != myManager.Black_Equip)
            {
                string s = myManager.Find_Item_Code(Img.texture.name);
                foreach (RawImage raw in myManager.Manager_Inven.Icons)
                {
                    Debug.Log("들어옴");
                    if (raw.texture == myManager.Black_Item)
                    {

                        Texture tmp = Img.texture;
                        Img.texture = myManager.Black_Equip;
                        myManager.Manager_Equip.Plus_Item_State(s, -1); // 장비창에 끼고있는 아이템의 능력치를 빼준다.
                        myManager.Manager_Equip.Hide_Icon_Name(Img.name, true);
                        raw.texture = tmp;
                        return;
                    }
                }
            }
        }
    }
    #endregion
}
