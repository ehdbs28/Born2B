using UnityEngine.Events;

public class FlowSlot
{
    public EventType eventType;
    public UnityEvent<object[]> onFlowEvent;
}
