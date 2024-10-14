using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutGoldButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnPressPutGoldButton()
    {
         SoundSFX.Play(SFXIndex.ButtonClick);
        MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupPutCoinInIt);
        //MonoSingleton<GameDataLoadManager>.Instance.MoveToLobbyScene();
    }
}
