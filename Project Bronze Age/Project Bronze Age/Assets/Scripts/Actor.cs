using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    public string actorName;
    public string permanentId = Guid.NewGuid().ToString();

    protected virtual void Awake()
    {

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
}