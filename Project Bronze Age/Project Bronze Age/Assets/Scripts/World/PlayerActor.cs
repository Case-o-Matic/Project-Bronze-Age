using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerActor : LiveActor
{
    public int maxAcceptableQuests = 20;

    public List<Quest> currentQuests;
    public sbyte horizontalMove, verticalMove;

    public void AcceptQuest(int id) // TODO: Use a real ID-system for quests
    {
        // Add the clone of the accepted quest to the currentQuests list
    }
    public void FinishQuest(int id)
    {

    }

    protected override void Update()
    {
        // If client
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S))
            SendClientRequest(new ClientRequest(hormove: (sbyte)Input.GetAxisRaw("Horizontal"), vermove: (sbyte)Input.GetAxisRaw("Vertical")));
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.S))
            SendClientRequest(new ClientRequest(hormove: (sbyte)Input.GetAxisRaw("Horizontal"), vermove: (sbyte)Input.GetAxisRaw("Vertical")));

        base.Update();
    }

    protected override void OnApplyServerCommand(ServerCommand cmd, float timestamp)
    {
        if(cmd.acceptQuestId != 0)
        {
            AcceptQuest(cmd.acceptQuestId);
        }
        if(cmd.finishQuestId != 0)
        {
            FinishQuest(cmd.finishQuestId);
        }

        base.OnApplyServerCommand(cmd, timestamp);
    }
    protected override void OnReceiveClientRequest(ClientRequest rq)
    {
        if (rq.horizontalMove != Actor.networkMessageSByteNoValue)
            horizontalMove = rq.horizontalMove;
        if (rq.verticalMove != Actor.networkMessageSByteNoValue)
            verticalMove = rq.verticalMove;
        if(rq.acceptQuestId != 0)
        {
            Quest quest = currentQuests[0]; // Find the quest
            if (currentQuests.Count + 1 < maxAcceptableQuests && !currentQuests.Contains(quest))
                SendServerCommand(new ServerCommand(acceptquestid: rq.acceptQuestId));
        }

        base.OnReceiveClientRequest(rq);
    }

    private Vector3 PerformMoveByAxes()
    {
        Vector3 newDir = new Vector3(horizontalMove, 0, verticalMove);
        newDir *= totalMovementspeed / 2;

        return newDir;
    }

    private void UpdateQuests()
    {
        for (int i = 0; i < currentQuests.Count; i++)
        {
            if (currentQuests[i].CheckQuestCompleted(this))
            {
                SendServerCommand(new ServerCommand(finishquestid: currentQuests[i].networkId)); // Is this ID right?

                // Send command for rewards?
                FinishQuest(currentQuests[i].networkId);
            }
        }
    }
}