using Fungus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public FungusStoryProcessor fungusStoryProcessor;
    public data1 data1_SO_file;
    private data1Data[] dataArray;
    private StroyController stroyController;

    void OnEnable()
    {
        // EventManager.StartListening(EGameEvents.StoryManager_Response_StartStory.ToString(), OnStartStory);
    }

    void OnDisable()
    {
        // EventManager.StopListening(EGameEvents.StoryManager_Response_StartStory.ToString(), OnStartStory);
    }

    private void Awake()
    {
        dataArray = data1_SO_file.dataArray;
        stroyController = gameObject.AddComponent<StroyController>();
        stroyController.SetStoryProcessor(fungusStoryProcessor);
        fungusStoryProcessor.SetStroyController(stroyController);
    }

    private void OnStartStory(Dictionary<string, object> message)
    {
        if (message.ContainsKey("index") && message.ContainsKey("callback"))
        {
            StartStory((int)message["index"], (Action)message["callback"]);
        }
    }

    public void StartStory(int index, Action callback = null)
    {
        ActionData[] actionDatas = CreateActionDatas(index, dataArray);
        if (actionDatas == null)
        {
            callback?.Invoke();
            Debug.Log("No ActionData for this index !");
            return;
        }
        stroyController.SetActionData(actionDatas, callback);
        stroyController.StartStory();
        stroyController.insertAction = InsertChapterData;
    }

    private void InsertChapterData(int index)
    {
        stroyController.SetActionData(CreateActionDatas(index, dataArray));
    }

    /// <summary>
    /// Parse the TutorialData to ActionData.
    /// </summary>
    /// <param name="index">The index is marked in the story information line to be packaged</param>
    /// <param name="datas">TutorialDatas</param>
    /// <returns></returns>
    private ActionData[] CreateActionDatas(int index, data1Data[] datas)
    {
        if (Array.Find(datas, d => d.Index == index) == null)
        {
            return null;
        }
        data1Data[] data = Array.FindAll(datas, d => d.Index == index);
        ActionData[] _actionDatas = new ActionData[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            _actionDatas[i] = ActionData.Clone(
                data[i].Action,
                data[i].Arg1,
                data[i].Arg2,
                data[i].Arg3,
                data[i].Arg4,
                data[i].Arg5,
                data[i].Arg6,
                data[i].Arg7
                );
        }
        return _actionDatas;
    }
}

