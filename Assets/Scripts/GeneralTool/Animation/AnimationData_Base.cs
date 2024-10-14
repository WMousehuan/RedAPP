using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationData_Base : ScriptableObject
{
    public float speed=1;
    public abstract float length { get; }
}
