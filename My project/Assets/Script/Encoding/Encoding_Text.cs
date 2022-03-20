using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Encoding_Text : MonoBehaviour
{
    private Encoding_Base myKey;

    public void KeySet(Encoding_Base tmp)
    {
        myKey = tmp;
    }

    /// <summary>
    /// 넣은 문자를 암호화시켜주는 함수
    /// </summary>
    public string EncodingText(string s)
    {
        if (myKey != null)
            return Encoding_Class.EncodingAESbyBase64Key(s.Trim(), myKey.aesBase64Key, myKey.aesBase64IV);

        return null;
    }

    /// <summary>
    /// 넣은 문자를 해독시켜주는 함수
    /// </summary>
    public string DecodingText(string s)
    {
        if (myKey != null)
        {
            return Encoding_Class.DecodingAESByBase64Key(s.Trim(), myKey.aesBase64Key, myKey.aesBase64IV);
        }

        return null;
    }

    /// <summary>
    /// Key값을 변환시켜 넣어주는 함수
    /// </summary>
    public void Set_KeyBase64(string Key)
    {
        if (string.IsNullOrEmpty(Key)) // aesKey가  없고
        {
            if (string.IsNullOrEmpty(myKey.aesBase64Key) == false) // Encoding_Base에서 aesBase64key가 존재한다면
            {
                Key = Encoding_Class.DecodingBase64(myKey.aesBase64Key);
            }
            else
            {
                Key = string.Empty;
            }
        }

        int _aesKeySize = Encoding.UTF8.GetByteCount(Key) * 8;
        bool isValid = false;
        string validKeySizeText = string.Empty;

        int validKeySize = 0;
        for (int i = 0, icount = Encoding_AES_Crypto.aesKeySize.Length; i < icount; i++)
        {
            validKeySize = Encoding_AES_Crypto.aesKeySize[i];
            validKeySizeText += validKeySize.ToString() + " ";
            if (_aesKeySize.Equals(validKeySize))
            {
                isValid = true;
                break;
            }
        }

        if (isValid)
        {
            myKey.aesBase64Key = Encoding_Class.EncodingBase64(Key);
        }
        else
            Debug.LogError("Key길이가 맞지 않습니다.");
    }

    /// <summary>
    /// IV값을 변환시켜 넣어주는 함수
    /// </summary>
    public void Set_KeyBase64IV(string IV)
    {

        if (string.IsNullOrEmpty(IV))
        {
            if (string.IsNullOrEmpty(myKey.aesBase64IV) == false)
            {
                IV = Encoding_Class.DecodingBase64(myKey.aesBase64IV);
            }
            else
            {
                IV = string.Empty;
            }
        }

        int _aesIVSize = Encoding.UTF8.GetByteCount(IV) * 8;

        if (_aesIVSize == Encoding_AES_Crypto.aesIVSize)
        {
            myKey.aesBase64IV = Encoding_Class.EncodingBase64(IV);
        }
        else
            Debug.LogError("KeyIV 길이가 맞지 않습니다.");
    }
}
