using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSystem : Singleton<DataSystem>
{
    public const string PLAYER_GAME_VALUE_DATA = "PlayerGameValueData";
    private GameValueData _gameValueData = null;

    public GameValueData gameValueData { get => _gameValueData; set => _gameValueData = value; }

    /// <summary>
    /// 儲存升級資料
    /// </summary>
    /// <param name="elementName">元素名稱</param>
    /// <param name="currentLevel">當前等級</param>
    public void SaveElementData(string elementName, int currentLevel)
    {
        if (_gameValueData.elementLevelDic.ContainsKey(elementName))
        {
            _gameValueData.elementLevelDic[elementName] = currentLevel;
        }
        else
        {
            _gameValueData.elementLevelDic.Add(elementName, currentLevel);
        }
        OnSaveData();
    }

    public void OnSaveData(Dictionary<string, object> message = null)
    {
        GameValueData cloneData = GameValueData.CloneForSave(_gameValueData);
        ES3.Save(PLAYER_GAME_VALUE_DATA, cloneData);
    }

    public void OnLoadData(Dictionary<string, object> message = null)
    {
        _gameValueData = ES3.Load(PLAYER_GAME_VALUE_DATA, new GameValueData());
    }
}

[System.Serializable]
public class GameValueData
{
    public long id = 0;
    public Dictionary<string, int> elementLevelDic = new Dictionary<string, int>();
    //TODO 這裡可以加入其他需要儲存的資料

    public GameValueData()
    {
        System.Random random = new System.Random();
        byte[] bytes = new byte[8];
        random.NextBytes(bytes);
        id = System.BitConverter.ToInt64(bytes, 0);
    }

    public static GameValueData CloneForSave(GameValueData data)
    {
        GameValueData cloneData = new GameValueData();
        cloneData.id = data.id;
        cloneData.elementLevelDic = new Dictionary<string, int>(data.elementLevelDic);
        return cloneData;
    }
}