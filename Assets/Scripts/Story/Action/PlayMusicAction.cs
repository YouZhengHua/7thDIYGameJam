using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicAction : StoryActionBase
{
    public override string GetActionKey()
    {
        actionKey = ActionKey.PLAY_MUSIC;
        return base.GetActionKey();
    }

    public override void StartAction(ActionData data, bool canSkip = false)
    {
        if (canSkip)
        {
            storyProcessor.Skip();
            return;
        }
        storyProcessor.PlayMusic(
            data.Args[(int)PlayMusicArgument.MusicName]);
    }
}
