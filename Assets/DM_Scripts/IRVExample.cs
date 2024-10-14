using UnityEngine;

public class IRVExample : MonoBehaviour
{
	public InternetReachabilityVerifier internetReachabilityVerifier;

	private string log = string.Empty;

	private bool logChosenDefaultByPlatformMethodPending;

	private string url = "http://www.google.com";

	private WWW testWWW;

	private string[] methodNames;

	private int selectedMethod;

	private Vector2 scrollPos;

	private void appendLog(string s)
	{
		log = log + s + "\n";
	}

	private void netStatusChanged(InternetReachabilityVerifier.Status newStatus)
	{
		appendLog("Net status changed: " + newStatus);
		if (newStatus == InternetReachabilityVerifier.Status.Error)
		{
			string lastError = internetReachabilityVerifier.lastError;
			appendLog("Error: " + lastError);
			if (lastError.Contains("no crossdomain.xml"))
			{
				appendLog("See http://docs.unity3d.com/Manual/SecuritySandbox.html - You should also check WWW Security Emulation Host URL of Unity Editor in Edit->Project Settings->Editor");
			}
		}
	}

	private void Start()
	{
		if (internetReachabilityVerifier == null)
		{
			internetReachabilityVerifier = (InternetReachabilityVerifier)UnityEngine.Object.FindObjectOfType(typeof(InternetReachabilityVerifier));
			if (internetReachabilityVerifier == null)
			{
				return;
			}
		}
		internetReachabilityVerifier.statusChangedDelegate += netStatusChanged;
		appendLog("IRVExample log:\n");
		appendLog("Selected method: " + internetReachabilityVerifier.captivePortalDetectionMethod);
		if (internetReachabilityVerifier.captivePortalDetectionMethod == InternetReachabilityVerifier.CaptivePortalDetectionMethod.DefaultByPlatform)
		{
			logChosenDefaultByPlatformMethodPending = true;
		}
		selectedMethod = (int)internetReachabilityVerifier.captivePortalDetectionMethod;
		int num = 8;
		methodNames = new string[num];
		for (int i = 0; i < num; i++)
		{
			string[] array = methodNames;
			int num2 = i;
			InternetReachabilityVerifier.CaptivePortalDetectionMethod captivePortalDetectionMethod = (InternetReachabilityVerifier.CaptivePortalDetectionMethod)i;
			array[num2] = captivePortalDetectionMethod.ToString();
		}
	}

	private void OnGUI()
	{
		if (logChosenDefaultByPlatformMethodPending && internetReachabilityVerifier.captivePortalDetectionMethod != 0)
		{
			appendLog("DefaultByPlatform selected, actual method: " + internetReachabilityVerifier.captivePortalDetectionMethod);
			logChosenDefaultByPlatformMethodPending = false;
		}
		GUI.color = new Color(0.9f, 0.95f, 1f);
		GUILayout.Label("Strobotnik InternetReachabilityVerifier for Unity");
		GUILayout.Label("Selected method: (changes to actual method as needed)");
		selectedMethod = (int)internetReachabilityVerifier.captivePortalDetectionMethod;
		int num = GUILayout.SelectionGrid(selectedMethod, methodNames, 2);
		if (selectedMethod != num)
		{
			selectedMethod = num;
			internetReachabilityVerifier.captivePortalDetectionMethod = (InternetReachabilityVerifier.CaptivePortalDetectionMethod)selectedMethod;
			if (selectedMethod == 0)
			{
				logChosenDefaultByPlatformMethodPending = true;
			}
			else if (selectedMethod == 6)
			{
				appendLog("Using custom method " + ((!internetReachabilityVerifier.customMethodWithCacheBuster) ? "without cache buster, url:\n" : "with cache buster, base url:\n") + internetReachabilityVerifier.customMethodURL);
			}
		}
		if (GUILayout.Button("Force reverification"))
		{
			internetReachabilityVerifier.status = InternetReachabilityVerifier.Status.PendingVerification;
		}
		GUILayout.BeginHorizontal();
		GUI.color = new Color(0.7f, 0.8f, 0.9f);
		GUILayout.Label("Status: ");
		GUI.color = Color.white;
		GUILayout.Label(string.Empty + internetReachabilityVerifier.status);
		GUILayout.EndHorizontal();
		GUI.color = new Color(0.7f, 0.8f, 0.9f);
		GUILayout.Label("Test WWW access:");
		bool flag = internetReachabilityVerifier.status == InternetReachabilityVerifier.Status.NetVerified;
		GUILayout.BeginHorizontal();
		if (!flag || (testWWW != null && !testWWW.isDone))
		{
			GUI.enabled = false;
		}
		if (GUILayout.Button("Fetch"))
		{
			testWWW = new WWW(url);
		}
		if (testWWW != null && !testWWW.isDone)
		{
			GUI.enabled = false;
		}
		else
		{
			GUI.enabled = true;
		}
		url = GUILayout.TextField(url);
		GUI.enabled = true;
		GUILayout.EndHorizontal();
		string text = string.Empty;
		if (testWWW != null)
		{
			text = ((testWWW.error != null && testWWW.error.Length > 0) ? ("error:" + testWWW.error) : ((!testWWW.isDone) ? ("progress:" + (int)(testWWW.progress * 100f) + "%") : "done"));
		}
		GUILayout.Label(text);
		GUI.color = Color.white;
		scrollPos = GUILayout.BeginScrollView(scrollPos);
		GUILayout.Label(log);
		GUILayout.EndScrollView();
	}
}
