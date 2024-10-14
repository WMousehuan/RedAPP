
#if ENABLE_BESTHTTP
using BestHTTP;
using BestHTTP.JSON;
#endif
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class CM_main : MonoBehaviour
{
	private static CM_main _instance;

	private readonly string BannerURL = "";

	public Texture2D CM_LageBanner;

	public Texture2D CM_SmallIcon;

	public bool isCrossPromotionSuceess;

	public string TargetAppUrl = string.Empty;

	private string TargetLargeBannerUrl = string.Empty;

	public string TargetProjectCode = string.Empty;

	private string TargetSmallIconUrl = string.Empty;

	public static CM_main instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = UnityEngine.Object.FindObjectOfType<CM_main>();
				if (_instance == null)
				{
					GameObject gameObject = new GameObject("CallBannerInofo");
					_instance = gameObject.AddComponent<CM_main>();
					UnityEngine.Object.DontDestroyOnLoad(gameObject.gameObject);
				}
			}
			return _instance;
		}
	}

	private void Start()
	{
	}

	public void callAppInfo(string _projectNameCode, string _displayForm, string _platform)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("from", _projectNameCode);
		dictionary.Add("display", _displayForm);
		dictionary.Add("platform", _platform);
		dictionary.Add("data", "json");
		CallforCrosspromotionByBestHTTP(dictionary);
	}

	private void CallforCrosspromotionByBestHTTP(Dictionary<string, string> _obj)
	{
#if ENABLE_BESTHTTP
		HTTPRequest hTTPRequest = new HTTPRequest(new Uri(BannerURL), OnRequestFinished);
		foreach (KeyValuePair<string, string> item in _obj)
		{
			if (item.Value != null)
			{
				hTTPRequest.AddField(item.Key, item.Value);
			}
		}
		hTTPRequest.Send();
#endif
	}

#if ENABLE_BESTHTTP
	private void OnRequestFinished(HTTPRequest _request, HTTPResponse response)
	{
		int num = 0;
		switch (_request.State)
		{
		case HTTPRequestStates.Finished:
			num = ((!_request.Response.IsSuccess) ? (-1) : 0);
			break;
		case HTTPRequestStates.Error:
			num = -1;
			break;
		case HTTPRequestStates.ConnectionTimedOut:
			num = 1;
			break;
		case HTTPRequestStates.TimedOut:
			num = 1;
			break;
		}
		if (num == 0)
		{
			jsonParsingForCrossPromotion(response.DataAsText);
		}
		else
		{
			isCrossPromotionSuceess = false;
		}
	}
#endif

    private void jsonParsingForCrossPromotion(string resultData)
	{
		//List<object> list = Json.Decode(resultData) as List<object>;
		List<object> list = JsonConvert.DeserializeObject(resultData) as List<object>;
        if (list.Count == 0)
		{
			isCrossPromotionSuceess = false;
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (i == 0)
			{
				Dictionary<string, object> dictionary = list[i] as Dictionary<string, object>;
				TargetProjectCode = dictionary["destination_app"].ToString();
				TargetAppUrl = dictionary["store_url"].ToString();
				TargetSmallIconUrl = dictionary["icon_url"].ToString();
				TargetLargeBannerUrl = dictionary["image_url"].ToString();
			}
		}
		SaveLargeImagefile();
		SaveSmallImagefile();
	}

	private void SaveLargeImagefile()
	{
#if ENABLE_BESTHTTP
		HTTPRequest hTTPRequest = new HTTPRequest(new Uri(TargetLargeBannerUrl), OnRequestFinishedLargeImage);
		hTTPRequest.Send();
#endif
	}

#if ENABLE_BESTHTTP
	private void OnRequestFinishedLargeImage(HTTPRequest _request, HTTPResponse response)
	{
		int num = 0;
		switch (_request.State)
		{
		case HTTPRequestStates.Finished:
			num = ((!_request.Response.IsSuccess) ? (-1) : 0);
			break;
		case HTTPRequestStates.Error:
			num = -1;
			break;
		case HTTPRequestStates.ConnectionTimedOut:
			num = 1;
			break;
		case HTTPRequestStates.TimedOut:
			num = 1;
			break;
		}
		if (num == 0)
		{
			CM_LageBanner = response.DataAsTexture2D;
			isCrossPromotionSuceess = true;
		}
		else
		{
			isCrossPromotionSuceess = false;
		}
	}
#endif

	private void SaveSmallImagefile()
	{
#if ENABLE_BESTHTTP
		HTTPRequest hTTPRequest = new HTTPRequest(new Uri(TargetSmallIconUrl), OnRequestFinishedSmallImage);
		hTTPRequest.Send();
#endif
	}

#if ENABLE_BESTHTTP
	private void OnRequestFinishedSmallImage(HTTPRequest _request, HTTPResponse response)
	{
		int num = 0;
		switch (_request.State)
		{
		case HTTPRequestStates.Finished:
			num = ((!_request.Response.IsSuccess) ? (-1) : 0);
			break;
		case HTTPRequestStates.Error:
			num = -1;
			break;
		case HTTPRequestStates.ConnectionTimedOut:
			num = 1;
			break;
		case HTTPRequestStates.TimedOut:
			num = 1;
			break;
		}
		if (num == 0)
		{
			isCrossPromotionSuceess = true;
			CM_SmallIcon = response.DataAsTexture2D;
			if (MonoSingleton<SceneControlManager>.Instance.CurrentSceneType == SceneType.Lobby)
			{
				SceneLobby sceneLobby = MonoSingleton<SceneControlManager>.Instance.CurrentScene as SceneLobby;
				if ((bool)sceneLobby)
				{
					sceneLobby.RefreshCrossPromotionIcon();
				}
			}
		}
		else
		{
			isCrossPromotionSuceess = false;
		}
	}
#endif
}