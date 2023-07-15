using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBackGroundAction : StoryActionBase
{
    public override string GetActionKey()
    {
        actionKey = ActionKey.SHOW_BACKGROUND;
        return base.GetActionKey();
    }

    public override void StartAction(ActionData data, bool canSkip = false)
    {
        float duration = 1;
        if (!string.IsNullOrEmpty(data.Args[(int)ShowBackGroundArgument.Duration]))
            duration = float.Parse(data.Args[(int)ShowBackGroundArgument.Duration]);
        float alpha = 1;
        if (!string.IsNullOrEmpty(data.Args[(int)ShowBackGroundArgument.Alpha]))
            alpha = float.Parse(data.Args[(int)ShowBackGroundArgument.Alpha]);

        if (canSkip)
        {
            duration = 0;
        }
        storyProcessor.SetBg(
            data.Args[(int)ShowBackGroundArgument.Name],
             duration,
              data.Args[(int)ShowBackGroundArgument.Color],
               alpha
            );
        if (canSkip)
        {
            storyProcessor.Skip();
            return;
        }
    }
}
