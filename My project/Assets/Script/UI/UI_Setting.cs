﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Setting : MonoBehaviour
{
    [Header("사운드")]
    public Slider SE_Sound; // SE 슬라이더
    public Slider BGM_Sound;// BGM 슬라이더
    public TextMeshProUGUI SE_Text;    // SE 값
    public TextMeshProUGUI BGM_Text;   // BGM 값
    private float SE_Value = 0f;
    private float BGM_Value = 0f;

    [Header("Window")]
    public TMP_Dropdown Window_Value; // 창모드 크기
    private int Window_width;     // 창모드시 가로크기
    private int Window_height;    // 창모드시 세로크기
    private int DropBox_value = 0;     // 드롭박스의 선택된 값;
    

    public void Setting_Update()
    {
        CheckSound();
        Check_Window_Value();
    }

    public void Btk_Click(bool b)
    {
        if (b)
            Screen.SetResolution(1920, 1080, true);
        else
            Screen.SetResolution(Window_width, Window_height, false);
    }

    /// <summary>
    /// 음향 설정 체크
    /// </summary>
    public void CheckSound()
    {
        if (SE_Value == SE_Sound.value && BGM_Value == BGM_Sound.value) // 값이 변하지않았을경우 실행시키지않는다.
            return;
        SE_Value = SE_Sound.value;
        BGM_Value = BGM_Sound.value;

        GameManager.Instance.My_Sound.SE_Value(SE_Value);
        GameManager.Instance.My_Sound.BGM_Value(BGM_Value);

        SE_Value *= 100;
        BGM_Value *= 100;
        SE_Text.text = SE_Value.ToString("F0") + "%";
        BGM_Text.text = BGM_Value.ToString("F0") + "%";
    }


    /// <summary>
    /// 드롭박스의 값에 따른 창의 변화
    /// </summary>
    public void Check_Window_Value()
    {
        if (DropBox_value == Window_Value.value) // 드롭박스의 값이 바꼈을때만 함수가 실행
            return;
        string s = Window_Value.itemText.text;
        List<int> result=devid_Window_Value(s);
        Window_width = result[0];
        Window_height = result[1];
    }

    private List<int> devid_Window_Value(string s)
    {
        List<int> result = new List<int>();
        List<string> tmp_list = new List<string>();
        string tmp = "";
        for(int i=0;i<s.Length;i++)
        {
            if(s[i]!= ' ')
            {
                if (s[i] == 'X')
                {
                    tmp_list.Add(tmp);
                    tmp = "";
                }
                else
                    tmp += s[i];
            }
        }
        tmp_list.Add(tmp);
        for(int i =0;i<tmp_list.Count;i++)
        {
            result.Add(int.Parse(tmp_list[i]));
        }

        return result;
    }
}
