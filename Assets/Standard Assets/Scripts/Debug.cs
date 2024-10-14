//using System;
//using System.Diagnostics;
//using UnityEngine;

//public static class Debug
//{
//	public static bool isDebugBuild => UnityEngine.Debug.isDebugBuild;

//	[Conditional("UNITY_EDITOR")]
//	public static void Log(object message)
//	{
//		UnityEngine.Debug.Log(message);
//	}

//	[Conditional("UNITY_EDITOR")]
//	public static void Log(object message, UnityEngine.Object context)
//	{
//		UnityEngine.Debug.Log(message, context);
//	}

//	[Conditional("UNITY_EDITOR")]
//	public static void LogFormat(string format, params object[] args)
//	{
//		UnityEngine.Debug.LogFormat(format, args);
//	}

//	[Conditional("UNITY_EDITOR")]
//	public static void LogWarningFormat(string format, params object[] args)
//	{
//		UnityEngine.Debug.LogWarningFormat(format, args);
//	}

//	[Conditional("UNITY_EDITOR")]
//	public static void LogErrorFormat(string format, params object[] args)
//	{
//		UnityEngine.Debug.LogErrorFormat(format, args);
//	}

//	[Conditional("UNITY_EDITOR")]
//	public static void LogErrorFormat(UnityEngine.Object context, string format, params object[] args)
//	{
//		UnityEngine.Debug.LogErrorFormat(context, format, args);
//	}

//	[Conditional("UNITY_EDITOR")]
//	public static void LogError(object message)
//	{
//		UnityEngine.Debug.LogError(message);
//	}

//	[Conditional("UNITY_EDITOR")]
//	public static void LogError(object message, UnityEngine.Object context)
//	{
//		UnityEngine.Debug.LogError(message, context);
//	}

//	[Conditional("UNITY_EDITOR")]
//	public static void LogWarning(object message)
//	{
//		UnityEngine.Debug.LogWarning(message.ToString());
//	}

//	[Conditional("UNITY_EDITOR")]
//	public static void LogWarning(object message, UnityEngine.Object context)
//	{
//		UnityEngine.Debug.LogWarning(message.ToString(), context);
//	}

//	[Conditional("UNITY_EDITOR")]
//	public static void DrawLine(Vector3 start, Vector3 end, Color color = default(Color), float duration = 0f, bool depthTest = true)
//	{
//		UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
//	}

//	[Conditional("UNITY_EDITOR")]
//	public static void DrawRay(Vector3 start, Vector3 dir, Color color = default(Color), float duration = 0f, bool depthTest = true)
//	{
//		UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);
//	}

//	[Conditional("UNITY_EDITOR")]
//	public static void Assert(bool condition)
//	{
//		if (!condition)
//		{
//			throw new Exception();
//		}
//	}

//	[Conditional("UNITY_EDITOR")]
//	public static void Assert(bool condition, string message)
//	{
//	}

//	[Conditional("UNITY_EDITOR")]
//	public static void Break()
//	{
//		UnityEngine.Debug.Break();
//	}

//    public static void LogException(Exception ex)
//    {
//    }
//}
