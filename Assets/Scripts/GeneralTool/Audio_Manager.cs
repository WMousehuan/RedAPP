using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manager : Singleton_Base<Audio_Manager>
{
    public static List<AudioSource> audioSourcePool=new List<AudioSource>();
    public static HashSet<string> queueList=new HashSet<string>();

    public override bool isDontDestroy => true;

    public static AudioSource Play(AudioPack audioPack,Vector3 pos=default, Transform target=null, float intervalTime= 0.1f)
    {
        if (audioPack.audioClip != null)
        {
            return Play(audioPack.audioClip, audioPack.volume, audioPack.pitch, pos, target, intervalTime);
        }
        return null;
    }
    public static AudioSource Play(AudioClip audioClip,float volume,float pitch, Vector3 pos=default,Transform target=null, float intervalTime=0.1f)
    {
        if (!audioClip)
        {
            Debug.Log("ÒôÆµÎªNull");
            return null;
        }
        if (!queueList.Add(audioClip.name))
        {
            return null;
        }
      
        AudioSource audioSource = null;
        if (audioSourcePool.Count > 0)
        {
            audioSource = audioSourcePool[0];
            audioSourcePool.RemoveAt(0);
        }
        else
        {
            audioSource = new GameObject().AddComponent<AudioSource>();
            audioSource.gameObject.transform.SetParent(instance.transform);
        }
        if (audioSource == null)
        {
            return null;
        }
        audioSource.transform.position = pos;
        audioSource.name = "AudioSource_" + audioClip.name;
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.Play();
        IEPool_Manager.instance?.KeepTrueToDo("", () => {
            if (audioSource.isPlaying)
            {
                if (target != null)
                {
                    audioSource.volume = Mathf.Lerp(volume, 0, ((target.transform.position - audioSource.transform.position).magnitude - 2) / 4f);
                }
                return true;
            }
            audioSourcePool.Add(audioSource);
            audioSource.name = "AudioSource_None";
            return false;
        });
        if (intervalTime > 0)
        {
            queueList.Add(audioClip.name);
            IEPool_Manager.instance?.WaitTimeToDo("", intervalTime, null, () =>
            {
                queueList.Remove(audioClip.name);
            });
        }
        return audioSource;
    }
}
[System.Serializable]
public class AudioGroup<T> where T : AudioPack
{
    [SerializeField]
    protected List<T> audioPacks = new List<T>();
    protected Dictionary<string, int> audioIndex_Dictionary;
    public int Count
    {
        get
        {
            return audioPacks.Count;
        }
    }
    public AudioPack this[string name]
    {
        get
        {
            if (audioIndex_Dictionary == null)
            {
                audioIndex_Dictionary = new Dictionary<string, int>();
                for (int i = 0; i < audioPacks.Count; i++)
                {
                    if (!audioIndex_Dictionary.ContainsKey(audioPacks[i].name))
                    {
                        audioIndex_Dictionary.Add(audioPacks[i].name, i);
                    }
                }
            }
            if (audioIndex_Dictionary.ContainsKey(name))
            {
                return audioPacks[audioIndex_Dictionary[name]];
            }
            return default;
        }
    }
    public AudioPack this[int index]
    {
        get
        {
            return audioPacks[index];
        }
    }
    public bool Contains(string name)
    {
        if (audioIndex_Dictionary == null)
        {
            audioIndex_Dictionary = new Dictionary<string, int>();
            for (int i = 0; i < audioPacks.Count; i++)
            {
                if (!audioIndex_Dictionary.ContainsKey(audioPacks[i].name))
                {
                    audioIndex_Dictionary.Add(audioPacks[i].name, i);
                }
            }
        }
        if (audioIndex_Dictionary != null)
        {
            return audioIndex_Dictionary.ContainsKey(name);
        }
        return false;
    }
}
[System.Serializable]
public class AudioPack
{
    public string name;
    public AudioClip audioClip;
    public float volume = 1;
    public float pitch = 1;
}

