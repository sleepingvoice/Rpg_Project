using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class Encoding_AES_Crypto
{
	public static int[] aesKeySize = { 128, 192, 256 };
	public static int aesIVSize = 128;

	// 암복호화를 위한 인터페이스
	private ICryptoTransform encrypter;
	private ICryptoTransform decrypter;


	/// <summary>
	/// AES 암호화 생성
	/// </summary>
	public void Create(string base64Key, string base64IV)
	{
		byte[] key = Convert.FromBase64String(base64Key);
		byte[] iv = Convert.FromBase64String(base64IV);

		// 문자열 양방향 암호화 클래스 사전 설정
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.KeySize = key.Length * 8; 
		rijndaelManaged.BlockSize = aesIVSize;    
		rijndaelManaged.Padding = PaddingMode.PKCS7; 
		rijndaelManaged.Mode = CipherMode.CBC;

		rijndaelManaged.Key = key;
		rijndaelManaged.IV = iv;

		encrypter = rijndaelManaged.CreateEncryptor();
		decrypter = rijndaelManaged.CreateDecryptor();
	}

	/// <summary>
	/// 암호화 함수
	/// </summary>
	public string Encrypt(string plainText)
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encrypter, CryptoStreamMode.Write))
			{
				byte[] byteData = Encoding.UTF8.GetBytes(plainText);
				cryptoStream.Write(byteData, 0, byteData.Length);
			}

			byte[] byteCrypto = memoryStream.ToArray();
			return Convert.ToBase64String(byteCrypto);
		}
	}

	/// <summary>
	/// 복호화 함수
	/// </summary>
	public string Decrypt(string encryptData)
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decrypter, CryptoStreamMode.Write))
			{
				byte[] byteEncrpt = Convert.FromBase64String(encryptData);
				cryptoStream.Write(byteEncrpt, 0, byteEncrpt.Length);
			}

			byte[] byteCrypto = memoryStream.ToArray();
			return Encoding.UTF8.GetString(byteCrypto);
		}
	}
}
