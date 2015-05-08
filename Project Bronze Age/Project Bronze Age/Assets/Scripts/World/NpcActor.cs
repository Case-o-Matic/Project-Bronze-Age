using System;
using System.Collections.Generic;
using UnityEngine;

public class NpcActor : LiveActor
{
    public string dialogText;
    public List<Quest> availableQuests;

    public NavMeshAgent navMeshAgent;

    protected override void Update()
    {
        DoAIControl();
        base.Update();
    }

    public void GotoPosition(Vector3 position)
    {
        navMeshAgent.SetDestination(position);
    }

    private void DoAIControl()
    {

    }
}