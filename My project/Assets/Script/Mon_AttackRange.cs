using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon_AttackRange : MonoBehaviour
{
    [HideInInspector] public bool PlayerOnOff = false;
    [HideInInspector] public GameObject Player = null;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (!PlayerOnOff)
            {
                PlayerOnOff = true;
                Player = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            if (PlayerOnOff && other == Player)
            {
                PlayerOnOff = false;
                Player = null;
            }
        }
    }
}
