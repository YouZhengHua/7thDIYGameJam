using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendEventAction : StoryActionBase
{
    public override string GetActionKey()
    {
        actionKey = ActionKey.SEND_EVENT;
        return base.GetActionKey();
    }

    public override void StartAction(ActionData data, bool canSkip = false)
    {
        storyProcessor.SendEvent(data.Args[(int)PropertyArgument.propertyKey], data.Args[(int)PropertyArgument.propertyValue]);
    }
}
