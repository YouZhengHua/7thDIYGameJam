using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeAction : StoryActionBase
{
    public override string GetActionKey()
    {
        actionKey = ActionKey.SHAKE;
        return base.GetActionKey();
    }

    public override void StartAction(ActionData data, bool canSkip = false)
    {
        if (canSkip) { 
            storyProcessor.Skip();
            return;
        }
        storyProcessor.Shake();
    }
}
