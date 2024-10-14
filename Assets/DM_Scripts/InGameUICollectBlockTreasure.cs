/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUICollectBlockTreasure : InGameUICollectBlock
{
	public GameObject ObjBaseTreasure;

	public Sprite[] SpriteTreasuresByGrade;

	public Sprite[] SpriteTreasuresDisableByGrade;

	public Sprite SpriteTreasure;

	private List<Image> listImageTreasures = new List<Image>();

	private int g1Count;

	private int g2Count;

	private int g3Count;

	private int allCollectCount;

	public void SetData(int _g1_count, int _g2_count, int _g3_count)
	{
		g1Count = _g1_count;
		g2Count = _g2_count;
		g3Count = _g3_count;
		allCollectCount = _g1_count + _g2_count + _g3_count;
		listImageTreasures.Add(ObjBaseTreasure.GetComponent<Image>());
		for (int i = 1; i < allCollectCount; i++)
		{
			GameObject gameObject = Object.Instantiate(ObjBaseTreasure);
			gameObject.transform.SetParent(ObjBaseTreasure.transform.parent);
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.name = i.ToString();
			listImageTreasures.Add(gameObject.GetComponent<Image>());
		}
		for (int j = 0; j < _g3_count; j++)
		{
			listImageTreasures[j].sprite = SpriteTreasuresDisableByGrade[2];
		}
		for (int k = 0; k < _g2_count; k++)
		{
			listImageTreasures[_g3_count + k].sprite = SpriteTreasuresDisableByGrade[1];
		}
		for (int l = 0; l < _g1_count; l++)
		{
			listImageTreasures[_g3_count + _g2_count + l].sprite = SpriteTreasuresDisableByGrade[0];
		}
	}

	public override void UpdateTargetCount(int _targetCount, int param1 = 0)
	{
		int num = 0;
		switch (param1)
		{
		case 3:
			num = g3Count - _targetCount - 1;
			break;
		case 2:
			num = g3Count + (g2Count - _targetCount - 1);
			break;
		case 1:
			num = g3Count + g2Count + (g1Count - _targetCount - 1);
			break;
		}
		if (num < listImageTreasures.Count)
		{
			listImageTreasures[num].sprite = SpriteTreasuresByGrade[param1 - 1];
			Sequence sequence = DOTween.Sequence();
			sequence.Append(listImageTreasures[num].transform.DOScale(2f, 0.2f));
			sequence.Append(listImageTreasures[num].transform.DOScale(1f, 0.2f));
			sequence.Play();
		}
	}

	public override void Reset()
	{
	}
}
