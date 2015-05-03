using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class Effect
{
    public EffectPosition position;
    public GameObject gameObject;

    public GameObject instantiatedEffectObject1, instantiatedEffectObject2;
}

[Serializable]
public enum EffectPosition
{
    Head,
    Center,
    Hands,
    Foots,
}