//@TODO ENABLE_LOCAL_NOTIFICATION
//#define ENABLE_LOCAL_NOTIFICATION

using System.Collections.Generic;
using UnityEngine;

public class LocalPush
{
	public static bool isInit = false;

#if ENABLE_LOCAL_NOTIFICATION
	private static AndroidJavaClass Notification_Class;

	private static string classname = "com.unity3d.player.UnityPlayerActivity";

	private static string packageName = "com.cookapps.localpush.LocalNotification";
#endif
	private static List<int> ID = new List<int>();

	public static void Init_Notification()
	{
#if ENABLE_LOCAL_NOTIFICATION
        isInit = true;
		Notification_Class = new AndroidJavaClass(packageName);
#endif
    }

    public static void SetNotification(int id, string ticker, string title, string body, int second)
	{
#if ENABLE_LOCAL_NOTIFICATION
        if (isInit && !ID.Contains(id))
		{
			ID.Add(id);
			long num = second * 1000;
			Notification_Class.CallStatic("SetLocalNoti", classname, id, ticker, title, body, num);
		}
#endif
	}

	public static void DeleteNotification(int id)
	{
#if ENABLE_LOCAL_NOTIFICATION
        if (isInit)
		{
			Notification_Class.CallStatic("CancelLocalNoti", id);
		}
#endif
	}

	public static void DeleteAllNotification()
	{
#if ENABLE_LOCAL_NOTIFICATION
        if (!isInit)
		{
			return;
		}
		Notification_Class.CallStatic("DeleteNotiBar");
		if (ID != null)
		{
			for (int i = 0; i < ID.Count; i++)
			{
				Notification_Class.CallStatic("CancelLocalNoti", ID[i]);
			}
			ID.Clear();
		}
#endif
    }

	public static void CheckLocalPushOpend()
	{
#if ENABLE_LOCAL_NOTIFICATION
        if (Application.platform == RuntimePlatform.Android)
		{
			if (Notification_Class == null)
			{
				Notification_Class = new AndroidJavaClass(packageName);
			}
			Notification_Class.CallStatic("ResumeEvent");
		}
#endif
	}
}
