using Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoSingleton<DataManager>
{
    private List<string> _datakeyList = new List<string>();
    private readonly string _dataKeyFilePath = Path.Combine(Application.dataPath, "DataKeys.json");

    private void Awake()
    {
        // 파일 경로에 저장된 키값들이 있다면
        if (File.Exists(_dataKeyFilePath))
        {
            // 키값들을 추출하여 리스트에 담는다.
            string[] keyArr = File.ReadAllText(_dataKeyFilePath).Split(",");
            int saveFileCount = keyArr.Length - 1;

            for (int i = 0; i < saveFileCount; i++)
            {
                _datakeyList.Add(keyArr[i]);
            }
        }
    }

    // 데이터 저장 시 저장하려는 데이터 파일, 데이터 키 필요
    public void SaveData(JsonDataFile saveData, string dataKey)
    {
        // 데이터 키로 저장된 이력이 있는 데이터인지 확인
        if (!IsHaveData(dataKey))
        {
            string prevData = "";
            if (File.Exists(_dataKeyFilePath))
            {
                // 이미 존재하는 데이터 키들을 미리 prevData에 담아 놓는다.
                prevData = File.ReadAllText(_dataKeyFilePath);
            }

            //새로운 데이터 키 삽입
            string saveKey = prevData + $"{dataKey},";


            // 새로운 데이터 키가 삽입된 데이터 키들을 다시 저장
            File.WriteAllText(_dataKeyFilePath, saveKey);
            _datakeyList.Add(dataKey);
        }

        // 데이터 파일 경로를 불러오고 그곳에 저장
        File.WriteAllText(GetFilePath(dataKey), JsonUtility.ToJson(saveData));
    }

    // 데이터 로드 시 미리 저장된 데이터 키 필요
    public T LoadData<T>(string dataKey) where T : JsonDataFile
    {
        // 데이터 키로 저장된 이력이 있는 데이터인지 확인
        if (!IsHaveData(dataKey))
        {
            // 데이터 키가 없을 시 에러를 출력한다.
            Debug.LogWarning($"Error! No exit data key!! Key name : {dataKey}");
            return default(T);
        }

        // 데이터가 존재할 시 파일을 읽어 리턴해준다.
        return JsonUtility.FromJson<T>(File.ReadAllText(GetFilePath(dataKey)));
    }

    // 데이터 키를 활용한 데이터 유무를 간단히 확인
    public bool IsHaveData(string dataKey)
    {
        return _datakeyList.Contains(dataKey);
    }

    // 데이터 키를 받아 해당 파일이 없다면 생성 후 경로를 리턴
    private string GetFilePath(string dataKey)
    {
        return Path.Combine(Application.dataPath, $"{dataKey}.json");
    }
}
