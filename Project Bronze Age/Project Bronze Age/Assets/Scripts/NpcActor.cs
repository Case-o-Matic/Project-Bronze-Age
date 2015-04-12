using System;
using System.Collections.Generic;
using UnityEngine;

public class NpcActor : LiveActor
{
    public NavMeshAgent navMeshAgent;
    public List<string> startBuffs, startAbilities, startItems;

    protected override void Update()
    {
        DoAIControl();
        base.Update();
    }



    public void GotoPosition(Vector3 position)
    {
        navMeshAgent.SetDestination(position);
    }

    private void Initialize()
    {
        ResourceSystem.Instance.ApplyResourceData(this);

        foreach (var startBuff in startBuffs)
        {
            ApplyBuff(startBuff);
        }
        foreach (var startAbility in startAbilities)
        {
            AddAbility(startAbility);
        }
        foreach (var startItem in startItems)
        {
            inventory.AddItem(startItem);
        }
    }

    private void DoAIControl()
    {

    }
}