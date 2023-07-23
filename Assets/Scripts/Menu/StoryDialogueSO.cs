using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StoryDialogueSO : ScriptableObject
{
    [Serializable]
    public struct stage_dialogue {
        public StageManager.stage stage;

        [Tooltip("Need consecutive numbers!")]
        public List<int> dialougeIndex;
        public List<int> defeatedDialogueIndex;
    }

    public stage_dialogue[] dialogues;
}
