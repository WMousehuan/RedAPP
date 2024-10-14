using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LogReporterToParse : MonoBehaviour
{
	private static LogReporterToParse s_instance;

	private readonly Dictionary<string, string> headers = new Dictionary<string, string>();

	private readonly LogJson logJson = new LogJson();

	private readonly string parseAPIKey = "s7SqKSCcbYGqbhMxB15bem1pec5rFb4e4D6McSQ3";

	private readonly string parseAppID = "kYBfMrUmrFn8pT5ieClyLIifsSMv072zxogJBau5";

	private readonly string postURL = "https://api.parse.com/1/classes/CrashReport";

	public static LogReporterToParse instance
	{
		get
		{
			if (null == s_instance)
			{
				s_instance = new GameObject("LogReport", typeof(LogReporterToParse)).GetComponent<LogReporterToParse>();
			}
			return s_instance;
		}
	}

	public void CreateObject()
	{
	}

	private void Awake()
	{
		if (!Application.isEditor)
		{
			Application.logMessageReceived += CaptureLog;
		}
		Object.DontDestroyOnLoad(base.gameObject);
		headers.Add("X-Parse-Application-Id", parseAppID);
		headers.Add("X-Parse-REST-API-Key", parseAPIKey);
		headers.Add("Content-Type", "application/json");
		logJson.AppName = Application.productName;
		logJson.DeviceUniqueKey = PlayerDataManager.GetDeviceID();
		logJson.AppVersion = GlobalSetting.ConfigData.AppVersion;
		logJson.Device = SystemInfo.deviceModel + ":" + SystemInfo.deviceName;
		logJson.OS = SystemInfo.operatingSystem;
	}

	public void SetUserInfo(string UID, string userName)
	{
		logJson.UID = UID;
		logJson.UserName = userName;
	}

	public void SetUserInfo_Guest(string CCID)
	{
		logJson.CCID = CCID;
	}

	private IEnumerator SendDebugToServer()
	{
		yield return new WWW(postURL, Encoding.ASCII.GetBytes(JsonUtility.ToJson(logJson)), headers);
	}

	private void CaptureLog(string condition, string stacktrace, LogType type)
	{
		if (type == LogType.Exception)
		{
			logJson.ExceptionLog = type + " " + condition + "\n" + stacktrace;
			StartCoroutine(SendDebugToServer());
			if (!Application.isEditor)
			{
				SomethingReallyBadHappened();
			}
		}
	}

	private void SomethingReallyBadHappened()
	{
	}
}
