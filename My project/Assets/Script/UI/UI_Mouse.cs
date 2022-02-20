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
    private Action Chasing_Mouse;
    private GraphicRaycaster gr;
    private bool Chasing_Icon_now; // 아이콘이 마우스를 따라다니는 중인지 여부파악
    private Texture Empty_texture; // 클릭시 인벤토리 창을 바꿔줄 텍스쳐

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
        if (Chasing_Mouse != null)
            Chasing_Mouse();
    }

    private void Click_Event()
    {
        Click_left_Inventory();
        Click_Right_Inventory();
    }
    /// <summary>
    /// 인벤토리가 비어있지 않은상태에서 마우스 왼쪽 클릭시 아이템이 마우스를 따라다니게되는 함수
    /// </summary>
    private void Click_left_Inventory()
    {
        if (Input.GetMouseButtonDown(0))
        {
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
                            Chasing_Mouse = null; //아이콘이 마우스를 따라다니는 것을 멈춘다.
                            Chasing_Icon_now = false;
                        }
                    }
                    else //따라다니는 중이 아니라면
                    {
                        Empty_Icon.texture = result.gameObject.GetComponent<RawImage>().texture; // 마우스를 따라다닐 아이콘의 텍스쳐를 바꿔준다.
                        result.gameObject.GetComponent<RawImage>().texture = Empty_texture;      // 인벤토리의 아이콘을 바꿔준다.
                        Empty_Icon.gameObject.SetActive(true);
                        Chasing_Mouse += Chase; //아이콘이 마우스를 따라다니도록 Action에 추가해준다.
                        Chasing_Icon_now = true;
                    }
                }
            }
        }
    }

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
    /// 인벤토리의 아이템을 오른쪽 클릭했을때 아이템이 장비창에 장착되게 하는 함수
    /// </summary>
    private void Click_Right_Inventory()
    {
        if (Input.GetMouseButtonDown(1))
        {
            foreach (RaycastResult result in Click_GUI_Check())
            {
                if (result.gameObject.name == "Icon" && result.gameObject.GetComponent<RawImage>().texture != Empty_texture) 
                    //인벤토리의 아이템을 클릭할때
                    //만약 오브젝트의 이름이 "Icon" 이고 클릭된 아이콘이 비어있지 않을경우
                {
                    string s = myManager.Find_Item_Code(result.gameObject.GetComponent<RawImage>().texture.name); // Icon에 있는 텍스쳐의 Item_code를 찾아 넣는다.
                    RawImage Equip_Img = myManager.Manager_Equip.Change_Equip_Item(s);                            // Item_code를 통해 알맞은 장비창의 아이콘을 찾는다.
                    if(Equip_Img != null) // 만약 알맞은 장비창이 존재할때
                    {
                        Texture Tmp = Equip_Img.texture;
                        Equip_Img.texture = result.gameObject.GetComponent<RawImage>().texture;
                        if (Tmp != myManager.Manager_Equip.Blank_tex) // 장비창이 빈아이콘이 아닐때
                        {
                            result.gameObject.GetComponent<RawImage>().texture = Tmp;
                        }
                        else // 장비창이 빈 아이콘일때
                        {
                            result.gameObject.GetComponent<RawImage>().texture = myManager.Black_Equip;
                        }
                    }
                }
            }
        }
    }

    private void Chase()
    {
        Empty_Icon.rectTransform.position = Input.mousePosition;
    }

}
