using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Notice
{
    public int Num;
    public string title;
    public string content;
    public string date;
}

public class UI_Notice : MonoBehaviour
{
    private List<Notice> nowNotice = new List<Notice>();

    public Transform Content;      // 윈도우 프리펩의 부모객체
    public GameObject WindowPrefeb; // 윈도우안의 내용
    public GameObject Window;      // 윈도우
    

    /// <summary>
    /// 공지사항 불러오는 코루틴
    /// </summary>
    public IEnumerator Notice_Text()
    {
        UnityWebRequest TakeNotice = UnityWebRequest.Get("http://localhost/Test/Notice.php");

        yield return TakeNotice.SendWebRequest();

        string s = TakeNotice.downloadHandler.text;
        List<string> nowNotice_text = Divide_Notice_Num(s.Trim());

        for(int i=0;i<nowNotice_text.Count;i++)
        {
            Deivide_Notice_Text(nowNotice_text[i]);
        }
        Create_Notice_Window();
    }

    /// <summary>
    /// 불러온 공지사항을 순서대로 나누어 주는 함수
    /// </summary>
    private List<string> Divide_Notice_Num(string s)
    {
        List<string> tmpList = new List<string>();
        while (s.Contains("<br>"))
        {
            int br_index = s.IndexOf("<br>");
            tmpList.Add(s.Substring(0, br_index));
            s = s.Substring(br_index+4);
        }

        return tmpList;
    }

    /// <summary>
    /// 불러온 공지사항을 각 파트별로 나누어주는 함수
    /// </summary>
    private void Deivide_Notice_Text(string s)
    {
        Notice tmpNotice = new Notice();

        List<string> tmpList = new List<string>();
        while (s.Contains("+"))
        {
            int br_index = s.IndexOf("+");
            tmpList.Add(s.Substring(0, br_index));
            s = s.Substring(br_index+1);
        }
        tmpList.Add(s);

        tmpNotice.Num = int.Parse(tmpList[0]);
        tmpNotice.title = tmpList[1];
        tmpNotice.content = tmpList[2];
        tmpNotice.date = tmpList[3];

        nowNotice.Add(tmpNotice);
    }

    public void Create_Notice_Window()
    {
        if(nowNotice.Count != 0)
        {
            for (int i = 0; i < nowNotice.Count; i++)
            {
                GameObject tmpObj = Instantiate(WindowPrefeb, Content);
                tmpObj.GetComponent<UI_Notice_Toolbar>().Title.text = nowNotice[i].title;
                tmpObj.GetComponent<UI_Notice_Toolbar>().Set_Window_Values(nowNotice[i]);
                tmpObj.GetComponent<UI_Notice_Toolbar>().Window = Window;
                tmpObj.GetComponent<UI_Notice_Toolbar>().SetButton();
            }
        }
    }
}
