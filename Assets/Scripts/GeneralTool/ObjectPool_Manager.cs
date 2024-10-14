using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool_Manager : Singleton_Base<ObjectPool_Manager>
{
    public override bool isDontDestroy => true;
    public Dictionary<string, List<GameObject>> activeObjectPool = new Dictionary<string, List<GameObject>>();
    public Dictionary<string, Queue<GameObject>> unactiveObjectPool=new Dictionary<string, Queue<GameObject>>();

    public System.Action selfDestroyAction;
    public void Reset()
    {
        selfDestroyAction?.Invoke();
        selfDestroyAction = null;
        activeObjectPool = new Dictionary<string, List<GameObject>>();
        unactiveObjectPool = new Dictionary<string, Queue<GameObject>>();
    }
    public List<GameObject> CreateAciveGroup(string name)
    {
        if (!activeObjectPool.ContainsKey(name))
        {
            activeObjectPool.Add(name, new List<GameObject>());
        }
        return activeObjectPool[name];
    }

    public GameObject Create(string name, GameObject prefab = null, Transform parent = null)
    {
        GameObject target = null;
        if (!unactiveObjectPool.ContainsKey(name))
        {
            unactiveObjectPool.Add(name, new Queue<GameObject>());
        }
        if (unactiveObjectPool[name].Count > 0)
        {
            target = unactiveObjectPool[name].Dequeue();
        }
        if (target == null)
        {
            if (prefab == null)
            {
                target = new GameObject();
            }
            else
            {
                target = Instantiate(prefab);
            }
            selfDestroyAction += () => {
                if (target != null)
                {
                    Destroy(target);
                }
            };
        }
        target.transform.parent = parent;
        target.gameObject.SetActive(true);
        CreateAciveGroup(name).Add(target);
        return target;
    }
    public T Create<T>(string name, T prefab = null, Transform parent = null) where T : Component
    {
        T target = null;
        if (!unactiveObjectPool.ContainsKey(name))
        {
            unactiveObjectPool.Add(name, new Queue<GameObject>());
        }
        if (unactiveObjectPool[name].Count > 0)
        {
            target = unactiveObjectPool[name].Dequeue().GetComponent<T>();
        }
        if (target == null)
        {
            target = Instantiate(prefab,parent);
            selfDestroyAction += () =>
            {
                if (target != null)
                {
                    Destroy(target);
                }
            };
        }
        else
        {
            target.transform.SetParent(parent);
        }
        target.gameObject.SetActive(true);
        CreateAciveGroup(name).Add(target.gameObject);
        return target;
    }
    public void Hide(string name, GameObject target, float delay = 0)
    {
        System.Action hideEvent = () =>
        {
            List<GameObject> gameObjects = CreateAciveGroup(name);
            if (gameObjects.Contains(target))
            {
                gameObjects.Remove(target);
            }
            if (!unactiveObjectPool.ContainsKey(name))
            {
                unactiveObjectPool.Add(name, new Queue<GameObject>());
            }
            if (!unactiveObjectPool[name].Contains(target))
            {
                unactiveObjectPool[name].Enqueue(target);
            }
            target.gameObject.SetActive(false);
        };
        if (delay > 0)
        {
            IEPool_Manager.instance?.WaitTimeToDo("", delay, null, () => {
                hideEvent();
            });
            return;
        }
        hideEvent();
    }
    public void HideGroup(string name)
    {
        List<GameObject> gameObjects = CreateAciveGroup(name);
        for(int i= gameObjects.Count - 1; i >= 0; i--)
        {
            GameObject target = gameObjects[i];
            gameObjects.RemoveAt(i);
            if (!unactiveObjectPool.ContainsKey(name))
            {
                unactiveObjectPool.Add(name, new Queue<GameObject>());
            }
            unactiveObjectPool[name].Enqueue(target);
            target.gameObject.SetActive(false);
        }
    }
}
