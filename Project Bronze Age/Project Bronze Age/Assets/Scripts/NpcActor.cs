using System;
using UnityEngine;

[Obsolete()]
public class NpcActor : MovableActor
{
    public NavMeshAgent navMeshAgent;

    protected override void Update()
    {
        DoAIControl();
        base.Update();
    }

    public void DoAIControl()
    {

    }
}