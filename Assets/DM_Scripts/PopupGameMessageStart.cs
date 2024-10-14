
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine;
using UnityEngine.UI;

public class PopupGameMessageStart : PopupGameMessage
{
	public GameObject PrefabCollect;

	public Transform TargetList;

	public Text TextTreasureCountForDailyBonusLevel;

	public override void Start()
	{
		base.Start();
		if (TargetList != null)
		{
			for (int i = 0; i < MapData.main.collectBlocks.Length; i++)
			{
				if (string.IsNullOrEmpty(MapData.main.collectBlocks[i].blockType))
				{
					continue;
				}
				GameObject gameObject = Object.Instantiate(PrefabCollect);
				if (!gameObject)
				{
					continue;
				}
				CollectBlockOnlyIcon component = gameObject.GetComponent<CollectBlockOnlyIcon>();
				if ((bool)component)
				{
					component.gameObject.transform.SetParent(TargetList, worldPositionStays: false);
					CollectBlockType collectBlockType = MapData.main.collectBlocks[i].GetCollectBlockType();
					Sprite sprite;
					switch (collectBlockType)
					{
					case CollectBlockType.NormalRed:
					case CollectBlockType.NormalOrange:
					case CollectBlockType.NormalYellow:
					case CollectBlockType.NormalGreen:
					case CollectBlockType.NormalBlue:
					case CollectBlockType.NormalPurple:
						sprite = Resources.Load<Sprite>(CPanelGameUI.GetCollectIconNormalBlockName(MapData.main.collectBlocks[i].blockType));
						break;
					default:
						sprite = Resources.Load<Sprite>("UI/CollectIcon/" + MapData.main.collectBlocks[i].blockType);
						break;
					case CollectBlockType.Null:
						continue;
					}
					if (sprite != null)
					{
						component.SetData(collectBlockType, sprite);
					}
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(component);
				}
			}
		}
		//TextMessage.font = MonoSingleton<UIManager>.Instance.GetLocalizeFont();
	}
}
