using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User_Info : MonoBehaviour
{
    [HideInInspector]public string User_Code;
    private Save_Data my_Save;
    public Save_Data save { get { return my_Save; } }

    private Encoding_Base myKey;
    public string Key { set { myKey.aesBase64Key = value; } }

    private static User_Info instance = null;
    
    [HideInInspector]public Encoding_Text myEncoding; // 암복호화 함수
    public string IVKey;

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

        Set();
    }

    private void Set()
    {
        myKey = GetComponent<Encoding_Base>();

        myEncoding = GetComponent<Encoding_Text>();
        myEncoding.KeySet(myKey);
        myEncoding.Set_KeyBase64IV(IVKey);

        my_Save = GetComponent<Save_Data>();
    }
}
