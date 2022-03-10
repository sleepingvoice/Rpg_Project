using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("BGM")]
    public AudioClip Town_BGM;

    [Header("AudioSource")]
    [SerializeField] private Sound_SE SE;
    [SerializeField] private AudioSource BGM;

    private void Awake()
    {
        BGM_Start();
    }

    private void BGM_Start()
    {
        BGM.clip = Town_BGM;
        BGM.Play();
    }

    /// <summary>
    /// SE의 audio이름을 받아 AudioSouce를 반환시켜주는 함수
    public AudioSource SE_Sound_Change(string AudioName)
    {
        SE.Mapping();
        return SE.SE_Audio[AudioName];
    }

    /// <summary>
    /// BGM 음향 크기 조절 함수
    /// 0~1까지의 값을 value에 넣으면 된다.
    /// </summary>
    public void BGM_Value(float value)
    {
        BGM.volume = value;
    }

    /// <summary>
    /// SE 음향 크기 조절 함수
    /// 0~1까지의 값을 value에 넣으면 된다.
    /// </summary>
    public void SE_Value(float value)
    {
        SE.Mapping();
        foreach(AudioSource Audio in SE.SE_Audio.Values)
        {
            Audio.volume = value;
        }
    }
}
