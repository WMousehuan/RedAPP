using System;
using System.Collections.Generic;

public class XOR
{
	public static string base64Encode(byte[] data)
	{
		try
		{
			return Convert.ToBase64String(data);
		}
		catch (Exception ex)
		{
			throw new Exception("Error in base64Encode" + ex.Message);
		}
	}

	public static byte[] base64Decode(string data)
	{
		try
		{
			data = data.Replace(' ', '+');
			return Convert.FromBase64String(data);
		}
		catch (Exception ex)
		{
			throw new Exception("Error in base64Decode" + ex.Message);
		}
	}

	public static byte[] Encrypt(byte[] textToEncrypt, byte[] key)
	{
		List<byte> list = new List<byte>(textToEncrypt.Length);
		for (int i = 0; i < textToEncrypt.Length; i++)
		{
			for (int j = 0; j < key.Length; j++)
			{
				textToEncrypt[i] = (byte)(textToEncrypt[i] ^ key[j]);
			}
			list.Add(textToEncrypt[i]);
		}
		return list.ToArray();
	}

	public static byte[] Decrypt(byte[] textToEncrypt, byte[] key)
	{
		List<byte> list = new List<byte>(textToEncrypt.Length);
		for (int i = 0; i < textToEncrypt.Length; i++)
		{
			for (int j = 0; j < key.Length; j++)
			{
				textToEncrypt[i] = (byte)(key[j] ^ textToEncrypt[i]);
			}
			list.Add(textToEncrypt[i]);
		}
		return list.ToArray();
	}
}
