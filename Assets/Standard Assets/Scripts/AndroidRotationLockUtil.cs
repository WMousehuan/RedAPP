using UnityEngine;

public class AndroidRotationLockUtil
{
	private static AndroidJavaClass unity;

	public static bool AllowAutorotation()
	{
        //@TODO NATIVE
        //bool flag = false;
        //using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unityutils.RotationLockUtil"))
        //{
        //	if (androidJavaClass.CallStatic<int>("GetAutorotateSetting", new object[1]
        //	{
        //		GetUnityActivity()
        //	}) == 0)
        //	{
        //		return false;
        //	}
        //	return true;
        //}

        return false;
	}

	//private static AndroidJavaObject GetUnityActivity()
	//{
	//	if (unity == null)
	//	{
	//		unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
	//		if (unity != null)
	//		{
	//		}
	//	}
	//	return unity.GetStatic<AndroidJavaObject>("currentActivity");
	//}
}
