using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy_Tool : MonoBehaviour
{
    public static Dictionary<string, GameObject> aliveGameObject=new Dictionary<string, GameObject>();
    void Awake()
    {
        if (aliveGameObject.ContainsKey(this.gameObject.name))
        {
            Destroy(this.gameObject);
        }
        else
        {
            aliveGameObject.Add(this.gameObject.name, this.gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }
}
