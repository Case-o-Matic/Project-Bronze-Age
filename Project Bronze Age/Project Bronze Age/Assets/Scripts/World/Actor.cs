using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public abstract class Actor : MonoBehaviour, INetworkID
{
    public const sbyte networkMessageSByteNoValue = 2;
    public const int interpolationTimeDelta = 100, interpolationBufferLength = 20;
    public const float interpolationBacktime = 0.1f;
    
    //[SerializeField]
    private int _networkId;
    public string actorName;

    // Networking
    private ClientRequest lastRequest;
    private ServerCommand lastCommand;
    private InterpolationState[] interpolationStateBuffer;
    private int timestampCount;

    public int networkId
    {
        get { return _networkId; }
    }

    protected virtual void Awake()
    {
        _networkId = GetHashCode();
        interpolationStateBuffer = new InterpolationState[interpolationBufferLength];
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        PerformInterpolation(1 /* Find client timestamp */);
    }

    protected virtual void FixedUpdate()
    {
    }

    protected virtual void OnDestroy()
    {
    }

    protected virtual void OnAnimatorMove()
    {
    }

    [Obsolete]
    protected virtual void OnSerializeServerStream(BitStream stream)
    {
    }
    [Obsolete]
    protected virtual void OnSerializeClientStream(BitStream stream)
    {
    }

    // Client
    protected void SendClientRequest(ClientRequest rq)
    {
        // TODO: Send the client request
    }
    // Client
    protected virtual void OnApplyServerCommand(ServerCommand cmd, float timestamp)
    {
        if (cmd.position != Vector3.zero) // if the pos. isnt zero the rot. isnt too
        {
            for (int i = interpolationStateBuffer.Length - 1; i >= 1; i--)
            {
                interpolationStateBuffer[i] = interpolationStateBuffer[i - 1];
            }

            InterpolationState newState = new InterpolationState(cmd.position, cmd.rotation, timestamp);
            interpolationStateBuffer[0] = newState;

            timestampCount = Mathf.Min(timestampCount + 1, interpolationStateBuffer.Length);
            //for (int i = 0; i < timestampCount - 1; i++)
            //{
            //    if (interpolationStateBuffer[i].serverTimestamp < interpolationStateBuffer[i + 1].serverTimestamp)
            //        Debug.Log("Inconsistent state found, reshuffling...");
            //}
        }
    }
    // Server
    protected virtual void SendServerCommand(ServerCommand cmd)
    {
        cmd.position = transform.position;
        cmd.rotation = transform.rotation.eulerAngles;

        // TODO: Send the server command
    }
    // Server
    protected virtual void OnReceiveClientRequest(ClientRequest rq)
    {

    }

    private void OnSerializeBitStream(BitStream stream, NetworkMessageInfo info)
    {
    }

    private void PerformInterpolation(float clienttimestamp)
    {
        float interpolationTime = clienttimestamp - interpolationBacktime;
        if(interpolationStateBuffer[0].serverTimestamp > interpolationTime)
        {
            for (int i = 0; i < timestampCount; i++)
            {
                if(interpolationStateBuffer[i].serverTimestamp <= interpolationTime || i == timestampCount)
                {
                    var newerState = interpolationStateBuffer[Math.Min(i - 1, 0)];
                    var bestState = interpolationStateBuffer[i];

                    float length = newerState.serverTimestamp - bestState.serverTimestamp;
                    float time = 0.0f;

                    if (length > 0.0001f)
                        time = (float)((interpolationTime - bestState.serverTimestamp) / length);

                    transform.position = Vector3.Lerp(bestState.position, newerState.position, time);
                    transform.rotation = Quaternion.Lerp(Quaternion.Euler(bestState.rotation), Quaternion.Euler(newerState.rotation), time);
                    return;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    protected struct ClientRequest // Packet size in bytes: 34 (without sequential packaging)
    {
        // (s)byte = 2 means no value (networkMessageSByteNoValue)
        // int = 0 means no value
        // Vector3 = Vector3.zero means no value

        public sbyte horizontalMove, verticalMove;
        public int invokeAbilityId,
            pickupItemToInvId,
            removeItemFromInvId, acceptQuestId;

        public int invokeAbilityTargetActorId;
        public Vector3 invokeAbilityTargetPos;

        // TODO: Change primary weapon, and find more requests

        public ClientRequest(sbyte hormove = networkMessageSByteNoValue, sbyte vermove = networkMessageSByteNoValue, int invokeabilityid = 0, int pickupitemtoinvid = 0, int removeitemfrominvid = 0, int acceptquestid = 0, int invokeabilitytargetactorid = 0, Vector3 invokeabilitytargetpos = default(Vector3))
        {
            horizontalMove = hormove;
            verticalMove = vermove;
            invokeAbilityId = invokeabilityid;
            pickupItemToInvId = pickupitemtoinvid;
            removeItemFromInvId = removeitemfrominvid;
            acceptQuestId = acceptquestid;

            invokeAbilityTargetActorId = invokeabilitytargetactorid;
            invokeAbilityTargetPos = invokeabilitytargetpos;
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    protected struct ServerCommand // Packet size in bytes: 60 (without sequential packaging)
    {
        // (s)byte = 2 means no value
        // int = 0 means no value
        // Vector3 = default(Vector3) means no value

        public Vector3 position, rotation;
        public int invokeAbilityId,
            pickupItemToInvId,
            removeItemFromInvId,
            interactWithActorId, acceptQuestId, finishQuestId, currentGold, currentLevel, currentXp;

        public int invokeAbilityTargetActorId;
        public Vector3 invokeAbilityTargetPos;

        // TODO: Change primary weapon, and find more commands

        public ServerCommand(Vector3 pos = default(Vector3), Vector3 rot = default(Vector3), int invokeabilityid = 0, int pickupitemtoinvid = 0, int removeitemfrominv = 0, int interactwithactorid = 0, int acceptquestid = 0, int finishquestid = 0, int invokeabilitytargetactorid = 0, Vector3 invokeavilitytargetpos = default(Vector3), int currentgold = 0, int currentlevel = 0, int currentxp = 0)
        {
            position = pos;
            rotation = rot;
            invokeAbilityId = invokeabilityid;
            pickupItemToInvId = pickupitemtoinvid;
            removeItemFromInvId = removeitemfrominv;
            interactWithActorId = interactwithactorid;
            acceptQuestId = acceptquestid;
            finishQuestId = finishquestid;
            invokeAbilityTargetActorId = invokeabilitytargetactorid;
            invokeAbilityTargetPos = invokeavilitytargetpos;
            currentGold = currentgold;
            currentLevel = currentlevel;
            currentXp = currentxp;
        }
    }

    private struct InterpolationState
    {
        public Vector3 position, rotation;
        public float serverTimestamp;

        public InterpolationState(Vector3 pos, Vector3 rot, float serverts)
        {
            position = pos;
            rotation = rot;
            serverTimestamp = serverts;
        }
    }
}