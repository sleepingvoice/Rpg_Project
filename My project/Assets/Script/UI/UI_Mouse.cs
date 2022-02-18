using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UI_Mouse : MonoBehaviour
{
    public RawImage Empty_Icon; // 클릭시 마우스를 따라다닐 이미지
    public Texture Empty_texture; // 클릭시 인벤토리 창을 바꿔줄 텍스쳐

    private Action Chasing_Mouse;
    private GraphicRaycaster gr;
    private bool Chasing_Icon_now; // 아이콘이 마우스를 따라다니는 중인지 여부파악

    private void Awake()
    {
        gr = GetComponent<GraphicRaycaster>();
        Chasing_Icon_now = false;
    }

    private void Update()
    {
        Click_left_Inventory();

        if (Chasing_Mouse != null)
            Chasing_Mouse();
    }

    /// <summary>
    /// 인벤토리가 비어있지 않은상태에서 마우스 왼쪽 클릭시 아이템이 마우스를 따라다니게되는 함수
    /// </summary>
    public void Click_left_Inventory()
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

    private void Chase()
    {
        Empty_Icon.rectTransform.position = Input.mousePosition;
    }

}
