using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable()]
public abstract class Buff : ScriptableObject, IGlobalID
{
    public string buffName;
    public string description;
    public BuffType type;
    public float liveTime, effectInvokationIntervall;
    public bool hasLivetime, isRemovable, isDebuff, hasInvokationIntervall;
    public float currentLiveTime, currentEffectInvokationIntervall;
    public List<Effect> effects;

    private int _networkId;

    public int globalId
    {
        get { return _networkId; }
    }

    public void Update()
    {
        if (hasLivetime && currentLiveTime > 0)
        {
            currentLiveTime -= Time.deltaTime;
            if (currentLiveTime <= 0)
            {
                currentLiveTime = 0;
            }
        }
    }

    public virtual void OnApply(LiveActor actor)
    {

    }
    public virtual void OnUnapply()
    {

    }
}

[Serializable]
public enum BuffType
{
    Heal,
    Damage,
    DoT,
    Poison,
    Movement,
    Stun
}