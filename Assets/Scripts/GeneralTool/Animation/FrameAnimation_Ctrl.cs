using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FrameAnimation_Ctrl : Animation_Base
{
    public int findComponentBits = 0;
    public Image _image;
    public Image image
    {
        get
        {
            if ( (findComponentBits >> 0 & 1) == 0 && _image == null)
            {
                findComponentBits |= 1 << 0;
                _image = this.transform.GetComponent<Image>();
            }
            return _image;
        }
    }
    public SpriteRenderer _spriteRenderer;
    public SpriteRenderer spriteRenderer
    {
        get
        {
            if ( (findComponentBits >> 1 & 1) == 0 && _spriteRenderer == null)
            {
                findComponentBits |= 1 << 1;
                _spriteRenderer = this.transform.GetComponent<SpriteRenderer>();
            }
            return _spriteRenderer;
        }
    }

    public List<Sprite> sprites;
    [SerializeField]
    private FrameAnimationData_Scriptable frameAnimationData;
    [SerializeField]
    private ObjectGroup<FrameAnimationData_Scriptable> animation_Group;
    public bool isPlaying;
    public float speed = 1;
    public float time;
    public bool isUnsacleTime;
    private int frameIndex = -1;

    public float _intervalTime=0.01f;
    public float intervalTime
    {
        get
        {
            if (frameAnimationData != null)
            {
                return frameAnimationData.intervalTime;
            }
            return _intervalTime;
        }
    }
    public bool _isLoop;
    public bool isLoop
    {
        get
        {
            if (frameAnimationData != null)
            {
                return frameAnimationData.isLoop;
            }
            return _isLoop;
        }
    }
    public Sprite this[int index]
    {
        get
        {
            if (frameAnimationData != null)
            {
                return frameAnimationData[index];
            }
            return sprites[index];
        }
    }
    public int Count
    {
        get
        {
            if (frameAnimationData != null)
            {
                return frameAnimationData.count;
            }
            return sprites.Count;
        }
    }
    public float _length;
    public float length
    {
        get
        {
            if (frameAnimationData != null)
            {
                return frameAnimationData.length;
            }
            return _length;
        }
    }
    public string currentAnimationName
    {
        get
        {
            return frameAnimationData != null ? frameAnimationData.name : null;
        }
    }

    public List<FrameActionData> frameActions;
    public Dictionary<int, int> frameAction_Dictionary;
    public Dictionary<string, int> frameActionNameIndex_Dictionary;

    public List<Animation_Base> linkAnimation;
    private void Start()
    {
        frameAction_Dictionary = new Dictionary<int, int>();
        frameActionNameIndex_Dictionary = new Dictionary<string, int>();
        for (int i = 0; i < frameActions.Count; i++)
        {
            frameAction_Dictionary.Add(frameActions[i].frameIndex, i);
            frameActionNameIndex_Dictionary.Add(frameActions[i].frameName, i);
        }

        if (Count > 0&&intervalTime!=0)
        {
            _length = intervalTime * Count;
        }
    }
    private void Update()
    {
        if (isPlaying)
        {
            if (frameAnimationData != null)
            {
                time += (isUnsacleTime ? Time.unscaledDeltaTime : Time.deltaTime) * speed * frameAnimationData.speed;
            }
            else
            {
                time += (isUnsacleTime ? Time.unscaledDeltaTime : Time.deltaTime) * speed;
            }
            Run(time);
            if (linkAnimation.Count > 0)
            {
                foreach(Animation_Base animation_Base in linkAnimation)
                {
                    animation_Base.Run(time);
                }
            }
        }
    }

    public override void Run(float time)
    {
        int currentFrameIndex = (int)(time / intervalTime) % Count;
        if (!isLoop)
        {
            if (time >= length)
            {
                isPlaying = false;
                return;
            }
        }
        if (frameIndex != currentFrameIndex)
        {
            frameIndex = currentFrameIndex;
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = this[currentFrameIndex];
            }
            if (image != null)
            {
                image.sprite = this[currentFrameIndex];
            }
            if (frameAction_Dictionary != null && frameAction_Dictionary.ContainsKey(currentFrameIndex))
            {
                frameActions[frameAction_Dictionary[currentFrameIndex]].action.Invoke();
            }
        }
    }
    public void Play(FrameAnimationData_Scriptable frameAnimationData, bool isReal = false)
    {
        if (frameAnimationData == null || currentAnimationName == frameAnimationData.name && !isReal)
        {
            return;
        }
        this.frameAnimationData = frameAnimationData;
        time = 0;
        isPlaying = true;
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = frameAnimationData[0];
        }
        if (image != null)
        {
            image.sprite = frameAnimationData[0];
        }
        if (frameAction_Dictionary != null && frameAction_Dictionary.ContainsKey(0))
        {
            frameActions[frameAction_Dictionary[0]].action.Invoke();
        }
    }
    public void Play(string name, bool isReal = false)
    {
        Play(animation_Group[name],isReal);
    }
    public void Play()
    {
        time = 0;
        isPlaying = true;
        Run(time);
    }
    public void Stop()
    {
        isPlaying = false;
    }
    public void SetFrameAction(string name, System.Action action)
    {
        if (frameActionNameIndex_Dictionary.ContainsKey(name))
        {
            UnityEngine.Events.UnityEvent unityEvent = new UnityEngine.Events.UnityEvent();
            unityEvent.AddListener(() => { action?.Invoke(); });
            frameActions[frameActionNameIndex_Dictionary[name]].action = unityEvent;
        }
        else
        {
            Debug.Log("[" + this.name + "_" + this.GetInstanceID() + "]找不到当前事件:"+ name);
        }
    }
    [System.Serializable]
    public class FrameActionData
    {
        public int frameIndex;
        public string frameName;
        public UnityEngine.Events.UnityEvent action;
    }
}
