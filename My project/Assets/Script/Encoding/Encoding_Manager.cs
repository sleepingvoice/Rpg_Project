using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography;
using System;
using System.Text;

[CustomEditor(typeof(Encoding_Base))]
public class Encoding_AES : Editor
{
	private Encoding_Base _EncodingComponent;
	private int _aesKeySize;
	private string _aesKey;

	private int _aesIVSize;
	private string _aesIV;

	private void OnEnable()
	{
		_EncodingComponent = (Encoding_Base)target;
	}

	/// <summary>
	/// 인스펙터 설정함수
	/// </summary>
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("ASE키값");

		if (string.IsNullOrEmpty(_aesKey)) // aesKey가  없고
		{
			if (string.IsNullOrEmpty(_EncodingComponent.aesBase64Key) == false) // Encoding_Base에서 aesBase64key가 존재한다면
			{
				_aesKey = Encoding_Class.DecodingBase64(_EncodingComponent.aesBase64Key);
			}
			else
			{
				_aesKey = string.Empty;
			}
		}



		_aesKeySize = Encoding.UTF8.GetByteCount(_aesKey) * 8; // 8bit에 맞춰 크기를 조절한다.
		EditorGUILayout.IntField("AES Key Size", _aesKeySize);
		_aesKey = EditorGUILayout.TextField("AES Key", _aesKey);

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("IV키값");

		if (string.IsNullOrEmpty(_aesIV))
		{
			if (string.IsNullOrEmpty(_EncodingComponent.aesBase64IV) == false)
			{
				_aesIV = Encoding_Class.DecodingBase64(_EncodingComponent.aesBase64IV);
			}
			else
			{
				_aesIV = string.Empty;
			}
		}

		_aesIVSize = Encoding.UTF8.GetByteCount(_aesIV) * 8;
		EditorGUILayout.IntField("AES IV Size", _aesIVSize);

		_aesIV = EditorGUILayout.TextField("AES IV", _aesIV);
	}
}

