#if ENABLE_MEC
using MEC;
#endif
using MovementEffects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneControlManager : MonoSingleton<SceneControlManager>
{
    [HideInInspector]
    public SceneClass CurrentScene;

    [HideInInspector]
    public SceneType CurrentSceneType = SceneType.None;

    private readonly Dictionary<SceneType, SceneClass> dicAllSceneClass = new Dictionary<SceneType, SceneClass>();

    private bool isLoadGamePool;

    public bool IsLoadingScene;

    private EventSystem MainEventSystem;

    private SceneClass OldScene;
    private string lastSceneName = null;


    [HideInInspector]
    public SceneType OldSceneType = SceneType.None;

    public UISceneLoading sceneChangeLoading;

    public void LoadScene(SceneType newSceneType, SceneChangeEffect sceneChangeEffect = SceneChangeEffect.None, Color fadeColor = default(Color))
    {
        print(lastSceneName+"||"+ newSceneType);
        if (lastSceneName != null)
        {
            if (dicAllSceneClass.ContainsKey(OldSceneType))
            {
                dicAllSceneClass.Remove(OldSceneType);
            }
            SceneManager.UnloadSceneAsync(lastSceneName);
            lastSceneName = null;
        }
        if (newSceneType != CurrentSceneType)
        {
            if ((bool)EventSystem.current)
            {
                MainEventSystem = EventSystem.current;
                //EventSystem.current.enabled = false;
            }
            OldScene = CurrentScene;
            OldSceneType = CurrentSceneType;
            StartCoroutine(LoadSceneAsync(newSceneType, sceneChangeEffect, fadeColor));
        }
    }

    private IEnumerator LoadSceneAsync(SceneType newSceneType, SceneChangeEffect sceneChangeEffect, Color fadeColor = default(Color))
    {

        IsLoadingScene = true;

        if (CurrentSceneType == SceneType.FirstLoad)
        {
            MonoSingleton<PopupManager>.Instance.CloseAllPopup();
            if (fadeColor == default(Color))
            {
                fadeColor = Color.white;
            }
            if (sceneChangeEffect != SceneChangeEffect.None)
            {
                //Debug.Log("LOAD FROM FIRST SCENE");

                if ((bool)sceneChangeLoading)
                {
                    yield return StartCoroutine(sceneChangeLoading.FadeIn(newSceneType, sceneChangeEffect, null, fadeColor));
                }
                while (sceneChangeLoading.CurFadeState == UISceneLoading.FadeState.FadeIn)
                {
                    yield return null;
                }
            }
        }
        if (CurrentSceneType != 0)
        {

            MonoSingleton<PopupManager>.Instance.CloseAllPopup();
            if (fadeColor == default(Color))
            {
                fadeColor = Color.white;
            }
            if (sceneChangeEffect != SceneChangeEffect.None)
            {
                //Debug.Log("CHAY VAO DSA");

                if ((bool)sceneChangeLoading)
                {
                    yield return StartCoroutine(sceneChangeLoading.FadeIn(newSceneType, sceneChangeEffect, null, fadeColor));
                }
                while (sceneChangeLoading.CurFadeState == UISceneLoading.FadeState.FadeIn)
                {
                    yield return null;
                }
            }
            yield return null;
            if ((bool)OldScene)
            {
                OldScene.OnSceneHideStart();
            }
        }
        if (CurrentSceneType == SceneType.Game)
        {
#if ENABLE_MEC
			Timing.KillCoroutines();
#endif
        }
        if ((bool)OldScene)
        {
            OldScene.OnSceneHideEnd();
            if (OldScene.DontDestroyAtSceneChange)
            {
                OldScene.gameObject.SetActive(value: false);
            }
            else
            {
                if (dicAllSceneClass.ContainsKey(OldSceneType))
                {
                    dicAllSceneClass.Remove(OldSceneType);
                }
                UnityEngine.Object.Destroy(OldScene.gameObject);
                OldScene = null;
            }
        }
        if (sceneChangeEffect != SceneChangeEffect.None)
        {
            yield return null;
        }
        Resources.UnloadUnusedAssets();
        GC.Collect();
        if (!dicAllSceneClass.ContainsKey(newSceneType))
        {
            switch (newSceneType)
            {
                case SceneType.Lobby:
                    SoundManager.StopSFX();
                    
                    yield return SceneManager.LoadSceneAsync("Lobby", LoadSceneMode.Additive);
                    lastSceneName = "Lobby";
                    break;
                case SceneType.Game:
                    if (!isLoadGamePool)
                    {
                        yield return SceneManager.LoadSceneAsync("GamePool", LoadSceneMode.Additive);
                        isLoadGamePool = true;
                    }
               
                    yield return SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
                    lastSceneName = "Game";
                    break;
                case SceneType.Title:
                    lastSceneName = SceneType.Lobby.ToString();
                    yield return SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive);
                    lastSceneName = "Menu";
                    break;
            }
        }
        else
        {
            CurrentScene = dicAllSceneClass[newSceneType];
            CurrentSceneType = newSceneType;
            if ((bool)CurrentScene && (bool)CurrentScene.gameObject)
            {
                CurrentScene.gameObject.SetActive(value: true);
            }
            switch (newSceneType)
            {
                case SceneType.Game:
                    break;
            }
        }
        yield return null;
        if (!(CurrentScene == null))
        {
            if (!dicAllSceneClass.ContainsKey(newSceneType))
            {
                dicAllSceneClass.Add(newSceneType, CurrentScene);
            }
            CurrentSceneType = newSceneType;
            if (OldSceneType == SceneType.Title && CurrentSceneType == SceneType.Lobby && MonoSingleton<PlayerDataManager>.Instance.CurrentLevelNo == 1000000000000)
            {
                MapData.main = new MapData(1);
                GameMain.CompleteGameStart();
                LoadScene(SceneType.Game, SceneChangeEffect.Color);
                yield break;
            }
            yield return StartCoroutine(CurrentScene.OnSceneShow());
        }
        if (sceneChangeEffect != SceneChangeEffect.None)
        {
            sceneChangeLoading.FadeOut();
            while (sceneChangeLoading.CurFadeState == UISceneLoading.FadeState.FadeOut)
            {
                yield return null;
            }
        }
        if ((bool)MainEventSystem)
        {
            MainEventSystem.enabled = true;
        }
        IsLoadingScene = false;
    }

    public void RemoveCurrentScene()
    {
        if ((bool)CurrentScene)
        {
            CurrentScene.OnSceneHideStart();
            CurrentScene.OnSceneHideEnd();
        }
        if (CurrentSceneType == SceneType.Game)
        {
#if ENABLE_MEC
			Timing.KillCoroutines();
#endif
        }
        if ((bool)CurrentScene && CurrentScene.DontDestroyAtSceneChange)
        {
            CurrentScene.gameObject.SetActive(value: false);
        }
        else
        {
            if (dicAllSceneClass.ContainsKey(CurrentSceneType))
            {
                dicAllSceneClass.Remove(CurrentSceneType);
            }
            if ((bool)CurrentScene)
            {
                //SceneManager.UnloadSceneAsync(OldSceneType.ToString());
                UnityEngine.Object.Destroy(CurrentScene.gameObject);
            }
            CurrentScene = null;
        }
        CurrentScene = OldScene;
        CurrentSceneType = OldSceneType;
    }

    public void SetFirstMapToolSet(SceneClass mapToolSceneClass)
    {
        dicAllSceneClass.Add(SceneType.MapTool, mapToolSceneClass);
        CurrentScene = mapToolSceneClass;
        CurrentSceneType = SceneType.MapTool;
    }
}
