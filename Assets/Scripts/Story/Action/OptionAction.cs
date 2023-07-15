using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionAction : StoryActionBase
{
    public override string GetActionKey()
    {
        actionKey = ActionKey.OPTION;
        return base.GetActionKey();
    }

    public override void StartAction(ActionData data, bool canSkip = false)
    {
        int index1 = 0;
        int index2 = 1;
        if (!string.IsNullOrEmpty(data.Args[(int)OptionArgument.Btn1index]))
            index1 = int.Parse(data.Args[(int)OptionArgument.Btn1index]);
        if (!string.IsNullOrEmpty(data.Args[(int)OptionArgument.Btn2index]))
            index2 = int.Parse(data.Args[(int)OptionArgument.Btn2index]);

        storyProcessor.SetOption(
            data.Args[(int)OptionArgument.Btn1text],
           index1,
            data.Args[(int)OptionArgument.Btn2text],
            index2, 
            data.Args[(int)OptionArgument.Owner], 
            data.Args[(int)OptionArgument.Name], 
            data.Args[(int)OptionArgument.Content]);
    }
}
