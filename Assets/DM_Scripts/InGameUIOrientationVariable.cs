/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using PathologicalGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIOrientationVariable : MonoBehaviour
{
	public bool IsInit;

	public Transform boosterRoot;

	[NonSerialized]
	public Booster[] boosterUI;

	private bool enableCreateCollectEffect = true;

	public Dictionary<CollectBlockType, InGameUICollectBlock> dicCollectObjs = new Dictionary<CollectBlockType, InGameUICollectBlock>();

	public GameObject ObjCollectParent;

	public OrientationType orientationType;

	public GameObject PrefabCollect;

	public Text TextLevel;

	public Text TextMoveCount;

	public Text TextScore;

	public Text textStep;

	private readonly float textTweenTime = 1.2f;

	private readonly float throwEffectTime = 1.5f;

	public GameObject throwingTarget;

	public bool isShowUITween;

	public RectTransform TweenValueBoosterButton;

	private float tweenValueBoosterButtonStartX;

	public RectTransform TweenValueOption;

	private float tweenValueOptionStartX;

	public Slider[] starGauges = new Slider[3];

	public GameObject[] FullStarEffects = new GameObject[3];

	public virtual void Start()
	{
		IsInit = true;
		boosterUI = new Booster[MonoSingleton<PlayerDataManager>.Instance.enableBoosterIndexes.Length];
		for (int i = 0; i < boosterUI.Length; i++)
		{
			GameObject original = Resources.Load("ButtonBooster") as GameObject;
			ButtonBooster component = UnityEngine.Object.Instantiate(original).GetComponent<ButtonBooster>();
			component.ForceStart(MonoSingleton<PlayerDataManager>.Instance.enableBoosterIndexes[i]);
			boosterUI[i] = component.booster;
			boosterUI[i].transform.SetParent(boosterRoot);
			boosterUI[i].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
			boosterUI[i].transform.localPosition = Vector3.zero;
			boosterUI[i].ForceStart();
			boosterUI[i].SetUiIndex(i);
		}
		for (int j = 0; j < boosterUI.Length; j++)
		{
			Booster.BoosterType boosterType = MonoSingleton<PlayerDataManager>.Instance.enableBoosterIndexes[j];
			boosterUI[j].gameObject.SetActive(value: true);
		}
		UpdateTextBoosterCount();
	}

	public virtual void InitUITween()
	{
		if ((bool)TweenValueOption)
		{
			Vector2 anchoredPosition = TweenValueOption.anchoredPosition;
			tweenValueOptionStartX = anchoredPosition.x;
			RectTransform tweenValueOption = TweenValueOption;
			Vector2 anchoredPosition2 = TweenValueOption.anchoredPosition;
			tweenValueOption.anchoredPosition = new Vector2(150f, anchoredPosition2.y);
		}
		if ((bool)TweenValueBoosterButton)
		{
			float num = 600f;
			RectTransform tweenValueBoosterButton = TweenValueBoosterButton;
			float x = num;
			Vector2 anchoredPosition3 = TweenValueBoosterButton.anchoredPosition;
			tweenValueBoosterButton.anchoredPosition = new Vector2(x, anchoredPosition3.y);
			tweenValueBoosterButtonStartX = 0f;
		}
	}

	public virtual void ShowUITween()
	{
		if (!isShowUITween)
		{
			InitUITween();
			if ((bool)TweenValueOption && (CPanelGameUI.Instance.optionButtonTweener == null || !CPanelGameUI.Instance.optionButtonTweener.IsPlaying()))
			{
				CPanelGameUI.Instance.optionButtonTweener = TweenValueOption.DOAnchorPosX(tweenValueOptionStartX, 0.6f, snapping: true).SetEase(Ease.OutBack).SetDelay(0.8f);
			}
			if ((bool)TweenValueBoosterButton)
			{
				TweenValueBoosterButton.DOAnchorPosX(tweenValueBoosterButtonStartX, 0.6f, snapping: true).SetEase(Ease.OutBack).SetDelay(0.8f);
			}
			isShowUITween = true;
		}
	}

	public virtual void HideUITween()
	{
		if ((bool)TweenValueOption)
		{
			TweenValueOption.DOAnchorPosX(150f, 0.4f);
		}
		float endValue = 600f;
		if ((bool)TweenValueBoosterButton)
		{
			TweenValueBoosterButton.DOAnchorPosX(endValue, 0.4f);
		}
	}

	public virtual void Reset(MapDataCollectBlock[] collectBlocks, int[] starPoint, int gid)
	{
		TextLevel.text = "LEVEL " + gid;
		for (int i = 0; i < starPoint.Length; i++)
		{
			starGauges[i].value = 0f;
			starGauges[i].minValue = ((i - 1 != -1) ? starPoint[i - 1] : 0);
			starGauges[i].maxValue = starPoint[i];
			FullStarEffects[i].SetActive(value: false);
		}
		foreach (InGameUICollectBlock value in dicCollectObjs.Values)
		{
			if (value != null)
			{
				UnityEngine.Object.Destroy(value.gameObject);
			}
		}
		int num = 0;
		for (int j = 0; j < collectBlocks.Length; j++)
		{
			if (string.IsNullOrEmpty(collectBlocks[j].blockType) || collectBlocks[j].count <= 0 || !(PrefabCollect != null))
			{
				continue;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(PrefabCollect);
			if (!gameObject)
			{
				continue;
			}
			InGameUICollectBlock component = gameObject.GetComponent<InGameUICollectBlock>();
			if (!component)
			{
				continue;
			}
			if (!string.IsNullOrEmpty(collectBlocks[j].blockType) && collectBlocks[j].count > 0)
			{
				num++;
				component.gameObject.transform.SetParent(ObjCollectParent.transform, worldPositionStays: false);
				CollectBlockType collectBlockType = collectBlocks[j].GetCollectBlockType();
				Sprite sprite;
				switch (collectBlockType)
				{
				case CollectBlockType.NormalRed:
				case CollectBlockType.NormalOrange:
				case CollectBlockType.NormalYellow:
				case CollectBlockType.NormalGreen:
				case CollectBlockType.NormalBlue:
				case CollectBlockType.NormalPurple:
					sprite = Resources.Load<Sprite>(CPanelGameUI.GetCollectIconNormalBlockName(MapData.main.collectBlocks[j].blockType));
					break;
				default:
					sprite = Resources.Load<Sprite>("UI/CollectIcon/" + MapData.main.collectBlocks[j].blockType);
					break;
				case CollectBlockType.Null:
					continue;
				}
				if (sprite != null)
				{
					component.SetData(collectBlockType, collectBlocks[j].count, sprite);
					dicCollectObjs[collectBlockType] = component;
				}
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(component);
			}
		}
		if (num == 1 && orientationType == OrientationType.Landscape)
		{
			ObjCollectParent.transform.localScale = Vector3.one * 1.5f;
		}
		MonoSingleton<AnimationController>.Instance.RegistGoalAnimation();
	}

	public void UpdateStarPoint(int score)
	{
		for (int i = 0; i < starGauges.Length; i++)
		{
			starGauges[i].value = score;
			if (!FullStarEffects[i].activeSelf && starGauges[i].maxValue <= (float)score)
			{
				GameMain.main.StarPoint = i + 1;
				FullStarEffects[i].SetActive(value: true);
			}
		}
	}

	public void UpdateCollect(CollectBlockType collectBlockType, int count)
	{
		if (!dicCollectObjs.ContainsKey(collectBlockType))
		{
			return;
		}
		float num = 0.4f;
		dicCollectObjs[collectBlockType].UpdateTargetCount(count);
		dicCollectObjs[collectBlockType].ImageTarget.gameObject.transform.DOScale(2f, num * 0.5f);
		dicCollectObjs[collectBlockType].ImageTarget.gameObject.transform.DOScale(1f, num).SetDelay(num * 0.5f);
		if (base.gameObject.activeSelf && enableCreateCollectEffect)
		{
			enableCreateCollectEffect = false;
			Invoke("SetEnableCreateCollectEffect", 0.1f);
			GameObject spawnEffectObject = SpawnStringEffect.GetSpawnEffectObject(SpawnStringEffectType.BringDownUIEffectDecrease);
			if ((bool)spawnEffectObject)
			{
				spawnEffectObject.transform.parent = dicCollectObjs[collectBlockType].ImageTarget.transform;
				spawnEffectObject.transform.localPosition = new Vector3(0f, 0f, -10f);
				PoolManager.PoolGameEffect.Despawn(spawnEffectObject.transform, 0.9f);
			}
		}
	}

	private void SetEnableCreateCollectEffect()
	{
		enableCreateCollectEffect = true;
	}

	public Vector3 GetCollectObjectPosition(CollectBlockType collectBlockType)
	{
		if (dicCollectObjs.ContainsKey(collectBlockType))
		{
			if (dicCollectObjs[collectBlockType] == null)
			{
				return Vector3.zero;
			}
			return dicCollectObjs[collectBlockType].ImageTarget.transform.position;
		}
		if (ObjCollectParent == null)
		{
			return Vector3.zero;
		}
		return ObjCollectParent.transform.position;
	}

	public void UpdateTextBoosterCount()
	{
		if (boosterUI != null)
		{
			for (int i = 0; i < boosterUI.Length; i++)
			{
				boosterUI[i].UpdateTextBoosterCount();
			}
		}
	}

	public IEnumerator BuyEffect(SpawnStringEffectType EffectType, GameObject effTargetObject, int NumOfItem, AppEventManager.ItemEarnedBy earnedBy, Vector3 startPos = default(Vector3))
	{
		GameObject effAddMove = SpawnStringEffect.GetSpawnEffectObject(EffectType);
		if (!effAddMove)
		{
			yield return null;
		}
		GameMain.main.throwingMoveEffectCount++;
		effAddMove.transform.position = Camera.main.transform.position;
		if (MapData.main.target == GoalTarget.Digging)
		{
			effAddMove.transform.position = startPos;
		}
		effAddMove.transform.localScale = Vector3.zero;
		effAddMove.GetComponentInChildren<SpriteRenderer>().gameObject.layer = LayerMask.NameToLayer("GameEffect");
		effAddMove.transform.DOScale(Vector3.one, 0.5f);
		PoolManager.PoolGameEffect.Despawn(effAddMove.transform, throwEffectTime + 0.5f);
		SoundSFX.Play(SFXIndex.BuyBooster);
		yield return new WaitForSeconds(0.5f);
		Ease xEaseThrowItem = Ease.InOutBack;
		Ease yEaseThrowItem = Ease.InOutCubic;
		if (CPanelGameUI.Instance.IsOrientationPortrait)
		{
			Transform transform = effAddMove.transform;
			Vector3 gameCameraPosition = GetGameCameraPosition(effTargetObject);
			transform.DOMoveX(gameCameraPosition.x, throwEffectTime).SetEase(xEaseThrowItem);
			Transform transform2 = effAddMove.transform;
			Vector3 gameCameraPosition2 = GetGameCameraPosition(effTargetObject);
			transform2.DOMoveY(gameCameraPosition2.y, throwEffectTime).SetEase(yEaseThrowItem);
		}
		else
		{
			Transform transform3 = effAddMove.transform;
			Vector3 gameCameraPosition3 = GetGameCameraPosition(effTargetObject);
			transform3.DOMoveX(gameCameraPosition3.x, throwEffectTime).SetEase(yEaseThrowItem);
			Transform transform4 = effAddMove.transform;
			Vector3 gameCameraPosition4 = GetGameCameraPosition(effTargetObject);
			transform4.DOMoveY(gameCameraPosition4.y, throwEffectTime).SetEase(xEaseThrowItem);
		}
		ShortcutExtensions.DOScale(endValue: effAddMove.transform.localScale * 1.18f, target: effAddMove.transform, duration: throwEffectTime * 0.5f);
		effAddMove.transform.DOScale(0.5f, throwEffectTime * 0.5f).SetDelay(throwEffectTime * 0.5f);
		yield return new WaitForSeconds(throwEffectTime);
		SoundSFX.Play(SFXIndex.IncreaseBooster);
		switch (EffectType)
		{
		case SpawnStringEffectType.SuccessBuyMove:
			GameMain.main.IncreaseMoveCount(NumOfItem);
			if (NumOfItem == 5)
			{
				GameMain.main.DoGameContinue();
			}
			break;
		case SpawnStringEffectType.SuccessBuyMagicHammer:
		case SpawnStringEffectType.SuccessBuyCandyPack:
		case SpawnStringEffectType.SuccessBuyBoosterHBomb:
		case SpawnStringEffectType.SuccessBuyBoosterVBomb:
		{
			Booster.BoosterType boosterType = Booster.BoosterType.Hammer;
			switch (EffectType)
			{
			case SpawnStringEffectType.SuccessBuyCandyPack:
				boosterType = Booster.BoosterType.CandyPack;
				break;
			case SpawnStringEffectType.SuccessBuyBoosterHBomb:
				boosterType = Booster.BoosterType.HBomb;
				break;
			case SpawnStringEffectType.SuccessBuyBoosterVBomb:
				boosterType = Booster.BoosterType.VBomb;
				break;
			}
			MonoSingleton<PlayerDataManager>.Instance.IncreaseBoosterData(boosterType, NumOfItem, earnedBy);
			CPanelGameUI.Instance.UpdateTextBoosterCount();
			break;
		}
		default:
			CPanelGameUI.Instance.UpdateTextBoosterCount();
			break;
		}
		GameObject effect = SpawnStringEffect.GetSpawnEffectObject(SpawnStringEffectType.UIEffectAddMove);
		if ((bool)effect)
		{
			effect.transform.SetParent(effTargetObject.transform);
			effect.transform.localPosition = new Vector3(0f, 0f, -10f);
			PoolManager.PoolGameEffect.Despawn(effect.transform, 1f);
		}
		GameObject effAdd1MoveText = SpawnStringEffect.GetSpawnEffectObject(SpawnStringEffectType.TextEffectAddStuffs);
		if ((bool)effAdd1MoveText)
		{
			effAdd1MoveText.GetComponent<Text>().text = "+" + NumOfItem;
			effAdd1MoveText.transform.SetParent(effTargetObject.transform);
			effAdd1MoveText.transform.localPosition = Vector3.zero;
			effAdd1MoveText.transform.localScale = Vector3.one;
			PoolManager.PoolGameEffect.Despawn(effAdd1MoveText.transform, textTweenTime);
			effAdd1MoveText.transform.DOLocalMoveY(30f, textTweenTime).SetEase(Ease.OutBack);
		}
		GameMain.main.throwingMoveEffectCount--;
	}

	private Vector3 GetGameCameraPosition(GameObject obj)
	{
		return GameMain.main.GameEffectCamera.ViewportToWorldPoint(GameMain.main.UIGameCamera.WorldToViewportPoint(obj.transform.position));
	}
}
