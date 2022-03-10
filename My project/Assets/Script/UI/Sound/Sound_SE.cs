using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_SE : MonoBehaviour
{
    [Header("SE")]
    public AudioSource Attack;
    public AudioSource Walk;
    public AudioSource Damaged;
    public AudioSource Menu_Open;

    public Dictionary<string, AudioSource> SE_Audio;

    public void Mapping()
    {
        SE_Audio = new Dictionary<string, AudioSource>();
        SE_Audio.Add("Attack", Attack);
        SE_Audio.Add("Walk", Walk);
        SE_Audio.Add("Damaged", Damaged);
        SE_Audio.Add("Menu_Open", Menu_Open);
    }

}
