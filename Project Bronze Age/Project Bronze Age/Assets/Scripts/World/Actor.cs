using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    public const int maxTransformInterpolationBuffer = 25;

    public string permanentId = Guid.NewGuid().ToString();
    public string actorName;

    private Stack<TransformState> transformStates;

    protected virtual void Awake()
    {
        transformStates = new Stack<TransformState>();
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
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

    protected virtual void OnSerializeServerStream(BitStream stream)
    {
        Vector3 position = transform.position;
        stream.Serialize(ref position);

        Quaternion rotation = transform.rotation;
        stream.Serialize(ref rotation);
    }
    protected virtual void OnSerializeClientStream(BitStream stream)
    {
        Vector3 position = Vector3.zero;
        stream.Serialize(ref position);

        Quaternion rotation = Quaternion.identity; // Are the euler-angles maybe enough? Then we only need to use a Vector3
        stream.Serialize(ref rotation);

        InterpolateTransform(position, rotation);
    }

    private void OnSerializeBitStream(BitStream stream)
    {
        if (stream.isWriting)
            OnSerializeServerStream(stream);
        else
            OnSerializeClientStream(stream);
    }


    /*
        Is this good?
        Another attempt:

        Server Data:
        -Timestamp
        -Position
        -Rotation

        Client buffers it and checks every frame:
        if(currentClientTimestamp - interpolationTimeDelta < lastServerData.timestamp)
        {
            Apply the server data to the client actor
        }

        TODO: Check if this is right
    */
    private void InterpolateTransform(Vector3 position, Quaternion rotation)
    {
        // Is this really good?

        TransformState tState = new TransformState(position, rotation);
        transformStates.Push(tState);

        if (transformStates.Count > maxTransformInterpolationBuffer)
        {
            TransformState currentState = transformStates.Pop();
            transform.position = Vector3.Lerp(transform.position, currentState.position, 1);
            transform.rotation = Quaternion.Lerp(transform.rotation, currentState.rotation, 1); // Really t=1?
        }
    }

    private struct TransformState
    {
        public Vector3 position;
        public Quaternion rotation; // Maybe we only need 

        public TransformState(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }
}