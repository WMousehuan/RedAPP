using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Singleton_Base : MonoBehaviour
{
}

public abstract class Singleton_Base<T> : Singleton_Base where T: Singleton_Base, new()
{
    public static string className
    {
        get
        {
            return typeof(T).ToString();
        }
    }
    //当前正在运行的对象
    public static Dictionary<string, GameObject> aliveGameObject = new Dictionary<string, GameObject>();
    //软件是否退出运行
    private static bool isApplictionQuit=false;

    //是否是因为场景上有多个相同类型的删除(是的话会做特殊处理)
    protected static bool isMultipleDestory;
    private static string currentScene;

    //是否在跨场景时删除所挂物体
    public abstract bool isDontDestroy { get; }
    private static T _instance;
    public static T instance
    {
        get
        {
            if (isApplictionQuit)
            {
                return null;
            }
            if (_instance == null)
            {
                GameObject prefab = Resources.Load<GameObject>("Pack/Singleton/" + typeof(T).ToString());
                GameObject gameObj = null;
                if (prefab)
                {
                    gameObj = Instantiate(prefab);
                    _instance = gameObj.GetComponent<T>();
                }
                if (!gameObj)
                {
                    gameObj = new GameObject();
                    _instance = gameObj.AddComponent<T>();
                }
                gameObj.name = typeof(T).ToString();
     
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (!aliveGameObject.ContainsKey(className))
        {
            //print("+" + typeof(T).ToString());
            if (isDontDestroy)
            {
                aliveGameObject.Add(className, this.gameObject);
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            MultipleDestory();
            return;
        }
        if (_instance == null)
        {
            _instance = this.GetComponent<T>();
        }
    }
    public void MultipleDestory()
    {
        isMultipleDestory = true;
        Destroy(this.gameObject);
    }
    protected virtual void OnDestroy()
    {
        if (isMultipleDestory)
        {
            isMultipleDestory = false;
        }
        else
        {
            print("-" + this.gameObject);

            _instance = null;
            if (isDontDestroy)
            {
                isApplictionQuit = true;
            }
        }

    }

}
