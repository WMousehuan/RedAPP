using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEpisodeItemList : MonoBehaviour
{
	private enum EpisodeItemTweenState
	{
		Opening,
		Closing,
		Open,
		Close
	}

	public int EpisodeNo;

	public RectTransform RectPanel;

	public Text TextEpisodeNo;

	public Text TextLevelRange;

	public Image ImageEpisodeState;

	public Transform PosLevelBallParent;

	public GameObject PrefabLevelBall;

	public Sprite SpriteStateLock;

	public Sprite SpriteStateNormal;

	public Sprite SpriteLevelBallCleared;

	private EpisodeItemTweenState tweenState = EpisodeItemTweenState.Close;

	private List<GameObject> listObjLevelBalls = new List<GameObject>();

	private Vector2 baseSizeDelta = Vector2.zero;

	private void Start()
	{
		baseSizeDelta = RectPanel.sizeDelta;
	}

	public void SetData(int episodeNo)
	{
		EpisodeNo = episodeNo;
		TextEpisodeNo.text = "Stage " + EpisodeNo;
		TextLevelRange.text = $"{(EpisodeNo - 1) * 20 + 1}~{(EpisodeNo - 1) * 20 + 20} Levels";
		if ((EpisodeNo - 1) * 20 + 1 > MonoSingleton<PlayerDataManager>.Instance.CurrentLevelNo)
		{
			ImageEpisodeState.sprite = SpriteStateLock;
		}
		else
		{
			ImageEpisodeState.sprite = SpriteStateNormal;
		}
	}

	private void Update()
	{
	}

	public void OnPressButton(bool doNotTween = false)
	{
		if ((EpisodeNo - 1) * 20 + 1 > MonoSingleton<PlayerDataManager>.Instance.CurrentLevelNo)
		{
			return;
		}
		if (tweenState == EpisodeItemTweenState.Close)
		{
			SoundSFX.Play(SFXIndex.ButtonClick);
			for (int i = 0; i < 20; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(PrefabLevelBall);
				gameObject.transform.SetParent(PosLevelBallParent.transform, worldPositionStays: false);
				int num = (EpisodeNo - 1) * 20 + 1 + i;
				gameObject.name = num.ToString();
				gameObject.transform.Find("TextLevel").GetComponent<Text>().text = num.ToString();
				if (num < MonoSingleton<PlayerDataManager>.Instance.CurrentLevelNo)
				{
					gameObject.transform.Find("Current").gameObject.SetActive(value: false);
					gameObject.transform.Find("Disabled").gameObject.SetActive(value: false);
					gameObject.GetComponent<Image>().sprite = SpriteLevelBallCleared;
				}
				else if (num == MonoSingleton<PlayerDataManager>.Instance.CurrentLevelNo)
				{
					gameObject.transform.Find("Current").gameObject.SetActive(value: true);
					gameObject.transform.Find("Disabled").gameObject.SetActive(value: false);
				}
				else
				{
					gameObject.transform.Find("Current").gameObject.SetActive(value: false);
					gameObject.transform.Find("Disabled").gameObject.SetActive(value: true);
				}
				listObjLevelBalls.Add(gameObject);
			}
			ImageEpisodeState.transform.DOLocalRotate(new Vector3(0f, 0f, 90f), 0.4f);
			if (!doNotTween)
			{
				tweenState = EpisodeItemTweenState.Opening;
				RectPanel.DOSizeDelta(new Vector2(baseSizeDelta.x, 750f), 0.5f).OnComplete(delegate
				{
					tweenState = EpisodeItemTweenState.Open;
				});
			}
			else
			{
				tweenState = EpisodeItemTweenState.Open;
				RectPanel.sizeDelta = new Vector2(baseSizeDelta.x, 750f);
			}
		}
		else if (tweenState == EpisodeItemTweenState.Open)
		{
			SoundSFX.Play(SFXIndex.ButtonClick);
			tweenState = EpisodeItemTweenState.Closing;
			ImageEpisodeState.transform.DOLocalRotate(Vector3.zero, 0.4f);
			RectPanel.DOSizeDelta(new Vector2(baseSizeDelta.x, baseSizeDelta.y), 0.5f).OnComplete(delegate
			{
				foreach (GameObject listObjLevelBall in listObjLevelBalls)
				{
					UnityEngine.Object.Destroy(listObjLevelBall);
				}
				listObjLevelBalls.Clear();
				tweenState = EpisodeItemTweenState.Close;
			});
		}
	}
}
