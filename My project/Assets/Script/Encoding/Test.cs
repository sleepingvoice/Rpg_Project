using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	public Encoding_Base _EncodingComponent;

	[Header("암호화할 글")]
	public string TestText;

	private void Start()
	{
		string encryptText = Encoding_Class.EncodingAESbyBase64Key(TestText, _EncodingComponent.aesBase64Key, _EncodingComponent.aesBase64IV);
		Debug.Log("encryptText : " + encryptText); // 암호화한 글
		string decryptText = Encoding_Class.DecodingAESByBase64Key(encryptText, _EncodingComponent.aesBase64Key, _EncodingComponent.aesBase64IV);
		Debug.Log("decryptText : " + decryptText); // 복호화한 글
	}
}
