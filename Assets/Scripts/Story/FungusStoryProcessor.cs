// using DG.Tweening;
using Fungus;
using Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FungusStoryProcessor : MonoBehaviour, IStoryProcessor
{
    public Flowchart flowchart;
    [Space(5)]
    [Header("Asset資源")]
    public StoryResourceData storyResourceData;
    [Space(5)]
    [Header("UI 元件")]
    public GameObject blockInput;
    public Image bgImage;
    public Image fadeImage;
    public Image skipFadeImage;
    public TMP_Text characterNameText;
    public RectTransform characterNameTextRectTransform;
    // public SkeletonGraphic leftSpineRole;
    // public SkeletonGraphic rightSpineRole;
    public GameObject leftRoleGameObject;
    public GameObject rightRoleGameObject;
    // public TwoBtnView twoBtnView;
    public Button skipBtn;

    // private Vibrator vibrator;
    private StroyController stroyController;
    private Dictionary<string, StoryActionBase> disStoryActionBases = new Dictionary<string, StoryActionBase>();
    private List<int> BtnIndexList = new List<int>();
    private bool _canSkip = false;
    private string currentAction = string.Empty;
    // private TrackEntryDelegate spineAnimEndToIdle;
    // private TrackEntryDelegate waitAnimFinish;

    private Coroutine fadeInCoroutine;
    private Coroutine fadeOutCoroutine;
    private void Start()
    {
        // twoBtnView.ResetBtn();
    }
    public void SetStroyController(StroyController StroyController)
    {
        stroyController = StroyController;
        // vibrator = GetComponent<Vibrator>();
    }

    public void SetStoryActionBaseList(List<StoryActionBase> storyActionBases)
    {
        foreach (var go in storyActionBases)
        {
            go.SetStoryProcessor(this);
            disStoryActionBases.Add(go.GetActionKey(), go);
        }
    }

    public void OpenSkipBtn()
    {
        _canSkip = false;
        skipBtn.gameObject.SetActive(true);
    }

    public void CloseSkipBtn()
    {
        //_canSkip = false;
        skipBtn.gameObject.SetActive(false);
    }

    public void SetCanSkip()
    {
        _canSkip = true;
        skipBtn.gameObject.SetActive(false);
        if (!string.IsNullOrEmpty(currentAction))
        {
            switch (currentAction)
            {
                case ActionKey.TALK:
                    StopAction(currentAction);
                    currentAction = string.Empty;
                    Skip();
                    break;
                case ActionKey.OPTION:
                    BtnClicked(0);
                    break;
            }
        }
    }

    public void SetNextAction()
    {
        //Fungus block will execute this method.
        stroyController.SetNextAction();
    }

    public void ActionFinished()
    {
        if (blockInput) blockInput.SetActive(false);
        //關掉顯示
        if (bgImage) bgImage.gameObject.SetActive(false);
        // leftSpineRole.gameObject.SetActive(false);
        // rightSpineRole.gameObject.SetActive(false);
        stroyController.ActionFinished();

        if (skipFadeImage.color.a == 1)
        {
            StartCoroutine(IFadeOut(0.5f, skipFadeImage));
        }
    }

    public void Repeat()
    {
        stroyController.Repeat();
    }

    public void Skip()
    {

        flowchart.ExecuteBlock("Skip");
    }

    public void ExecuteAction(ActionData data)
    {
        if (blockInput && !blockInput.activeSelf) blockInput.SetActive(true);
        if (disStoryActionBases.ContainsKey(data.Action))
        {
            if (_canSkip)
            {
                StartCoroutine(doSkipCover());
            }
            Debug.Log("ExecuteBlock action key = " + data.Action);
            disStoryActionBases[data.Action].StartAction(data, _canSkip);
            if (!_canSkip)
            {
                currentAction = data.Action;
                flowchart.ExecuteBlock(data.Action);
            }
            if (data.Action == ActionKey.OPTION && _canSkip)
            {
                _canSkip = false;
                skipBtn.gameObject.SetActive(true);
                currentAction = data.Action;
                flowchart.ExecuteBlock(data.Action);
            }
        }
        else
        {
            Debug.Log("invalid action key = " + data.Action + "! plx check data value of action field");
            Repeat();
            SetNextAction();
        }
    }

    private IEnumerator doSkipCover(Action callback = null)
    {
        Color colorCache = fadeImage.color;
        colorCache.a = 0;
        fadeImage.color = colorCache;
        Color colorCache1 = skipFadeImage.color;
        colorCache.a = 1;
        skipFadeImage.color = colorCache;
        //yield return IFadeIn(0f, skipFadeImage);
        callback?.Invoke();
        yield return new WaitUntil(() => { return !_canSkip; });
        StartCoroutine(IFadeOut(0.5f, skipFadeImage));
    }

    public T GetAsset<T>(string path)
    {
        T result = default(T);
        return result;
    }

    public void SendEvent(string key, string value)
    {

    }

    public void SetBooleanVariable(string key, bool value)
    {
        flowchart.SetBooleanVariable(key, value);
    }

    public void SetStringVariable(string key, string value)
    {
        flowchart.SetStringVariable(key, value);
    }

    public void StopAction(string key)
    {
        flowchart.StopBlock(key);
    }

    public void SetBg(string bgName, float duration, string color, float alpha)
    {
        if (string.IsNullOrEmpty(bgName) || bgName == "none")
        {
            bgImage.gameObject.SetActive(false);
            return;
        }

        //if (bgName.Contains(animBgString))
        //{
        //    animBG.SetTrigger(bgName.Substring(animBgString.Length));
        //    return;
        //}
        //else
        //    animBG.SetTrigger("None");

        bgImage.gameObject.SetActive(true);
        if (!string.IsNullOrEmpty(color))
        {
            switch (color)
            {
                case "Black":
                    bgImage.color = Color.black;
                    break;
                case "White":
                    bgImage.color = Color.white;
                    break;
            }
        }
        else
        {
            bgImage.color = Color.white;
        }
        var tempColor = bgImage.color;
        tempColor.a = alpha;
        bgImage.color = tempColor;


        if (bgImage.sprite == null || bgName != bgImage.sprite.name)
        {
            Sprite _sp = storyResourceData.GetBackground(bgName);
            if (_sp)
            {
                bgImage.sprite = _sp;
            }
            else
            {
                Debug.Log("cant find the image by this bg name! name is " + bgName);
                bgImage.sprite = null;
            }
        }
    }

    public void SetTalk(string owner, string content, string name, string waitMode)
    {
        flowchart.SetStringVariable("playerName", getPlayerName());

        setOwner(owner);
        flowchart.SetStringVariable("Dialogue", content);
        if (characterNameText)
        {
            if (name == "{$playerName}")
            {
                name = flowchart.GetStringVariable("playerName");
            }
            characterNameText.text = name;
        }
        switch (waitMode)
        {
            case "WaitInput":
                break;
            case "Continue":
                break;
        }
    }
    private string getPlayerName()
    {
        // if (Helper.UserDB.UserInfo != null && !string.IsNullOrEmpty(Helper.UserDB.UserInfo.NickName))
        // {
        //     return Helper.UserDB.UserInfo.NickName;
        // }
        // else
        // {
        //     return "playerName";
        // }
        //TODO: get player name? 如果玩家可以自由設定的話
        return "playerName";
    }
    private void setOwner(string owner)
    {
        switch (owner)
        {
            // case "Left":
            //     leftSpineRole.material.SetColor("_Color", Color.white);
            //     rightSpineRole.material.SetColor("_Color", Color.gray);
            //     characterNameTextRectTransform.anchoredPosition = new Vector2(-580, 234);
            //     break;
            // case "Right":
            //     rightSpineRole.material.SetColor("_Color", Color.white);
            //     leftSpineRole.material.SetColor("_Color", Color.gray);
            //     characterNameTextRectTransform.anchoredPosition = new Vector2(-160, 234);
            //     break;
            // case "Both":
            //     rightSpineRole.material.SetColor("_Color", Color.white);
            //     leftSpineRole.material.SetColor("_Color", Color.white);
            //     characterNameTextRectTransform.anchoredPosition = new Vector2(-580, 234);
            //     break;
            // case "None":
            //     leftSpineRole.gameObject.SetActive(false);
            //     rightSpineRole.gameObject.SetActive(false);
            //     break;
        }
    }

    public void SetRole(string pos, string actionMode, string name, string animName, string loopMode, string waitMode)
    {
        Animator animator = null;
        GameObject roleGameObject = null;
        Vector2 FinPos = new Vector2(pos == "Left" ? -429 : 381, -247);
        switch (pos)
        {
            case "Left":
                roleGameObject = leftRoleGameObject;
                //TODO 圖片反轉
                // role.initialFlipX = true;
                break;
            case "Right":
                roleGameObject = rightRoleGameObject;
                break;
        }

        if (roleGameObject == null)
        {
            Debug.LogError("roleGameObject is null");
            return;
        }
        Sprite _sp = storyResourceData.GetCharacter(name);
        if (_sp == null)
        {
            Debug.Log("cant find the image by this character name! name is " + name);
        }
        roleGameObject.GetComponent<Image>().sprite = _sp;
        roleGameObject.GetComponent<Image>().SetNativeSize();

        //     switch (waitMode)
        //     {
        //         case "Continue":
        //             flowchart.SetBooleanVariable("Delay", false);
        //             break;
        //         case "WaitFinish":
        //             if (loopMode != "Loop")
        //             {
        //                 flowchart.SetBooleanVariable("Delay", true);
        //                 waitAnimFinish = (trackEntry) =>
        //                 {
        //                     flowchart.SetBooleanVariable("Delay", false);
        //                     role.AnimationState.Complete -= waitAnimFinish;
        //                 };
        //                 role.AnimationState.Complete += waitAnimFinish;
        //             }
        //             break;
        //     }

        if (string.IsNullOrEmpty(actionMode)) actionMode = "None";
        switch (actionMode)
        {
            case "Show":
            case "FadeIn":
            case "MoveIn":
                roleGameObject.gameObject.SetActive(true);
                break;
            case "FadeOut":
            case "MoveOut":
            case "Out":
                roleGameObject.gameObject.SetActive(false);
                break;
            case "None":
                break;
        }
    }

    public void SetFadeIn(float fadeInTime)
    {
        flowchart.SetBooleanVariable("Delay", true);
        fadeInCoroutine = StartCoroutine(IFadeIn(fadeInTime));
    }

    public void SetFadeOut(float fadeInTime)
    {
        flowchart.SetBooleanVariable("Delay", true);
        fadeOutCoroutine = StartCoroutine(IFadeOut(fadeInTime));
    }

    private IEnumerator IFadeIn(float fadeInTime, Image target = null)
    {
        Image fadeItem = fadeImage;
        if (target != null) fadeItem = target;
        float timeCache = Time.time;
        Color colorCache = fadeItem.color;
        while (Time.time - timeCache < fadeInTime)
        {
            yield return new WaitForEndOfFrame();
            colorCache.a = Mathf.Clamp01((Time.time - timeCache) / fadeInTime);
            fadeItem.color = colorCache;
        }
        flowchart.SetBooleanVariable("Delay", false);
        fadeInCoroutine = null;
    }

    private IEnumerator IFadeOut(float fadeOutTime, Image target = null)
    {
        Image fadeItem = fadeImage;
        if (target != null) fadeItem = target;
        float timeCache = Time.time;
        Color colorCache = fadeItem.color;
        while (Time.time - timeCache < fadeOutTime)
        {
            yield return new WaitForEndOfFrame();
            colorCache.a = 1 - Mathf.Clamp01((Time.time - timeCache) / fadeOutTime);
            fadeItem.color = colorCache;
        }
        flowchart.SetBooleanVariable("Delay", false);
        fadeOutCoroutine = null;
    }

    public void CheckFadeModeFinish()
    {
        if (fadeInCoroutine != null)
        {
            StopCoroutine(fadeInCoroutine);
            fadeInCoroutine = null;
        }
        if (fadeImage.color.a > 0)
        {
            Color colorCache = fadeImage.color;
            colorCache.a = 0;
            fadeImage.color = colorCache;
            flowchart.SetBooleanVariable("Delay", false);
        }
    }

    public void Shake()
    {
        //Vibrator vibratorCache = _actionDatas[_actionIndex].Args[(int)ShakeArgument.character] == "right" ? rightVibrator : leftVibrator;
        //float.TryParse(_actionDatas[_actionIndex].Args[(int)ShakeArgument.duringTime], out float duringTime);
        //float.TryParse(_actionDatas[_actionIndex].Args[(int)ShakeArgument.amplitude], out float amplitude);
        //float.TryParse(_actionDatas[_actionIndex].Args[(int)ShakeArgument.zRotationAmplitude], out float zRotationAmplitude);
        //bool.TryParse(_actionDatas[_actionIndex].Args[(int)ShakeArgument.delayWhileDone], out bool delayWhileDone);
        //flowchart.SetBooleanVariable("Delay", delayWhileDone);

        // vibrator.Shake();
    }

    public void SetOption(string btn1text, int btn1index, string btn2text, int btn2index, string owner, string name, string content)
    {
        Debug.Log("setBtn");
        BtnIndexList.Add(btn1index);
        BtnIndexList.Add(btn2index);

        // twoBtnView.SetBtn(btn1text, btn2text);
        setOwner(owner);
        // twoBtnView.SetContent(content, name, owner);
    }

    public void BtnClicked(int index)
    {
        //insert action index 
        stroyController.insertAction?.Invoke(BtnIndexList[index]);

        //reset btns view and cache
        BtnIndexList.Clear();
        // twoBtnView.ResetBtn();

        stroyController.SetNextAction();
    }

    public void PlaySound(string categoryName, string soundName)
    {
        AudioClip _ac = storyResourceData.GetAudio(soundName);
        if (_ac != null)
            AudioController.Instance.PlayEffect(_ac);
    }

    public void PlayMusic(string musicName)
    {
        AudioClip _ac = storyResourceData.GetAudio(musicName);
        if (_ac != null)
            AudioController.Instance.PlayEffect(_ac);
    }

    public void SetDelay(float duration)
    {
        StartCoroutine(setDelay(duration));
    }

    private IEnumerator setDelay(float duration)
    {
        flowchart.SetBooleanVariable("Delay", true);
        yield return new WaitForSeconds(duration);
        flowchart.SetBooleanVariable("Delay", false);
    }
}
