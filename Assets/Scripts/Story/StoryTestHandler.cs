
using System.Collections;
using System.Collections.Generic;
using Scripts;
using Scripts.Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class StoryTestHandler : MonoBehaviour
{
    public StoryManager storyManager;
    public GameObject testBtn;
    public GameObject loadingTextObj;
    public Text stateText;

    [SerializeField, Header("使用者設定")]
    private UserSetting _userSetting;

    private int _index = 0;
    void Awake()
    {
        AudioController.Instance.SetUserSetting(_userSetting);
    }
    void Start()
    {
        loadingTextObj.SetActive(false);
        stateText.gameObject.SetActive(true);
        stateText.text = "waiting...";
    }
    public void StartStory()
    {

        testBtn.SetActive(false);
        stateText.text = "going...";
        storyManager.StartStory(_index, () =>
        {
            stateText.text = "finished";
            testBtn.SetActive(true);
        });
    }

    public void IndexChanged(string index)
    {
        _index = int.Parse(index);
    }
}
