using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class IEPool_Manager : Singleton_Base<IEPool_Manager>
{
    public override bool isDontDestroy => true;
    private Dictionary<string, IEnumerator> iEPool = new Dictionary<string, IEnumerator>();

    private List<CoroutinePack> noNameIEPoolPack = new List<CoroutinePack>();
    public List<string> keys=new List<string>();
    public delegate void Action();
    public delegate bool TimeAction(float time);
    public delegate bool SwitchAction();
    public class DeltaTimeData
    {
        public float deltaTime;
        public float Value
        {
            get
            {
                return deltaTime;
            }
            set
            {
                deltaTime = value;
            }
        }
        public DeltaTimeData(float value)
        {
            this.deltaTime = value;
        }
    }
    protected override void Awake()
    {
        base.Awake();
        //SceneManager.activeSceneChanged += (lastScene, nextScene) =>
        //{
        //    foreach (CoroutinePack coroutinePack in noNameIEPoolPack)
        //    {
        //        if (coroutinePack != null && coroutinePack.ienumerator != null)
        //        {
        //            StopCoroutine(coroutinePack.ienumerator);
        //        }
        //    }
        //    noNameIEPoolPack.Clear();
        //};
    }
    //private void Update()
    //{
    //    deltaTime.deltaTime = Time.deltaTime;
    //}
    //private void LateUpdate()
    //{
    //    lateDeltaTime.deltaTime = Time.deltaTime;
    //}
    //private void FixedUpdate()
    //{
    //    fixedDeltaTime.deltaTime = Time.fixedDeltaTime;
    //}

    /// <summary>
    /// 开始协程
    /// </summary>
    /// <param name="name">名称 同名会覆盖上一个协程执行</param>
    /// <param name="ienumerator">协程</param>
    public  void StartIE(string name, IEnumerator ienumerator)
    {
        if (name != null && name != "")
        {
            if (iEPool.ContainsKey(name))
            {
                StopIE(name);
                iEPool[name] = ienumerator;
            }
            else
            {
                keys.Add(name);
                iEPool.Add(name, ienumerator);
            }
        }
        StartCoroutine(ienumerator);
    }
    public void StartIE(CoroutinePack coroutinePack)
    {
        if (coroutinePack.name != null && coroutinePack.name!="")
        {
            StartIE(coroutinePack.name, coroutinePack.ienumerator);
        }
        else
        {
            noNameIEPoolPack.Add(coroutinePack);
            StartCoroutine(coroutinePack.ienumerator);
        }
    }
    public void StartIE(IEnumerator ienumerator)
    {
        if (ienumerator != null)
        {
            StopIE(ienumerator);
        }
        StartCoroutine(ienumerator);
    }
    /// <summary>
    /// 中断协程
    /// </summary>
    /// <param name="name">名称</param>
    public void StopIE(string name)
    {
        if (iEPool.ContainsKey(name))
        {
            IEnumerator ienumerator = iEPool[name];
            StopCoroutine(ienumerator);
            RemoveIE(name);
        }
    }
    /// <summary>
    /// 中断协程
    /// </summary>
    /// <param name="ienumerator">协程</param>
    public void StopIE(IEnumerator ienumerator)
    {
        if (ienumerator!=null)
        {
            StopCoroutine(ienumerator);
        }
    }
    private void RemoveIE(string name)
    {
        if (name != null && iEPool.ContainsKey(name))
        {
            iEPool.Remove(name);
            keys.Remove(name);
        }
    }

    /// <summary>
    /// 等待一段时间后执行
    /// </summary>
    /// <param name="name">名称 同名会覆盖上一个协程执行</param>
    /// <param name="time">时间</param>
    /// <param name="deltaTime">时间增量</param>
    /// <param name="endAction">计时结束执行action</param>
    public void WaitTimeToDo(string name,float time, DeltaTimeData deltaTime, Action endAction, int TimeScaleState = 0)
    {
        CoroutinePack coroutinePack = new CoroutinePack(name);
        IEnumerator ienumerator = WaitTimeDoing(coroutinePack, time, deltaTime, endAction, TimeScaleState);
        coroutinePack.ienumerator = ienumerator;
        StartIE(coroutinePack);
    }
    public void WaitTimeToDo(ref IEnumerator ienumerator, float time, DeltaTimeData deltaTime, Action endAction, int TimeScaleState = 0)
    {
        if (ienumerator != null)
        {
            StopIE(ienumerator);
        }
        ienumerator = WaitTimeDoing(null,time, deltaTime, endAction, TimeScaleState);
        StartIE(ienumerator);
    }
    private IEnumerator WaitTimeDoing(CoroutinePack coroutinePack, float time, DeltaTimeData deltaTime , Action endAction, int TimeScaleState = 0)
    {
        bool isBreak = false;
        while (time > 0)
        {
            try
            {
                time -= deltaTime != null ? deltaTime.Value : Time.deltaTime;
            }
            catch
            {
                isBreak = true;
                break;
            }
            switch (TimeScaleState)
            {
                case 0:
                    yield return null;
                    break;
                case 1:
                    yield return new WaitForEndOfFrame();
                    break;
                case 2:
                    yield return new WaitForFixedUpdate();
                    break;
            }
        }
        if (coroutinePack != null)
        {
            if (coroutinePack.name != null && coroutinePack.name != "")
                StopIE(coroutinePack.name);
            else
                noNameIEPoolPack.Remove(coroutinePack);
        }
        if (!isBreak)
        {
            endAction?.Invoke();
        }
        yield return null;
    }

    /// <summary>
    /// 持续一段时间内执行
    /// </summary>
    /// <param name="name">名称 同名会覆盖上一个协程执行</param>
    /// <param name="time">时间</param>
    /// <param name="deltaTime">时间增量</param>
    /// <param name="action">计时中执行action，参数为时间的倒计时</param>
    /// <param name="endAction">计时结束执行action</param>
    public void KeepTimeToDo(string name, float time, DeltaTimeData deltaTime, TimeAction action, Action endAction = null, int TimeScaleState = 0)
    {
        CoroutinePack coroutinePack = new CoroutinePack(name);
        IEnumerator ienumerator = KeepTimeDoing(coroutinePack, time, deltaTime, action, endAction, TimeScaleState);
        coroutinePack.ienumerator = ienumerator;
        StartIE(coroutinePack);
    }
    public void KeepTimeToDo(ref IEnumerator ienumerator, float time, DeltaTimeData deltaTime, TimeAction action, Action endAction = null, int TimeScaleState = 0)
    {
        if (ienumerator != null)
        {
            StopIE(ienumerator);
        }
        ienumerator = KeepTimeDoing(null,time, deltaTime, action, endAction, TimeScaleState);
        StartIE( ienumerator);
    }
    private IEnumerator KeepTimeDoing(CoroutinePack coroutinePack, float time, DeltaTimeData deltaTime, TimeAction action, Action endAction = null,int TimeScaleState=0)
    {
        bool isBreak = false;
        while (time > 0)
        {
            try
            {
                isBreak = !(action?.Invoke(time)).Value;
                if (isBreak)
                {
                    break;
                }
                time -= deltaTime != null ? deltaTime.Value : Time.deltaTime;
            }
            catch
            {
                isBreak = true;
                break;
            }
            switch (TimeScaleState)
            {
                case 0:
                    yield return null;
                    break;
                case 1:
                    yield return new WaitForEndOfFrame();
                    break;
                case 2:
                    yield return new WaitForFixedUpdate();
                    break;
            }

        }
        if (coroutinePack != null)
        {
            if (coroutinePack.name != null && coroutinePack.name != "")
                StopIE(coroutinePack.name);
            else
                noNameIEPoolPack.Remove(coroutinePack);
        }
        if (!isBreak)
        {
            endAction?.Invoke();
        }
        yield return null;
    }

    /// <summary>
    /// 按条件持续执行
    /// </summary>
    /// <param name="name">名称 同名会覆盖上一个协程执行</param>
    /// <param name="action">返回值为true持续执行action 返回值为false则打断执行</param>
    public void KeepTrueToDo(string name, SwitchAction action, int TimeScaleState = 0)
    {
        CoroutinePack coroutinePack = new CoroutinePack(name);
        IEnumerator ienumerator = KeepTrueDoing(coroutinePack, action, null, TimeScaleState);
        coroutinePack.ienumerator = ienumerator;
        StartIE(coroutinePack);

    }
    public void KeepTrueToDo(ref IEnumerator ienumerator, SwitchAction action, int TimeScaleState = 0)
    {
        if (ienumerator != null)
        {
            StopIE(ienumerator);
        }
        ienumerator = KeepTrueDoing(null, action, null, TimeScaleState);
        StartIE(name, ienumerator);
    }
    private IEnumerator KeepTrueDoing(CoroutinePack coroutinePack, SwitchAction action, Action endAction = null, int TimeScaleState = 0)
    {
        bool isBreak = false;
        while (!isBreak && (action?.Invoke()).Value)
        {
            try
            {
            }
            catch
            {
                isBreak = true;
                break;
            }
            switch (TimeScaleState)
            {
                case 0:
                    yield return null;
                    break;
                case 1:
                    yield return new WaitForEndOfFrame();
                    break;
                case 2:
                    yield return new WaitForFixedUpdate();
                    break;
            }
        }
        if (coroutinePack != null)
        {
            if (coroutinePack.name != null && coroutinePack.name != "")
                StopIE(coroutinePack.name);
            else
                noNameIEPoolPack.Remove(coroutinePack);
        }
        if (!isBreak)
        {
            endAction?.Invoke();
        }
        yield return null;
    }

    public void Clear()
    {
        for (int i = keys.Count - 1; i >= 0; i--)
        {
            StopIE(iEPool[keys[i]]);
        }
        StopAllCoroutines();
    }
    protected override void OnDestroy()
    {
        StopAllCoroutines();
        base.OnDestroy();
    }
    public class CoroutinePack
    {
        public string name;
        public IEnumerator ienumerator;
        public CoroutinePack(string name)
        {
            this.name = name;
        }
        public CoroutinePack(IEnumerator enumerator)
        {
            this.ienumerator = enumerator;
        }
    }
}
