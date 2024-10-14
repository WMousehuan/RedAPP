using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombNumberHorizontal : MonoBehaviour
{
    public GameObject bombNumberCheckBoxGameobject;
    public NumberOfPlayerHorizontal numberOfPlayerHorizontal;
    List<BombNumberCheckBox> playersCheckBoxList = new List<BombNumberCheckBox>();
    // Start is called before the first frame update
    public void startInitPlayersNumberCHeckbox()
    {
        for (int i = 0; i < 10; i++)
        {
                GameObject newCHeckBox = Instantiate(bombNumberCheckBoxGameobject, gameObject.transform);
                BombNumberCheckBox playerCheckBox = newCHeckBox.GetComponent<BombNumberCheckBox>();
                playerCheckBox.id = i;
                if (i == 0)
                {
                    playerCheckBox.SetToggleOn();
                }
                playerCheckBox.setNumberText(i.ToString());
                playerCheckBox.setPlayersCheckBoxList(playersCheckBoxList);
                playersCheckBoxList.Add(playerCheckBox);
        }
    }

    public void redPkgCLickChangeRatio(double compensateRatio)
    {
        if (playersCheckBoxList != null)
        {
            foreach (var item in playersCheckBoxList)
            {
                item.setCompensateRatioText("{"+ compensateRatio.ToString()+ "X}");
            }
        }
    }

    public BombNumberCheckBox getBombNumberCheckBox()
    {
        if (playersCheckBoxList == null || playersCheckBoxList.Count == 0)
        {
            return null;
        }
        foreach (var player in playersCheckBoxList)
        {
            if (player.getToggleIsOn())
            {
                return player;
            }
        }
        return null;
    }
}
