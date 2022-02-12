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

	private void Awake()
    {
		signup.onClick.AddListener(() => RegisterBtk());
	}

	public void RegisterBtk()
    {
		if(Id.text != null && PassWord.text != null)
        {
			StartCoroutine(Register_ID());
		}
    }

	IEnumerator Register_ID()
    {
		UnityWebRequest CheckCode = UnityWebRequest.Get("http://localhost/Test/RegisterCheck.php");

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

		WWWForm form = new WWWForm();
		form.AddField("ID_Post", Id.text);
		form.AddField("PassWord_Post", PassWord.text);
		form.AddField("User_code_Post", User_code);
		UnityWebRequest www = UnityWebRequest.Post("http://localhost/Test/register.php", form);

		yield return www.SendWebRequest();

		if (www.error == null)
			Debug.Log("성공했습니다.");
		else
			Debug.Log("실패했습니다.");
	}
}
