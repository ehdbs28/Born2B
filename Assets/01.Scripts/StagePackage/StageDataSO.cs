using System.Collections.Generic;
using UnityEngine;
using StageDefine;

[CreateAssetMenu(menuName = "SO/Stage/Data")]
public class StageDataSO : ScriptableObject
{
    [Header("Stage info")]
    public StageType stageType;
    public string stageName;
    [TextArea] public string stageInfoText;

    [Header("Stage Element Data")]
    public List<string> data = new ();
    public int column;
    public PortalDataSO portalData;

    public void SetUp(string[,] getData)
    {
        data.Clear();

        int row = getData.GetLength(0);
        int column = getData.GetLength(1);

        this.column = row;

        for(int i = 0; i < column; i++)
        {
            for(int j = 0; j < row; j++)
            {
                data.Add(getData[j, i]);
            }
        }
    }
    public string[,] GetData()
    {
        int cnt = Mathf.FloorToInt(data.Count / column);
        string[,] board = new string[cnt, column];

        int num = 0;

        for (int i = 0; i < column; i++)
        {
            string[] d = data.GetRange(num, cnt).ToArray();

            for(int j = 0; j < d.Length; j++)
            {
                board[j, i] = d[j];
            }
            num += d.Length;
        }

        return board;
    }
    public void SetPortalData(PortalDataSO portaldata)
    {
        portalData = portaldata;
    }
}
