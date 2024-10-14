/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using System;
using System.Security.Cryptography;
using System.Text;

namespace dotmob
{
	public class ServerProtocolEncryption
	{
		private static readonly string key = "";

		public static string EncryptString(string _value)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(_value);
			TripleDES tripleDES = new TripleDESCryptoServiceProvider();
			tripleDES.Key = Encoding.UTF8.GetBytes(key);
			tripleDES.Mode = CipherMode.ECB;
			ICryptoTransform cryptoTransform = tripleDES.CreateEncryptor();
			byte[] inArray = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
			return Convert.ToBase64String(inArray);
		}

		public static string DecryptString(string _value)
		{
			byte[] array = Convert.FromBase64String(_value);
			TripleDES tripleDES = new TripleDESCryptoServiceProvider();
			tripleDES.Key = Encoding.UTF8.GetBytes(key);
			tripleDES.Mode = CipherMode.ECB;
			ICryptoTransform cryptoTransform = tripleDES.CreateDecryptor();
			byte[] bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
			return Encoding.UTF8.GetString(bytes);
		}
	}
}
