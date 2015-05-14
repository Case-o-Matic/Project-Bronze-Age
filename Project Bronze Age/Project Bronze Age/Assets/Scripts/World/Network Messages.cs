using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;
using NetSerializer;
using System;

// PROTOCOL:
// horizontal/verticalMove = 2 means no value
// other (s)byte = 0 means no value
// int = 0 means no value
// Vector3 = default(Vector3) means no value
// Reference types = null means no value

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct ClientRequest // Packet size in bytes: 38
{
    public sbyte horizontalMove, verticalMove;
    public int invokeAbilityId,
        pickupItemToInvId,
        removeItemFromInvId, acceptQuestId, interactWithActorId;

    public int invokeAbilityTargetActorId;
    public Vector3 invokeAbilityTargetPos;

    // TODO: Change primary weapon, and find more requests

    public ClientRequest(sbyte hormove = 2, sbyte vermove = 2, int invokeabilityid = 0, int pickupitemtoinvid = 0, int removeitemfrominvid = 0, int acceptquestid = 0, int invokeabilitytargetactorid = 0, Vector3 invokeabilitytargetpos = default(Vector3), int interactwithactorid= 0)
    {
        horizontalMove = hormove;
        verticalMove = vermove;
        invokeAbilityId = invokeabilityid;
        pickupItemToInvId = pickupitemtoinvid;
        removeItemFromInvId = removeitemfrominvid;
        acceptQuestId = acceptquestid;
        interactWithActorId = interactwithactorid;

        invokeAbilityTargetActorId = invokeabilitytargetactorid;
        invokeAbilityTargetPos = invokeabilitytargetpos;
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct ServerEvent // Packet size in bytes: 40
{
    public int invokeAbilityId,
        pickupItemToInvId,
        removeItemFromInvId,

        interactWithActorId,

        acceptQuestId, finishQuestId,
        
        receiveDamageAmount, receiveDamageDdActorId;
    public byte receiveDamageType;

    public int invokeAbilityTargetActorId;
    public Vector3 invokeAbilityTargetPos;

    public ServerEvent(int invokeabilityid = 0, int pickupitemtoinvid = 0, int removeitemfrominvid = 0, int interactwithactorid = 0, int acceptquestid = 0, int finishquestid = 0, int receivedamageamount = 0, int receivedamageddactorid = 0, byte receivedamagetype = 0, int invokeabilitytargetactorid = 0, Vector3 invokeabilitytargetpos = default(Vector3))
    {
        invokeAbilityId = invokeabilityid;
        pickupItemToInvId = pickupitemtoinvid;
        removeItemFromInvId = removeitemfrominvid;
        interactWithActorId = interactwithactorid;
        acceptQuestId = acceptquestid;
        finishQuestId = finishquestid;
        receiveDamageAmount = receivedamageamount;
        receiveDamageDdActorId = receivedamageddactorid;
        receiveDamageType = receivedamagetype;

        invokeAbilityTargetActorId = invokeabilitytargetactorid;
        invokeAbilityTargetPos = invokeabilitytargetpos;
    }
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct ServerState // Packet size in bytes: 44
{
    public Vector3 position, rotation;
    //public byte isDead; // 0 = false, 1 = true, 2 = no value
    public int currentGold, currentLevel, currentXp;
    public float[] attributes;

    public ServerState(Vector3 pos = default(Vector3), Vector3 rot = default(Vector3), byte isdead = 2, int currentgold = 0, int currentlevel = 0, int currentxp = 0, float[] attributes = null)
    {
        position = pos;
        rotation = rot;
        currentGold = currentgold;
        currentLevel = currentlevel;
        currentXp = currentxp;
        this.attributes = attributes;
    }
}

// TODO: For NetSerializer all message structs need Serializable-attributes
public static class MessageConverter
{
    private static Serializer serializer = new Serializer(new Type[3] { typeof(ClientRequest), typeof(ServerEvent), typeof(ServerState) });

    public static byte[] Serialize<T>(T obj)
    {
        using (var mStream = new MemoryStream())
        {
            serializer.Serialize(mStream, obj);
            return mStream.ToArray();
        }
    }
    public static T Deserialize<T>(byte[] bytes)
    {
        using (var mStream = new MemoryStream(bytes))
        {
            return (T)serializer.Deserialize(mStream);
        }
    }
}