﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public abstract class Actor : MonoBehaviour, IGlobalID
{
    public const int interpolationTimeDelta = 100, interpolationBufferLength = 20;
    public const float interpolationBacktime = 0.1f,
        // Client sent kilobytes (15 messages) per second: 0,57kb (ClientRequest), Server sent kilobytes (15 messages) per second: 0,6kb (ServerEvent), 0,72kb (ServerState)
        networkClientSendRateInterval = 0.066f, networkServerSendRateInterval = 0.066f; // 15ms/15ms! Are these send rates good?
    
    //[SerializeField]
    private int _networkId;
    public string actorName;

    // Networking
    protected ServerEvent nextServerEvent;
    protected ServerState nextServerState;

    private InterpolationState[] interpolationStateBuffer;
    private int timestampCount;
    private float serverMessageSendRateTime;

    public int globalId
    {
        get { return _networkId; }
    }

    protected virtual void Awake()
    {
        interpolationStateBuffer = new InterpolationState[interpolationBufferLength];
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        // If server
        nextServerState.timestamp = 1; // Findout timestamp
        nextServerState.position = transform.position;
        nextServerState.rotation = transform.rotation.eulerAngles;

        // If Server
        serverMessageSendRateTime += Time.deltaTime;
        if(serverMessageSendRateTime >= networkClientSendRateInterval)
        {
            serverMessageSendRateTime = 0;

            SendServerEvent();
            SendServerState();
        }

        // If client
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
    protected virtual void OnApplyServerEvent(ServerEvent ev)
    {
        
    }
    // Client
    protected virtual void OnApplyServerState(ServerState state)
    {
        float timestamp = 1;
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
    protected virtual void OnReceiveClientRequest(ClientRequest rq)
    {

    }

    protected virtual void OnSendNetworkMessage()
    {
        // If server
        SendServerEvent();
        SendServerState();
    }

    // Server
    private void SendServerEvent()
    {
        // Check if server and then send nextServerEvent
    }
    // Server
    private void SendServerState()
    {
        // TODO: Check if server and then send nextServerState
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