using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundAction : StoryActionBase
{
    public override string GetActionKey()
    {
        actionKey = ActionKey.PLAY_SOUND;
        return base.GetActionKey();
    }

    public override void StartAction(ActionData data, bool canSkip = false)
    {
        if (canSkip)
        {
            storyProcessor.Skip();
            return;
        }
        storyProcessor.PlaySound(
            data.Args[(int)PlaySoundArgument.CategoryName],
            data.Args[(int)PlaySoundArgument.SoundName]);
    }
}
