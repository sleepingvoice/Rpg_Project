using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login : MonoBehaviour
{

	public string inputUserName;
	public string inputPassword;

	string LoginURL = "http:///login.php";

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			StartCoroutine(LoginToDB(inputUserName, inputPassword));
	}

	IEnumerator LoginToDB(string username, string password)
	{
		WWWForm form = new WWWForm();
		form.AddField("usernamePost", username);
		form.AddField("passwordPost", password);

		WWW www = new WWW(LoginURL, form);

		yield return www;
		Debug.Log(www.text);

	}
}