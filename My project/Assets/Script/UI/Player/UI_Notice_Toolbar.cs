using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UI_Notice_Toolbar : MonoBehaviour
{
    public TextMeshProUGUI Title; // 제목
    public GameObject Window;     // 공지창

    private string Window_Title;
    private string Window_Content;
    private string Window_Date;



    /// <summary>
    /// 윈도우 값 세팅
    /// </summary>
    public void Set_Window_Values(Notice Tmp)
    {
        Window_Title = Tmp.title;
        Window_Content = Tmp.content;
        Window_Date = Tmp.date;

    }

    public void SetButton()
    {
        GetComponent<Button>().onClick.AddListener(() => click_event());
    }

    public void click_event()
    {
        Window.gameObject.SetActive(true);
        UI_Notice_Window myWindow = Window.GetComponent<UI_Notice_Window>();
        myWindow.Title.text = Window_Title;
        myWindow.Content.text = Window_Content;
        myWindow.Date.text = Window_Date;
    }
}
