using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetScreenAction : StoryActionBase
{
    public override string GetActionKey()
    {
        actionKey = ActionKey.SET_SCREEN;
        return base.GetActionKey();
    }
    public override void StartAction(ActionData data, bool canSkip = false)
    {
        if (canSkip)
        {
            storyProcessor.Skip();
            return;
        }
        float fadeDuration = 1;
        if (!string.IsNullOrEmpty(data.Args[(int)SetScreenArgument.Duration]))
            fadeDuration = float.Parse(data.Args[(int)SetScreenArgument.Duration]);

        switch (data.Args[(int)SetScreenArgument.Mode])
        {
            case "FadeIn":
                storyProcessor.SetFadeIn(fadeDuration);
                break;
            case "FadeOut":
                storyProcessor.SetFadeOut(fadeDuration);
                break;
        }
    }
}
