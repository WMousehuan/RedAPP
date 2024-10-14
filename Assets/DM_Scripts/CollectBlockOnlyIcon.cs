/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine;
using UnityEngine.UI;

public class CollectBlockOnlyIcon : MonoBehaviour
{
	public Image ImageTarget;

	public void SetData(CollectBlockType _collectType, Sprite _targetSprite)
	{
		ImageTarget.rectTransform.sizeDelta = new Vector2(_targetSprite.rect.width, _targetSprite.rect.height);
		ImageTarget.sprite = _targetSprite;
	}
}
