using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class FlowSlot
{
    public EventType eventType;
    [Space(5f)] public UnityEvent<object[]> onFlowEvent;
}
