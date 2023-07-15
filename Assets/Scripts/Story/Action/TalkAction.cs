using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkAction : StoryActionBase
{
    public override string GetActionKey()
    {
        actionKey = ActionKey.TALK;
        return base.GetActionKey();
    }

    public override void StartAction(ActionData data, bool canSkip = false)
    {
        if (canSkip)
        {
            storyProcessor.Skip();
            return;
        }
        storyProcessor.SetTalk(
            data.Args[(int)TalkArgument.Owner],
            data.Args[(int)TalkArgument.Content],
            data.Args[(int)TalkArgument.Name], 
            data.Args[(int)TalkArgument.WaitMode]);
    }
}
