using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Register : MonoBehaviour
{
	public Text Id;
	public Text PassWord;
	public Button signup;
	
	private string User_code;
	private string Normal_Url = "http://localhost/Test/";

	private void Awake()
    {
		signup.onClick.AddListener(() => RegisterBtk());
	}

	public void RegisterBtk()
    {
		if(Id.text != "" && PassWord.text != "")
        {
			StartCoroutine(Register_ID());
		}
    }

	IEnumerator Register_ID()
    {
		UnityWebRequest CheckCode = UnityWebRequest.Get(Normal_Url + "RegisterCheck.php");

		yield return CheckCode.SendWebRequest();

		if (CheckCode.error != null)
		{
			Debug.Log("번호 받아오기 실패");
			yield return null;
		}
		else
		{
			string CodeString = CheckCode.downloadHandler.text;
			int code = (int.Parse(CodeString));
			code++;
			User_code = code.ToString("000000");
		}

		WWWForm IDform = new WWWForm();
		IDform.AddField("ID_Post", Id.text);
		IDform.AddField("PassWord_Post", PassWord.text);
		IDform.AddField("User_code_Post", User_code);

        UnityWebRequest www = UnityWebRequest.Post(Normal_Url + "register.php", IDform);

        yield return www.SendWebRequest();

		Debug.Log(CheckResult(www.downloadHandler.text));
	}

	private string CheckResult(string s)
    {
		string st1 = s.Replace("\n", "");
		st1 = st1.Replace("\r", "");

		return st1;
	}
}
