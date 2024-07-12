using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PortalLinkData
{
    public Vector2Int linkPortalIndex_1;
    public Vector2Int linkPortalIndex_2;

    public PortalLinkData(Vector2Int idx1, Vector2Int idx2)
    {
        linkPortalIndex_1 = idx1;
        linkPortalIndex_2 = idx2;
    }
}

public class PortalDataSO : ScriptableObject
{
    public List<PortalLinkData> portalLinkList = new List<PortalLinkData>();

    public void SetPortalLinkData(Vector2Int idx1, Vector2Int idx2)
    {
        PortalLinkData portalLinkData = new PortalLinkData(idx1, idx2);
        portalLinkList.Add(portalLinkData);
    }
}
