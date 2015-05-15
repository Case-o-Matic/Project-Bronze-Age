using System;
using System.Collections.Generic;
using UnityEngine;

public class NpcActor : LiveActor
{
    // Is this range good?
    public const float xpRewardOnDeathRange = 10;

    public string dialogText;
    public List<Quest> availableQuests;
    public int xpRewardOnDeath;

    public NavMeshAgent navMeshAgent;

    protected override void Update()
    {
        navMeshAgent.speed = totalMovementspeed;
        DoAIControl();

        base.Update();
    }

    public override void Stun(bool value)
    {
        if (value)
            navMeshAgent.Stop();
        else
            navMeshAgent.Resume();
        base.Stun(value);
    }

    protected override void OnDeath()
    {
        // Total XP-reward = Base XP-reward + (Base XP-reward * (Current level / 100))
        // Every live actor in the XP-reward range gets: Total XP-reward / Amount of live actors in XP-reward range
        int totalXpRewardOnDeath = xpRewardOnDeath + (xpRewardOnDeath * Mathf.RoundToInt(((float)level.currentLevel / 100f)));

        var colliders = Physics.OverlapSphere(transform.position, xpRewardOnDeathRange);
        var liveActors = new List<LiveActor>();
        foreach (var collider in colliders)
        {
            if(collider.CompareTag("Actor"))
            {
                var liveActor = collider.GetComponent<LiveActor>();
                if (liveActor != null)
                    liveActors.Add(liveActor);
            }
        }

        foreach (var liveActor in liveActors)
        {
            liveActor.AddXp(Mathf.RoundToInt((float)totalXpRewardOnDeath / (float)liveActors.Count));
        }
        base.OnDeath();
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