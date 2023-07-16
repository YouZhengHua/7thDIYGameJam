using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayAction : StoryActionBase
{
    public override string GetActionKey()
    {
        actionKey = ActionKey.DELAY;
        return base.GetActionKey();
    }

    public override void StartAction(ActionData data, bool canSkip = false)
    {
        if (canSkip)
        {
            storyProcessor.Skip();
            return;
        }
        float duration = 1;
        if (!string.IsNullOrEmpty(data.Args[(int)DelayArgument.Duration]))
            duration = float.Parse(data.Args[(int)DelayArgument.Duration]);
        storyProcessor.SetDelay(duration);
    }
}
