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
        // Find the quest by the ID and finish it
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

    protected override void OnApplyServerEvent(ServerEvent ev)
    {
        if (ev.acceptQuestId != 0)
        {
            AcceptQuest(ev.acceptQuestId);
        }
        if (ev.finishQuestId != 0)
        {
            FinishQuest(ev.finishQuestId);
        }

        base.OnApplyServerEvent(ev);
    }
    protected override void OnApplyServerState(ServerState state, float timestamp)
    {
        base.OnApplyServerState(state, timestamp);
    }
    protected override void OnReceiveClientRequest(ClientRequest rq)
    {
        if (rq.horizontalMove != Actor.networkMessageSByteNoValue)
            horizontalMove = rq.horizontalMove;
        if (rq.verticalMove != Actor.networkMessageSByteNoValue)
            verticalMove = rq.verticalMove;
        if(rq.acceptQuestId != 0)
        {
            // Check quest and if available/acceptable accept it and send a ServerEvent back
        }

        base.OnReceiveClientRequest(rq);
    }

    // If this is only used once for the movement, use the method body code and remove this method
    // A nested call stack is lowering performance, especially in the often called Update method
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
                // Update the next client request to finish quest

                // Send command for rewards?
                FinishQuest(currentQuests[i].globalId);
            }
        }
    }
}