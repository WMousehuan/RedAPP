
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoSingleton<LobbyManager>
{
	private bool isFirstLoad = false;

	private bool isInit;

	public static bool afterFailed;

	//public Text currentLevel;
	public CanvasScaler canvasScaler;

	private string getChannelUrl = "/app-api/red/channel/get";

    public ChannelDataVO currnetChannelData;
    public override void Awake()
	{
        isFirstLoad = false;
        base.Awake();
	}

	private void Start()
	{
        UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
        UtilJsonHttp.Instance.GetRequestWithAuthorizationToken(getChannelUrl + "?id=" + PlayerTreasureGameData.Instance.entranceChannelId, new CommonHttpInterface(), (resultData) =>
        {
            ReturnData<ChannelDataVO> result = JsonConvert.DeserializeObject<ReturnData<ChannelDataVO>>(resultData);
            currnetChannelData = result.data;
            waitMask_Ui?.ShowResultCase("Success", 0);
        }, () => {
            waitMask_Ui?.ShowResultCase("Fail", 1, () => {
                //MonoSingleton<SceneControlManager>.Instance.LoadScene(SceneType.Title, SceneChangeEffect.Color);
            });
        });
    }

	private void OnDisable()
	{
	}

	private void OnEnable()
	{
		StopAllCoroutines();
		StartCoroutine(OnEnableCoroutine());
	}

	public void OnEventRecvLevelData()
	{
	}
	private void OnDestroy()
	{
	}

	private IEnumerator OnEnableCoroutine()
	{
		isInit = true;
		isFirstLoad = false;
		yield return null;
	}

	public void OnPressStartLevel()
	{
		//SoundSFX.Play(SFXIndex.ButtonClick);
		//MonoSingleton<PlayerDataManager>.Instance.lastPlayedLevel = MonoSingleton<PlayerDataManager>.Instance.CurrentLevelNo;
		//MapData.main = new MapData(MonoSingleton<PlayerDataManager>.Instance.CurrentLevelNo);
		//MonoSingleton<SceneControlManager>.Instance.LoadScene(SceneType.Game, SceneChangeEffect.Color);
		//GameMain.CompleteGameStart();
	}

	private void Update()
	{
		if (isInit)
		{
		}
        canvasScaler.matchWidthOrHeight = Mathf.Lerp(1, 0, ((Screen.height / (float)Screen.width) - (4 / 3f)) / ((16 / 9f) - (4 / 3f)));

    }

    private IEnumerator processStageClearEffect()
	{
		yield return null;
		yield return null;
	}
}
public class ChannelDataVO
{
	public int id;//Ƶ��id
	public int channelCateId;//��Ϸ����id
	public int channelType;//��Ϸ����
	public string channelName;//��Ϸ����
	public string channelCategory;//��Ϸ����
	public double redMaxMoney;//�������С
	public double redMinMoney;//��С�����С
	public int redExpirationTime;//�������ʱ��
	public int memberId;//����id
    public string createTime;//����ʱ��
}
