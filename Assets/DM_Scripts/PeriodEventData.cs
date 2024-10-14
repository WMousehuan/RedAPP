#if ENABLE_ANTI_CHEAT
using CodeStage.AntiCheat.ObscuredTypes;
#endif
using System;

public static class PeriodEventData
{
	public static PeriodEvent GetCurrentPeriodEvent()
	{
		DateTime now = DateTime.Now;
		if (now >= GetPeriodStartDateTime(PeriodEvent.Christmas) && now < GetPeriodEndDateTime(PeriodEvent.Christmas))
		{
			return PeriodEvent.Christmas;
		}
		return PeriodEvent.None;
	}

	public static DateTime GetPeriodStartDateTime(PeriodEvent p)
	{
		if (p == PeriodEvent.Christmas)
		{
			return new DateTime(DateTime.Now.Year, 12, 20);
		}
		return DateTime.Now;
	}

	public static DateTime GetPeriodEndDateTime(PeriodEvent p)
	{
		if (p == PeriodEvent.Christmas)
		{
			DateTime dateTime = new DateTime(DateTime.Now.Year, 12, 30);
			return dateTime.AddDays(1.0);
		}
		return DateTime.Now;
	}

	public static bool CheckAndOpenPopup()
	{
#if ENABLE_IAP
		for (int i = 1; i < 2; i++)
		{
			if (GetCurrentPeriodEvent() == (PeriodEvent)i && !AlreadyBuyPeriodEventPack((PeriodEvent)i))
			{
				if (MonoSingleton<IAPManager>.Instance.GetProduct(10 + i - 1) != null)
				{
					MonoSingleton<PopupManager>.Instance.Open((PopupType)(94 + i - 1));
				}
				return true;
			}
		}
#endif
		return false;
	}

	public static void OnEventClosePeriodEventPopup()
	{
		//if (MonoSingleton<PlayerDataManager>.Instance.PayCount == 0 && MonoSingleton<PlayerDataManager>.Instance.CurrentLevelNo >= 50)
		//{
		//	MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupStarterPack);
		//}
	}

	public static bool AlreadyBuyPeriodEventPack(PeriodEvent p)
	{
#if ENABLE_ANTI_CHEAT
		return ObscuredPrefs.GetBool("BuyPackage" + p, defaultValue: false);
#else
        return CPlayerPrefs.GetBool("BuyPackage" + p, defaultValue: false);
#endif
    }

    public static void SetBuyPeriodEventPack(PeriodEvent p)
	{
#if ENABLE_ANTI_CHEAT
		ObscuredPrefs.SetBool("BuyPackage" + p, value: true);
#else
		CPlayerPrefs.SetBool("BuyPackage" + p, value: true);
#endif
    }
}
