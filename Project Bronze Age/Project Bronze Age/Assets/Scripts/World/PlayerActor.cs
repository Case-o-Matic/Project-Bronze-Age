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

    // Network
    protected ClientRequest nextClientRequest;
    private float clientMessageSendRateTime;

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
        // If client and owner of actor
        nextClientRequest.horizontalMove = (sbyte)Input.GetAxisRaw("Horizontal");
        nextClientRequest.verticalMove = (sbyte)Input.GetAxisRaw("Vertical");

        // If client and owner of actor
        clientMessageSendRateTime += Time.deltaTime;
        if (clientMessageSendRateTime >= networkClientSendRateInterval)
        {
            clientMessageSendRateTime = 0;
            SendClientRequest();
        }

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
    protected override void OnApplyServerState(ServerState state)
    {
        base.OnApplyServerState(state);
    }

    protected override void OnDeath()
    {
        // Is really nothing happening if a player dies?
        // If owner of actor: Should the death screen, respawn info and other things get initiated here?
        base.OnDeath();
    }

    // If this is only used once for the movement, use the method body code and remove this method
    // A nested call stack is lowering performance, especially in the often called Update method
    private Vector3 PerformMoveByAxes()
    {
        Vector3 newDir = new Vector3(horizontalMove, 0, verticalMove);
        newDir *= totalMovementspeed / 2;

        return newDir;
    }

    private void SendClientRequest()
    {
        // TODO: Check if client and if this actor is the local player and then send nextClientRequest
    }
}