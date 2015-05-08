using UnityEngine;
using System.Collections;
using System;

public interface ILoadable
{
    float progress { get; }
    bool isLoading { get; }
    bool isDone { get; }
    bool loadErrorOccured { get; }
}
