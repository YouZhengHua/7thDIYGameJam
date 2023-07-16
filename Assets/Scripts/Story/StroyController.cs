using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;
using System;
using System.Reflection;

public class StroyController : MonoBehaviour
{
    public Image topBoard;
    public Image bottomBoard;
    public Animator animBG;
    public Transform BehindSayDialog;
    public Transform InFrontOfSayDialog;
   


    public delegate void InsertAction(int index);
    public InsertAction insertAction;

    private ActionData[] _actionDatas = null;
    private Action _callback;
    private int _actionIndex = 0;
    private List<int> BtnIndexList = new List<int>();
    private const string animBgString = "anim_";
    private IStoryProcessor storyProcessor;

    void Start()
    {
    }

    public void SetStoryProcessor(IStoryProcessor istoryProcessor)
    {
        storyProcessor = istoryProcessor;
        Assembly a = Assembly.GetExecutingAssembly();
        List<StoryActionBase> storyActionBaseList = new List<StoryActionBase>();
        foreach (Type type in a.GetTypes())
        {
            if (type.IsSubclassOf(typeof(StoryActionBase)))
            {
                StoryActionBase sa = Activator.CreateInstance(type) as StoryActionBase;
                storyActionBaseList.Add(sa);
                Debug.Log("type = " + type.Name + " key  = " + sa.GetActionKey());
            }
        }
        storyProcessor.SetStoryActionBaseList(storyActionBaseList);
    }

    public void SetActionData(ActionData[] data, Action callback = null)
    {
        if (_actionDatas != null)
        {
            List<ActionData> tempActionDatas = new List<ActionData>();
            for (int i = 0; i < data.Length; i++)
            {
                tempActionDatas.Add(data[i]);
            }
            if (_actionIndex != _actionDatas.Length - 1)
            {
                for (int i = _actionIndex + 1; i < _actionDatas.Length; i++)
                {
                    tempActionDatas.Add(_actionDatas[i]);
                }
            }
            _actionDatas = tempActionDatas.ToArray();
            _actionIndex = 0;
        }
        else
        {
            _actionDatas = data;
        }

        if (callback != null)
        {
            _callback = callback;
        }
    }

    public void StartStory(Action onComplete = null)
    {
        storyProcessor.SetBooleanVariable("Finished", false);
        StartCoroutine(Execute(onComplete));
        storyProcessor.OpenSkipBtn();
    }

    public IEnumerator Execute(Action onComplete = null)
    {
        if (onComplete != null)
        {
            _callback = onComplete;
        }
        yield return new WaitForEndOfFrame();
        storyProcessor.StopAction(ActionKey.ACTION);
        //yield return new WaitForEndOfFrame();
        storyProcessor.ExecuteAction(_actionDatas[_actionIndex]);
        
    }

    public void SetNextAction()
    {
        StartCoroutine(Execute());
    }

    public IEnumerator dialogueShow(float time, Action<float> action, Action callBack = null)
    {
        float timeCache = Time.time + time;

        while (Time.time < timeCache)
        {
            yield return new WaitForEndOfFrame();

            float percent = 1 - (timeCache - Time.time) / time;

            if (action != null)
                action.Invoke(percent);
        }

        if (callBack != null)
            callBack.Invoke();
    }

    public void ActionFinished()
    {
        _actionIndex = 0;
        _actionDatas = null;
        //DoCallBack
        if (_callback != null)
        {
            _callback();
        }
        storyProcessor.CloseSkipBtn();
        storyProcessor.CheckFadeModeFinish();
        //Services.Get<EventManager>().SendEvent((int)EGameEvents.SetPlayerControl, true);
    }

    public void Repeat()
    {
        if (_actionIndex == _actionDatas.Length - 1)
        {
            storyProcessor.SetBooleanVariable("Finished", true);
        }
        else
        {
            _actionIndex++;
        }
    }
}
