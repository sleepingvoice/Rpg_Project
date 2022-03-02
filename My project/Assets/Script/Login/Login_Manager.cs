using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Login_Manager : MonoBehaviour
{
    [Header("로그인")]
    public InputField Log_Id;
    public InputField Log_PassWord;
	public Button Log_Btk;

	[Header("로그인 알림창")]
	public GameObject Log_Error;

    [Header("회원가입")]
	public InputField Reg_Id;
	public InputField Reg_PassWord;
	public InputField Reg_PassWord_Check;
	public Button Reg_SignUp;

	[Header("회원가입 알림창")]
	public GameObject Reg_Success;
	public GameObject Reg_Error_SameID;
	public GameObject Reg_Error_NoSame;


	private string User_code;
	private string Normal_Url = "http://localhost/Test/";

	private void Awake()
	{
		Reg_SignUp.onClick.AddListener(() => Reg_BtkCheck(Register_ID()));
		Log_Btk.onClick.AddListener(() => Log_BtkCheck(Login_ID()));
		GetComponent<UI_Setting>().Load_Window_Value();
	}

    #region 버튼

    public void Log_BtkCheck(IEnumerator enumerator)
	{
		if (Log_Id.text != "" && Log_PassWord.text != "")
		{
			StartCoroutine(enumerator);
		}
	}
	public void Reg_BtkCheck(IEnumerator enumerator)
	{
		CheckPass();
		if (Reg_Id.text != "" && Reg_PassWord.text != "")
		{
			StartCoroutine(enumerator);
		}
	}

    #endregion

    #region 로그인 회원가입

    /// <summary>
    /// ID 로그인 함수
    /// </summary>
    private IEnumerator Login_ID()
	{

		WWWForm IDform = new WWWForm();
		IDform.AddField("ID_Post", Log_Id.text);
		IDform.AddField("PassWord_Post", Log_PassWord.text);
		UnityWebRequest CheckID = UnityWebRequest.Post(Normal_Url + "Login.php", IDform); // 아이디와 비번을 받아서 전송시켜준다.

		yield return CheckID.SendWebRequest();

		string b = CheckResult(CheckID.downloadHandler.text); // 성공인지 실패인지를 받아온다.
		if(b != "false") // 만약 실패하지않았다면 아이디가 존재하는 것이므로
        {
			User_Info.Instance.User_Code = b; // 나타난 유저코드를 넣어준다.
			User_Info.Instance.save.user_Code = b;
			User_Info.Instance.save.Load_UserData(); // User_Data를 불러온다.
			User_Info.Instance.gameObject.GetComponent<Scene_Manager>().Go_to_GameScene(); // 씬을 GameScene로 바꾼다.
		}
		else // 만약 아이디나 비밀번호가 틀렸다면
        {
			Log_Error.SetActive(true);  // 에러메세지를 보여준다.
			Log_PassWord.text = "";  // 비밀번호를 초기화한다.
		}
	}

	/// <summary>
	/// 아이디 회원가입
	/// </summary>
	private IEnumerator Register_ID()
	{
		UnityWebRequest CheckCode = UnityWebRequest.Get(Normal_Url + "RegisterCheck.php"); // User_Code의 마지막 값을 받아온다.

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
			User_code = code.ToString("000000"); // 유저코드의 마지막값에서 1 더해준값을 넣어준다.
		}

		WWWForm IDform = new WWWForm();
		IDform.AddField("ID_Post", Reg_Id.text);
		IDform.AddField("PassWord_Post", Reg_PassWord.text);
		IDform.AddField("User_code_Post", User_code);

		UnityWebRequest www = UnityWebRequest.Post(Normal_Url + "register.php", IDform); // 회원가입을 진행한다.

		yield return www.SendWebRequest();

		string s = CheckResult(www.downloadHandler.text);

		if(s== "false")
			Reg_Error_SameID.SetActive(false);
		else
			Reg_Success.SetActive(true);

		// 서버의 값을 받아온후 회원가입 text를 모두 초기화해준다.

		Reg_Id.text = "";
		Reg_PassWord.text = "";
		Reg_PassWord_Check.text = "";
	}

    #endregion

    #region 보조함수

    private void CheckPass()
    {
		if(Reg_PassWord_Check.text != Reg_PassWord.text)
        {
			Reg_Error_NoSame.SetActive(true);
			Reg_PassWord.text = "";
			Reg_PassWord_Check.text = "";
		}
    }

	private string CheckResult(string s)
	{
		string st1 = s.Replace("\n", "");
		st1 = st1.Replace("\r", "");

		return st1;
	}

    #endregion
}
