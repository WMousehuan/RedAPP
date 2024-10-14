using System.Collections;
using UnityEngine;

public class SceneClass : MonoBehaviour
{
	public bool DontDestroyAtSceneChange;

	public virtual void Awake()
	{
		MonoSingleton<SceneControlManager>.Instance.CurrentScene = this;
	}

	public virtual IEnumerator OnSceneShow()
	{
		yield return null;
	}

	public virtual void OnSceneHideStart()
	{
	}

	public virtual void OnSceneHideEnd()
	{
	}
}
