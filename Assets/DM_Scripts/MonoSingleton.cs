
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : Component
{
	private static T _instance;

	public static T Instance
	{
		get
		{
			if ((Object)_instance == (Object)null)
			{
				_instance = (UnityEngine.Object.FindObjectOfType(typeof(T)) as T);
				if ((Object)_instance == (Object)null)
				{
					GameObject gameObject = new GameObject();
					gameObject.hideFlags = HideFlags.HideAndDontSave;
					_instance = gameObject.AddComponent<T>();
				}
			}
			return _instance;
		}
	}

	public virtual void Awake()
	{
		_instance = (this as T);
	}
}
