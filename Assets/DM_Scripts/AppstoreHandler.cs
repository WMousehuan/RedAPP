using UnityEngine;

public class AppstoreHandler : MonoSingleton<AppstoreHandler>
{
	private static AndroidJavaObject jo;

	private new void Awake()
	{
        //@TODO NATIVE
        //if (!Application.isEditor)
        //{
        //	jo = new AndroidJavaObject("com.purplelilgirl.nativeappstore.NativeAppstore");
        //}
    }

    public void openAppInStore(string appID)
	{
        //@TODO NATIVE
        //if (!Application.isEditor)
        //{
        //	jo.Call("OpenInAppStore", "market://details?id=" + appID);
        //}
    }

    public void appstoreClosed()
	{
	}
}
