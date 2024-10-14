/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using PathologicalGames;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CPanelGameUI : CPanel
{
	public enum GameCameraMaskingType
	{
		SweetRoad,
		Digging
	}

	private static CPanelGameUI _instance;

	private static readonly float MaskBoardOffsetValue = 312f;

	private int _currentMoveCount;

	private int _currentScore;

	public CanvasScaler BGCanvasScaler;

	public CanvasScaler BGMaskCanvasScaler;

	public Image ImageBGMask;

	public Image ImageGameBG;

	public Image ImageGameMaskBG;

	[HideInInspector]
	public bool IsOrientationPortrait;

	public bool isPlayingBoosterItem;

	private float lerpScoreTimeDuration;

	public GameObject ObjBGMask;

	public UIOptionButton ObjOptionButton;

	public Tweener optionButtonTweener;

	private readonly Color32 orangeColor = new Color32(byte.MaxValue, 164, 0, byte.MaxValue);

	private dotmob.JewelHunter.Gradient portraitMoveCountGradient;

	public GameObject PrefabEffectGetStar;

	private readonly Color32 redColor = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);

	private readonly float[] starPoint = new float[3];

	private int targetScore;

	private int oldScore;

	public GameObject TestVersionWatermark;

	public InGameUIOrientationPortrait varPortrait;

	private readonly Color32 yellowColor = new Color32(byte.MaxValue, byte.MaxValue, 36, byte.MaxValue);

    [SerializeField]
    private InGameUIBoosterUsingGuide _boosterGuide;
    public InGameUIBoosterUsingGuide BoosterGuide
    {
        get { return _boosterGuide; }
        set { _boosterGuide = value; }
    }


    public static CPanelGameUI Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = (Object.FindObjectOfType(typeof(CPanelGameUI)) as CPanelGameUI);
				if (!(_instance == null))
				{
				}
			}
			return _instance;
		}
	}

	private int currentScore
	{
		get
		{
			return _currentScore;
		}
		set
		{
			_currentScore = value;
			varPortrait.TextScore.text = Utils.GetCurrencyNumberString(value);
		}
	}

	private int currentMoveCount
	{
		get
		{
			return _currentMoveCount;
		}
		set
		{
			_currentMoveCount = value;
			varPortrait.TextMoveCount.text = _currentMoveCount.ToString();
			if (_currentMoveCount <= 5 && !GameMain.main.isBonusTime)
			{
#if !UNITY_2018_3_OR_NEWER
                varPortrait.TextMoveCount.GetComponent<Gradient>().ChangeColor(orangeColor, redColor);
#endif
                Sequence sequence = DOTween.Sequence();
				sequence.Append(varPortrait.TextMoveCount.transform.DOScale(2f, 0.2f));
				sequence.Append(varPortrait.TextMoveCount.transform.DOScale(1f, 0.2f));
				sequence.Play();
			}
			else
			{
#if !UNITY_2018_3_OR_NEWER
				portraitMoveCountGradient.ChangeColor(yellowColor, orangeColor);
#endif
			}
		}
	}

	public void Awake()
	{
		_instance = this;
		portraitMoveCountGradient = varPortrait.TextMoveCount.GetComponent<dotmob.JewelHunter.Gradient>();
	}

	public void Start()
	{
		if ((bool)TestVersionWatermark)
		{
			TestVersionWatermark.SetActive(value: false);
		}
	}

	public void ShowUITween()
	{
		varPortrait.ShowUITween();
	}

	public void HideUITween()
	{
		varPortrait.HideUITween();
	}

	public Vector3 GetBoardPosition()
	{
		Vector3 result = Vector3.zero;
		result = varPortrait.GetBoardPosition();
		result.z = 0f;
		
		return result;
	}

	public IEnumerator waitChangedOrientation(bool isPortrait)
	{
		varPortrait.gameObject.SetActive(value: true);
		bool isInit = varPortrait.IsInit;
		while (!varPortrait.IsInit)
		{
			yield return null;
		}
		if (!isInit)
		{
			varPortrait.ShowUITween();
		}
		IsOrientationPortrait = isPortrait;
		UpdateTextBoosterCount();
		StartCoroutine(delayedSetBoardPositionAndBGScalar(isPortrait: true));
		foreach (InGameUICollectBlock value in varPortrait.dicCollectObjs.Values)
		{
			if (value != null && value.gameObject != null)
			{
				ParticleSystem[] componentsInChildren = value.GetComponentsInChildren<ParticleSystem>();
				foreach (ParticleSystem particleSystem in componentsInChildren)
				{
					if (PoolManager.PoolGameEffect.IsSpawned(particleSystem.transform))
					{
						PoolManager.PoolGameEffect.Despawn(particleSystem.transform);
					}
				}
			}
		}
	}

	public void SetBGCanvasScalar()
	{
		BGCanvasScaler.matchWidthOrHeight = Mathf.Clamp01((float)Screen.height / (float)Screen.width / 1.5f);
	}

	private IEnumerator delayedSetBoardPositionAndBGScalar(bool isPortrait)
	{
		yield return null;
		if ((bool)BoardManager.main.slotGroup)
		{
			BoardManager.main.slotGroup.transform.position = GetBoardPosition();
		}
	}

	public override void Update()
	{
		base.Update();
		if (oldScore < targetScore)
		{
			currentScore = (int)Mathf.Lerp(oldScore, targetScore, lerpScoreTimeDuration);
			lerpScoreTimeDuration += Time.deltaTime * 4f;
			varPortrait.UpdateStarPoint(currentScore);
		}
	}

	public void Reset(int startScore, int moveCount, int[] starPosition, MapDataCollectBlock[] collectBlocks, int gid)
	{
		currentScore = (targetScore = (oldScore = startScore));
		SetMoveCount(moveCount);
		varPortrait.Reset(collectBlocks, starPosition, gid);
	}

	public void SetTargetScore(int targetScore)
	{
		oldScore = currentScore;
		this.targetScore = targetScore;
		lerpScoreTimeDuration = 0f;
	}

	public void SetMoveCount(int moveCount)
	{
		currentMoveCount = moveCount;
	}

	public void UpdateTextBoosterCount()
	{
		varPortrait.UpdateTextBoosterCount();
	}

	public void UpdateCollect(CollectBlockType collectBlockType, int count)
	{
		varPortrait.UpdateCollect(collectBlockType, count);
	}

	public Vector3 GetCollectObjectPosition(CollectBlockType collectBlockType)
	{
		return varPortrait.GetCollectObjectPosition(collectBlockType);
	}

	public Vector3 GetCollectObjectGameCameraPosition(CollectBlockType collectBlockType)
	{
        //return GameMain.main.GameEffectCamera.ViewportToWorldPoint(GameMain.main.UIGameCamera.WorldToViewportPoint(GetCollectObjectPosition(collectBlockType)));
        return GameMain.main.GameEffectCamera.ViewportToWorldPoint(GameMain.main.UIGameCamera.WorldToViewportPoint(GetCollectObjectPosition(collectBlockType)));
    }

	public Sprite InitGameCameraMasking(GameCameraMaskingType maskingType)
	{
		float num4;
		float num3;
		float num2;
		float num;
		float num5;
		float num6 = num5 = (num4 = (num3 = (num2 = (num = 0f))));
		switch (maskingType)
		{
		case GameCameraMaskingType.SweetRoad:
			if (Screen.width < Screen.height)
			{
				Vector3 vector5 = GameMain.main.UIGameCamera.WorldToViewportPoint(varPortrait.AnchorBoardPositionTop.position);
				float y = vector5.y;
				Vector3 vector6 = GameMain.main.UIGameCamera.WorldToViewportPoint(varPortrait.AnchorBoardPositionBottom.position);
				num5 = y - vector6.y;
				Vector3 vector7 = GameMain.main.UIGameCamera.WorldToViewportPoint(varPortrait.AnchorBoardPositionTop.position);
				num4 = vector7.y * (float)Screen.height;
				num3 = num4 - num5 * (float)Screen.height;
				num2 = 0f;
				num = Screen.width;
			}
			else
			{
				num6 = (float)Screen.height / (float)Screen.width;
				float num7 = (1f - num6) / 2f;
				Vector3 vector8 = Camera.main.WorldToViewportPoint(BoardManager.main.slotGroup.transform.position);
				num2 = (num7 - (0.5f - vector8.x)) * (float)Screen.width;
				num = num2 + num6 * (float)Screen.width;
				num4 = Screen.height;
				num3 = 0f;
			}
			break;
		case GameCameraMaskingType.Digging:
		{
			Vector3 vector = Camera.main.WorldToViewportPoint(BoardManager.main.slotGroup.transform.position + new Vector3(0f, MaskBoardOffsetValue - 1f, 0f));
			num4 = vector.y * (float)Screen.height;
			Vector3 vector2 = Camera.main.WorldToViewportPoint(BoardManager.main.slotGroup.transform.position - new Vector3(0f, MaskBoardOffsetValue, 0f));
			num3 = vector2.y * (float)Screen.height;
			Vector3 vector3 = Camera.main.WorldToViewportPoint(BoardManager.main.slotGroup.transform.position - new Vector3(MaskBoardOffsetValue, 0f, 0f));
			num2 = vector3.x * (float)Screen.width;
			Vector3 vector4 = Camera.main.WorldToViewportPoint(BoardManager.main.slotGroup.transform.position + new Vector3(MaskBoardOffsetValue - 1f, 0f, 0f));
			num = vector4.x * (float)Screen.width;
			break;
		}
		}
		Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.Alpha8, mipChain: false);
		byte[] array = new byte[texture2D.width * texture2D.height];
		for (int i = 0; i < texture2D.height; i++)
		{
			for (int j = 0; j < texture2D.width; j++)
			{
				array[i * texture2D.width + j] = (byte)((!((float)i >= num3) || !((float)i <= num4) || !((float)j >= num2) || !((float)j <= num)) ? byte.MaxValue : 0);
			}
		}
		texture2D.LoadRawTextureData(array);
		texture2D.Apply();
		Sprite sprite = null;
		return MonoSingleton<UIManager>.Instance.spriteBGMaskPortrait = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), Vector2.zero, 1f);
	}

	public void EnableGameCameraMasking(GameCameraMaskingType maskingType)
	{
		Sprite sprite = null;
		sprite = MonoSingleton<UIManager>.Instance.spriteBGMaskPortrait;
		if (sprite == null)
		{
			sprite = InitGameCameraMasking(maskingType);
		}
		Instance.ImageBGMask.sprite = sprite;
		Instance.ImageGameMaskBG.sprite = Instance.ImageGameBG.sprite;
		Instance.BGMaskCanvasScaler.matchWidthOrHeight = Instance.BGCanvasScaler.matchWidthOrHeight;
		Instance.ObjBGMask.SetActive(value: true);
	}

	public void DisableGameCameraMasking()
	{
		ObjBGMask.SetActive(value: false);
		Instance.ImageGameMaskBG.sprite = null;
	}

	public void ThrowPurchasedItemEffectForDigging(SpawnStringEffectType EffectType, int NumOfItem, Vector3 startPos)
	{
		StartCoroutine(varPortrait.BuyEffect(EffectType, varPortrait.throwingTarget, NumOfItem, AppEventManager.ItemEarnedBy.Other_Reward, startPos));
	}

	public void ThrowPurchasedItemEffect(SpawnStringEffectType EffectType, int NumOfItem)
	{
		StartCoroutine(varPortrait.BuyEffect(EffectType, varPortrait.throwingTarget, NumOfItem, AppEventManager.ItemEarnedBy.Other_Reward));
	}

	public void ThrowPurchasedBoosterItemEffect(SpawnStringEffectType EffectType, int boosterTypeToInt, int NumOfItem, AppEventManager.ItemEarnedBy earnedBy = AppEventManager.ItemEarnedBy.Other_Reward)
	{
		int boosterUiIndexesFromBoosterType = MonoSingleton<PlayerDataManager>.Instance.GetBoosterUiIndexesFromBoosterType((Booster.BoosterType)boosterTypeToInt);
		StartCoroutine(varPortrait.BuyEffect(EffectType, varPortrait.boosterUI[boosterUiIndexesFromBoosterType].gameObject, NumOfItem, earnedBy));
	}

	public static string GetCollectIconNormalBlockName(string blockTypeName)
	{
		return "UI/CollectIcon/" + blockTypeName;
	}
}
