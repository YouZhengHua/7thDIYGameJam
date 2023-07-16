using System.Collections.Generic;
using UnityEngine.Events;

public interface IStoryProcessor
{
    void SetStoryActionBaseList(List<StoryActionBase> storyActionBases);
    T GetAsset<T>(string path);
    void ExecuteAction(ActionData data);
    void Repeat();
    void Skip();
    void OpenSkipBtn();
    void CloseSkipBtn();
    void CheckFadeModeFinish();
    void StopAction(string key);
    void SetStringVariable(string key, string value);
    void SetBooleanVariable(string key, bool value);
    void SendEvent(string key, string value);
    void SetTalk(string owner, string content, string name,string waitMode);
    void SetRole(string pos, string actionMode, string name, string animName, string loopMode, string waitMode);
    void SetFadeIn(float fadeInTime);
    void SetFadeOut(float fadeInTime);
    void SetBg(string bgName, float duration, string color, float alpha);
    void Shake();
    void SetOption(string btn1text,int btn1index, string btn2text, int btn2index, string owner, string content, string name);
    void PlaySound(string categoryName, string soundName);
    void PlayMusic(string musicName);
    void SetDelay(float duration);
}
