using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable()]
public class Buff : ScriptableObject
{
    public string buffName;
    public string buffDescription;
    public float liveTime;
    public bool hasLivetime, isRemovable;
    public float currentLiveTime;

    public List<Effect> effects;
}