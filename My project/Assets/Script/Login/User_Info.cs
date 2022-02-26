using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User_Info : MonoBehaviour
{
    [HideInInspector]public string User_Code;
    private Save_Data my_Save;
    public Save_Data save { get { return my_Save; } }

    private static User_Info instance = null;

    public static User_Info Instance
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

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        DontDestroyOnLoad(this);
        my_Save = GetComponent<Save_Data>();
    }
}
