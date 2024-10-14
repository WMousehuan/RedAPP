/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine;

public class ButtonBooster : MonoBehaviour
{
	public Booster booster;

	public void ForceStart(Booster.BoosterType type)
	{
		AddBoosterComponent(type);
	}

	private void AddBoosterComponent(Booster.BoosterType type)
	{
		string empty = string.Empty;
		switch (type)
		{
		case Booster.BoosterType.Hammer:
			booster = base.gameObject.AddComponent<BoosterMagicHammer>();
			break;
		case Booster.BoosterType.CandyPack:
			booster = base.gameObject.AddComponent<BoosterCandyPack>();
			break;
		case Booster.BoosterType.Shuffle:
			booster = base.gameObject.AddComponent<BoosterShuffle>();
			break;
		case Booster.BoosterType.HBomb:
			booster = base.gameObject.AddComponent<BoosterHBomb>();
			break;
		case Booster.BoosterType.VBomb:
			booster = base.gameObject.AddComponent<BoosterVBomb>();
			break;
		default:
			booster = base.gameObject.AddComponent<BoosterMagicHammer>();
			break;
		}
	}

	public void UseBoosterInterface()
	{
		booster.UseBooster();
	}
}
