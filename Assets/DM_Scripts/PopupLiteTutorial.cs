
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine;
using UnityEngine.UI;

public class PopupLiteTutorial : Popup
{
	private string[] tutorialDesc = new string[8]
	{
		"Make match 3 same Fruit!",
		"Match the Fruit within the Jail!",
		"Match the Fruit around the Blocks.",
		"Match the Fruit on the Tile.",
		"Match the Fruit on the Gold to spread it.",
		"Bring All Items down to the bottom of the board.",
		"Find the Golden Teapot behind the Tiles!",
		"Match the Fruit & Find the Golden Teapot!"
	};

	private string[] tutorialTitle = new string[0];

	public GameObject[] ObjTutorailGroups;

	public Text TextTutorialTitle;

	public Text TextTutorialDesc;

	public void SetData(int tutorialIndex)
	{
		ObjTutorailGroups[tutorialIndex].SetActive(value: true);
		TextTutorialDesc.text = tutorialDesc[tutorialIndex];
	}

    
}
