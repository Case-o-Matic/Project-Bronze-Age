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
        navMeshAgent.speed = totalMovementspeed;
        DoAIControl();

        base.Update();
    }

    private void DoAIControl()
    {
        // Check if server then calculate
    }

    private void GotoPosition(Vector3 position)
    {
        navMeshAgent.SetDestination(position);
    }
}