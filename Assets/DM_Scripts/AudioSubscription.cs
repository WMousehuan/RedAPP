using antilunchbox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

[Serializable]
public class AudioSubscription
{
	public AudioSourcePro owner;

	public bool isStandardEvent = true;

	public AudioSourceStandardEvent standardEvent;

	public Component sourceComponent;

	public string methodName = string.Empty;

	private bool isBound;

	public AudioSourceAction actionType;

	public string cappedName;

	public bool filterLayers;

	public bool filterTags;

	public bool filterNames;

	public int tagMask;

	public int nameMask;

	public string nameToAdd = string.Empty;

	public List<string> allNames = new List<string>();

	public int layerMask;

	public List<string> tags = new List<string>
	{
		"Default"
	};

	public List<string> names = new List<string>();

	private Component targetComponent;

	private FieldInfo eventField;

	private Delegate eventDelegate;

	private MethodInfo handlerProxy;

	private ParameterInfo[] handlerParameters;

	public bool componentIsValid
	{
		get
		{
			if (standardEventIsValid)
			{
				return true;
			}
			if (!(sourceComponent != null) || string.IsNullOrEmpty(methodName))
			{
				return false;
			}
			MemberInfo memberInfo = sourceComponent.GetType().GetMember(methodName).FirstOrDefault();
			if (memberInfo == null)
			{
				return false;
			}
			return true;
		}
	}

	public bool standardEventIsValid
	{
		get
		{
			if (isStandardEvent && Enum.IsDefined(typeof(AudioSourceStandardEvent), methodName))
			{
				return true;
			}
			return false;
		}
	}

	public void Bind(AudioSourcePro sourcePro)
	{
		if (isBound || isStandardEvent || sourceComponent == null)
		{
			return;
		}
		owner = sourcePro;
		if (componentIsValid)
		{
			MethodInfo methodInfoForAction = getMethodInfoForAction(actionType);
			targetComponent = owner;
			eventField = getField(sourceComponent, methodName);
			if (eventField != null)
			{
				try
				{
					MethodInfo method = eventField.FieldType.GetMethod("Invoke");
					ParameterInfo[] parameters = method.GetParameters();
					eventDelegate = createProxyEventDelegate(targetComponent, eventField.FieldType, parameters, methodInfoForAction);
				}
				catch (Exception)
				{
					return;
				}
				Delegate value = Delegate.Combine(eventDelegate, (Delegate)eventField.GetValue(sourceComponent));
				eventField.SetValue(sourceComponent, value);
				isBound = true;
			}
		}
	}

	public void Unbind()
	{
		if (isBound)
		{
			isBound = false;
			Delegate source = (Delegate)eventField.GetValue(sourceComponent);
			Delegate value = Delegate.Remove(source, eventDelegate);
			eventField.SetValue(sourceComponent, value);
			eventField = null;
			eventDelegate = null;
			handlerProxy = null;
			targetComponent = null;
		}
	}

	private FieldInfo getField(Component sourceComponent, string fieldName)
	{
		return (from f in sourceComponent.GetType().GetAllFieldInfos()
			where f.Name == fieldName
			select f).FirstOrDefault();
	}

	private bool signatureIsCompatible(ParameterInfo[] lhs, ParameterInfo[] rhs)
	{
		if (lhs == null || rhs == null)
		{
			return false;
		}
		if (lhs.Length != rhs.Length)
		{
			return false;
		}
		for (int i = 0; i < lhs.Length; i++)
		{
			if (!areTypesCompatible(lhs[i], rhs[i]))
			{
				return false;
			}
		}
		return true;
	}

	private bool areTypesCompatible(ParameterInfo lhs, ParameterInfo rhs)
	{
		if (lhs.ParameterType.Equals(rhs.ParameterType))
		{
			return true;
		}
		if (lhs.ParameterType.IsAssignableFrom(rhs.ParameterType))
		{
			return true;
		}
		return false;
	}

	[ProxyEvent]
	private void CallbackProxy()
	{
		callProxyEventHandler();
	}

	private Delegate createProxyEventDelegate(object target, Type delegateType, ParameterInfo[] eventParams, MethodInfo eventHandler)
	{
		MethodInfo methodInfo = (from m in typeof(AudioSubscription).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
			where m.IsDefined(typeof(ProxyEventAttribute), inherit: true) && signatureIsCompatible(eventParams, m.GetParameters())
			select m).FirstOrDefault();
		if (methodInfo == null)
		{
			return null;
		}
		handlerProxy = eventHandler;
		handlerParameters = eventHandler.GetParameters();
		return Delegate.CreateDelegate(delegateType, this, methodInfo, throwOnBindFailure: true);
	}

	private void callProxyEventHandler(params object[] arguments)
	{
		if (handlerProxy != null)
		{
			if (handlerParameters.Length == 0)
			{
				arguments = null;
			}
			object obj = new object();
			switch (actionType)
			{
			case AudioSourceAction.Play:
				obj = handlerProxy.Invoke(targetComponent, new object[0]);
				break;
			case AudioSourceAction.PlayCapped:
				obj = handlerProxy.Invoke(targetComponent, new object[1]
				{
					cappedName
				});
				break;
			case AudioSourceAction.PlayLoop:
				obj = handlerProxy.Invoke(targetComponent, new object[0]);
				break;
			case AudioSourceAction.Stop:
				obj = handlerProxy.Invoke(targetComponent, new object[0]);
				break;
			}
			if (obj is IEnumerator && targetComponent is MonoBehaviour)
			{
				((MonoBehaviour)targetComponent).StartCoroutine((IEnumerator)obj);
			}
		}
	}

	private MethodInfo getMethodInfoForAction(AudioSourceAction act)
	{
		MethodInfo result = null;
		switch (act)
		{
		case AudioSourceAction.Play:
			result = typeof(AudioSourcePro).GetMethod("PlayHandler", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, Type.DefaultBinder, new Type[0], null);
			break;
		case AudioSourceAction.PlayCapped:
			result = typeof(AudioSourcePro).GetMethod("PlayCappedHandler", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, Type.DefaultBinder, new Type[1]
			{
				typeof(string)
			}, null);
			break;
		case AudioSourceAction.PlayLoop:
			result = typeof(AudioSourcePro).GetMethod("PlayLoopHandler", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, Type.DefaultBinder, new Type[0], null);
			break;
		case AudioSourceAction.Stop:
			result = typeof(AudioSourcePro).GetMethod("StopHandler", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, Type.DefaultBinder, new Type[0], null);
			break;
		}
		return result;
	}
}
