using System.Diagnostics;
using UnityEngine;

public static class DebugColor
{
	[Conditional("UNITY_EDITOR")]
	public static void LogJC(object message)
	{
		UnityEngine.Debug.Log("<color=green>" + message + "</color>");
	}

	[Conditional("UNITY_EDITOR")]
	public static void LogJC(object message, Object context)
	{
		UnityEngine.Debug.Log("<color=green>" + message + "</color>", context);
	}

	[Conditional("UNITY_EDITOR")]
	public static void LogAR(object message)
	{
		UnityEngine.Debug.Log("<color=blue>" + message + "</color>");
	}

	[Conditional("UNITY_EDITOR")]
	public static void LogAR(object message, Object context)
	{
		UnityEngine.Debug.Log("<color=blue>" + message + "</color>", context);
	}

	[Conditional("UNITY_EDITOR")]
	public static void LogCJE(object message)
	{
		UnityEngine.Debug.Log("<color=purple>" + message + "</color>");
	}

	[Conditional("UNITY_EDITOR")]
	public static void LogCJE(object message, Object context)
	{
		UnityEngine.Debug.Log("<color=purple>" + message + "</color>", context);
	}
}
