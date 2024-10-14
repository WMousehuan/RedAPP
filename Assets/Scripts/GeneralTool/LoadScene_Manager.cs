using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene_Manager : Singleton_Base<LoadScene_Manager>
{
    public static string currentSceneName
    {
        get
        {
            return SceneManager.GetActiveScene().name;
        }
    }
    public override bool isDontDestroy => true;

    public delegate void FloatDelegate(float value);
    public delegate bool ReturnBoolDelegate();
    private AsyncOperation asyncOperation;

    private IEnumerator loadIE;
    public  void LoadScene(string sceneName)
    {
        //currentSceneName = sceneName;
        DestoryLoadSceneSync();
        SceneManager.LoadScene(sceneName);
        IEPool_Manager.instance.Clear();
    }

    public void LoadSceneSync(string sceneName, FloatDelegate loadingAction = null, System.Action loadFinishAction = null, ReturnBoolDelegate allowSceneActivationAction = null)
    {
        //currentSceneName = sceneName;
        DestoryLoadSceneSync();
        asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = false;
        loadIE = LoadingSceneSync(loadingAction, loadFinishAction, allowSceneActivationAction);
        StartCoroutine(loadIE);
    }
    public IEnumerator LoadingSceneSync(FloatDelegate loadingAction = null,System.Action loadFinishAction=null, ReturnBoolDelegate allowSceneActivationAction = null)
    {
        while (asyncOperation.progress < 0.9f)
        {
            loadingAction?.Invoke(asyncOperation.progress / 0.9f);
            yield return null;
        }
        loadingAction?.Invoke(1);
        loadFinishAction?.Invoke();
        while (allowSceneActivationAction != null && !(allowSceneActivationAction?.Invoke()).Value)
        {
            yield return null;
        }
        IEPool_Manager.instance.Clear();
        asyncOperation.allowSceneActivation = true;

    }
    public void DestoryLoadSceneSync()
    {
        if (loadIE != null)
        {
            StopCoroutine(loadIE);
        }
        if (asyncOperation != null)
        {
            asyncOperation.allowSceneActivation = false;
            asyncOperation = null;
        }
    }
}
