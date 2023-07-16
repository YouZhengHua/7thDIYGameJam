using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRoleAction : StoryActionBase
{
    public override string GetActionKey()
    {
        actionKey = ActionKey.SHOW_ROLE;
        return base.GetActionKey();
    }

    public override void StartAction(ActionData data, bool canSkip = false)
    {
        string waitMode = data.Args[(int)ShowRoleArgument.WaitMode];
        string actionMode = data.Args[(int)ShowRoleArgument.ActionMode];
        if (canSkip)
        {
            waitMode = "Continue";
            actionMode = "None";
        }
        storyProcessor.SetRole(
            data.Args[(int)ShowRoleArgument.Pos],
            actionMode,
            data.Args[(int)ShowRoleArgument.Name],
            data.Args[(int)ShowRoleArgument.AnimName],
            data.Args[(int)ShowRoleArgument.LoopMode],
            waitMode);
        if (canSkip)
        {
            storyProcessor.Skip();
            return;
        }
    }
}
