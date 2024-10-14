#if ENABLE_BESTHTTP
using BestHTTP;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;

public class NetworkManager : MonoSingleton<NetworkManager>
{
	public enum URLList
	{
		Dev,
		Release
	}

	[NonSerialized]
	[HideInInspector]
	//public string[] strURLLists = new string[2]
	//{
		
	//};

	//[NonSerialized]
	public URLList ConnectURL;

	[HideInInspector]
	public string URL;

	[HideInInspector]
	public readonly string MapToolURL = "";

	private string packetKey = string.Empty;

	public bool IsCustomizeTimeOut;

	public float CustomizeTimeOut = 30f;

	private WebCommandBase lastCmd;

	public string PacketKey
	{
		get
		{
			return packetKey;
		}
		set
		{
			packetKey = value;
		}
	}

	public bool EncryptOn
	{
		get;
		set;
	}

	public bool IsConnecting
	{
		get;
		set;
	}

	public NetworkManager()
	{
		IsConnecting = false;
		EncryptOn = false;
	}

	public override void Awake()
	{
		base.Awake();
		//URL = strURLLists[(int)ConnectURL];
		ServicePointManager.Expect100Continue = false;
	}

	public void StartNetwork(WebCommandBase cmd)
	{
		IsConnecting = true;
		PushWebCommand(cmd);
	}

	public void StopNetwork(string errMsg, WebCommandBase cmd, bool showErrorPopup = false)
	{
		IsConnecting = false;
		StopAllCoroutines();
	}

	public string GetPacketKey()
	{
		return (!EncryptOn) ? string.Empty : packetKey;
	}

	private void PushWebCommand(WebCommandBase cmd)
	{
		if (MonoSingleton<IRVManager>.Instance.CurrentNetStatus == InternetReachabilityVerifier.Status.Offline)
		{
			MonoSingleton<NetworkManager>.Instance.StopNetwork("Network Offline", cmd);
			return;
		}
		cmd.Pack();
		POST(cmd);
	}

	private void POST(WebCommandBase cmd)
	{
#if ENABLE_BESTHTTP
		HTTPRequest hTTPRequest = new HTTPRequest(new Uri(cmd.URL), HTTPMethods.Post);
		if (cmd.requestBodyJson)
		{
			hTTPRequest.AddHeader("Content-Type", "application/json");
			hTTPRequest.AddHeader("Content-Length", Encoding.UTF8.GetByteCount(cmd.RequestString).ToString());
			hTTPRequest.RawData = Encoding.UTF8.GetBytes(cmd.RequestString);
		}
		else
		{
			Dictionary<string, object> dictionary = Utils.JsonToDict(cmd.RequestString);
			foreach (KeyValuePair<string, object> item in dictionary)
			{
				if (item.Value != null)
				{
					hTTPRequest.AddField(item.Key, item.Value.ToString());
				}
			}
		}
		StartCoroutine(WaitForRequest(hTTPRequest, cmd));
#endif
	}

#if ENABLE_BESTHTTP
	private IEnumerator WaitForRequest(HTTPRequest request, WebCommandBase cmd)
	{
		request.Send();
		yield return StartCoroutine(request);
		if (request.State == HTTPRequestStates.ConnectionTimedOut || request.State == HTTPRequestStates.TimedOut || request.State == HTTPRequestStates.Aborted)
		{
			MonoSingleton<NetworkManager>.Instance.StopNetwork("Timeout", cmd);
		}
		else if (request.State == HTTPRequestStates.Error)
		{
			if (!(request.Uri != null) || request.Exception != null)
			{
			}
			string errMsg = "NetError";
			if (request.Exception != null)
			{
				errMsg = request.Exception.Message;
			}
			MonoSingleton<NetworkManager>.Instance.StopNetwork(errMsg, cmd, showErrorPopup: true);
		}
		else
		{
			if (cmd.IsRetComressed)
			{
				cmd.SetResponseByteData(request.Response.Data);
			}
			else
			{
				cmd.SetResponseStringData(request.Response.DataAsText);
			}
			cmd.Complete();
		}
	}
#endif
}