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
    //protected ClientRequest clientNextRequest;
    //protected ServerEvent serverNextEvent;
    //protected ServerState serverNextState;

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
        PerformInterpolation(1 /* Find client timestamp (like Network.time) */);
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

    // Client
    protected void SendClientRequest(ClientRequest rq)
    {
        // TODO: Check if client and "this == local player (owner)" and then send the client request
    }
    // Client
    protected virtual void OnApplyServerEvent(ServerEvent ev)
    {
        
    }
    // Client
    protected virtual void OnApplyServerState(ServerState state, float timestamp)
    {
        if (state.position != default(Vector3)) // if the pos. isnt default the rot. isnt too
        {
            for (int i = interpolationStateBuffer.Length - 1; i >= 1; i--)
            {
                interpolationStateBuffer[i] = interpolationStateBuffer[i - 1];
            }

            InterpolationState newState = new InterpolationState(state.position, state.rotation, timestamp);
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
    protected void SendServerEvent(ServerEvent ev)
    {
        // Check if server and then send the server command
    }
    // Server
    protected void SendServerState(ServerState state)
    {
        // TODO: Check if server and then send the server command
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