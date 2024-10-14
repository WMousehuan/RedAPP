using UnityEngine;

[RequireComponent(typeof(InternetReachabilityVerifier))]
public class CustomIRVExample : MonoBehaviour
{
	private InternetReachabilityVerifier irv;

	private string log = string.Empty;

	private Vector2 scrollPos;

	private void appendLog(string s)
	{
		log = log + s + "\n";
	}

	private bool verifyNetCheckData(WWW www, string customMethodExpectedData)
	{
		if (customMethodExpectedData == null || customMethodExpectedData.Length == 0)
		{
			appendLog("Custom verifyNetCheckData - Null or empty customMethodExpectedData!");
			return false;
		}
		bool flag = www.text.Contains(customMethodExpectedData);
		appendLog("Custom verifyNetCheckData - result:" + flag + ", customMethodExpectedData:" + customMethodExpectedData + ", www.text:" + www.text);
		return flag;
	}

	private void netStatusChanged(InternetReachabilityVerifier.Status newStatus)
	{
		appendLog("Net status changed: " + newStatus);
		if (newStatus == InternetReachabilityVerifier.Status.Error)
		{
			string lastError = irv.lastError;
			appendLog("Error: " + lastError);
			if (lastError.Contains("no crossdomain.xml"))
			{
				appendLog("See http://docs.unity3d.com/Manual/SecuritySandbox.html - You should also check WWW Security Emulation Host URL of Unity Editor in Edit->Project Settings->Editor");
			}
		}
	}

	private void Start()
	{
		irv = GetComponent<InternetReachabilityVerifier>();
		irv.customMethodVerifierDelegate = verifyNetCheckData;
		irv.statusChangedDelegate += netStatusChanged;
		appendLog("CustomIRVExample log:\n");
		appendLog("Selected method: " + irv.captivePortalDetectionMethod);
		appendLog("Custom Method URL: " + irv.customMethodURL);
		appendLog("Custom Method Expected Data: " + irv.customMethodExpectedData);
		if (irv.customMethodVerifierDelegate != null)
		{
			appendLog("Using custom method verifier delegate.");
		}
		if (irv.customMethodURL.Contains("strobotnik.com"))
		{
			appendLog("WARNING - You're using the default value for Custom Method URL specified in example scene. It has no guarantee of being up and running. Please use your own custom URL.\n");
		}
	}

	private void OnGUI()
	{
		GUI.color = new Color(0.9f, 0.95f, 1f);
		GUILayout.Label("Strobotnik InternetReachabilityVerifier for Unity");
		GUILayout.BeginHorizontal();
		GUI.color = new Color(0.7f, 0.8f, 0.9f);
		GUILayout.Label("Status: ");
		GUI.color = Color.white;
		GUILayout.Label(string.Empty + irv.status);
		GUILayout.EndHorizontal();
		scrollPos = GUILayout.BeginScrollView(scrollPos);
		GUILayout.Label(log);
		GUILayout.EndScrollView();
	}
}
