using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryAdaptor : MonoBehaviour
{
    [SerializeField] private StageManager stageManager;
    private StoryManager storyManager;

    private void Awake() {
        storyManager = GetComponent<StoryManager>();
    }

    private void Start() {
    }
}
