using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
	public Encoding_Base _EncodingComponent;

	[Header("암호화할 글")]
	public string TestText;
	public Button TestBtk;

    private void Start()
    {
		TestBtk.onClick.AddListener(() => BtkClick());
    }

    private void BtkClick()
	{
		User_Info.Instance.myEncoding.Set_KeyBase64("LogIDcodeK000001");




		string incodingText = Encoding_Class.EncodingAESbyBase64Key(TestText, _EncodingComponent.aesBase64Key, _EncodingComponent.aesBase64IV);
		Debug.Log("incodingText : " + incodingText);

		string decryptText = Encoding_Class.DecodingAESByBase64Key(incodingText, _EncodingComponent.aesBase64Key, _EncodingComponent.aesBase64IV);
		Debug.Log("decryptText : " + decryptText);

	}
}
