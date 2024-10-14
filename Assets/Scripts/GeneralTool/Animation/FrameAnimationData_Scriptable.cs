using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AnimationData", menuName = "ScriptableObject/AnimationData", order = 0)]
public class FrameAnimationData_Scriptable : AnimationData_Base
{
    public float intervalTime = 0.02f;
    public bool isLoop;
    public List<Sprite> frames;
    public int count => frames.Count;
    public override float length
    {
        get
        {
            return count * intervalTime;
        }
    }
    public Sprite this[int index]
    {
        get
        {
            return frames[index];
        }
    }
    
}
