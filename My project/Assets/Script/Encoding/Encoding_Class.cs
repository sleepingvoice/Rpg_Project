using System.Collections.Generic;
using System;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;
public class Encoding_Class
{
	static Dictionary<string, Encoding_AES_Crypto> aesManages = new Dictionary<string, Encoding_AES_Crypto>();

	/// <summary>
	/// key값을 저장한 dictionary를 추가한다
	/// </summary>
	static void CreateAESManage(string base64Key, string base64IV)
	{
		Encoding_AES_Crypto aesManage = new Encoding_AES_Crypto();
		aesManage.Create(base64Key, base64IV);
		aesManages.Add(base64Key, aesManage);
	}

	/// <summary>
	/// base64키 기반 AES Encoding 함수
	/// </summary>
	public static string EncodingAESbyBase64Key(string plainText, string base64Key, string base64IV)
	{
		if (aesManages.ContainsKey(base64Key) == false)
		{
			CreateAESManage(base64Key, base64IV);
		}

		return aesManages[base64Key].Encrypt(plainText);
	}

	/// <summary>
	/// base64키 기반 AES Decoding 함수
	/// </summary>
	public static string DecodingAESByBase64Key(string encryptData, string base64Key, string base64IV)
	{
		if (aesManages.ContainsKey(base64Key) == false)
			CreateAESManage(base64Key, base64IV);

		return aesManages[base64Key].Decrypt(encryptData);
	}

	/// <summary>
	/// Base64 인코딩 함수
	/// </summary>
	public static string EncodingBase64(string Text)
	{
		Byte[] strByte = Encoding.UTF8.GetBytes(Text);
		return Convert.ToBase64String(strByte);
	}

	/// <summary>
	/// Base64 디코딩 함수
	/// </summary>
	public static string DecodingBase64(string Text) 
	{
		Byte[] strByte = Convert.FromBase64String(Text);
		return Encoding.UTF8.GetString(strByte);
	}
}
