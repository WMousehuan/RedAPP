using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetCurrentLevel : MonoBehaviour
{

    public Text CurrentLevel;

    

    private void Start()
    {
        CurrentLevel.text = "LEVEL " + MonoSingleton<PlayerDataManager>.Instance.CurrentLevelNo;

    }

    public void OnPressAllLevel()
    {
       // SoundSFX.Play(SFXIndex.ButtonClick);
        MonoSingleton<GameDataLoadManager>.Instance.MoveToLobbyScene();
    }

 
}
