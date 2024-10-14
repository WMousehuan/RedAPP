using UnityEngine;

public class AnalyticsLocalDataManager : MonoSingleton<AnalyticsLocalDataManager>
{
	private readonly string PrefsKey = "Analytics";

	public AnalyticsLocalData Data = new AnalyticsLocalData();

	private bool dirtyValue;

	public override void Awake()
	{
		base.Awake();
		string @string = PlayerPrefs.GetString(PrefsKey, string.Empty);
		if (!string.IsNullOrEmpty(@string))
		{
			Data = JsonUtility.FromJson<AnalyticsLocalData>(@string);
		}
	}

	public void Save()
	{
		dirtyValue = true;
	}

	private void LateUpdate()
	{
		if (dirtyValue)
		{
			dirtyValue = false;
			PlayerPrefs.SetString(PrefsKey, JsonUtility.ToJson(Data));
		}
	}
}
