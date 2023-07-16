using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionData
{
    public List<string> Args = new List<string>();
    public string Action;
    public string Arg1;
    public string Arg2;
    public string Arg3;
    public string Arg4;
    public string Arg5;
    public string Arg6;
    public string Arg7;

    public static ActionData Clone(string Action,
    string Arg1,
    string Arg2,
    string Arg3,
    string Arg4,
    string Arg5,
    string Arg6,
    string Arg7)
    {
        ActionData actionData = new ActionData();
        actionData.Action = Action;
        actionData.Args.Add(Arg1);
        actionData.Args.Add(Arg2);
        actionData.Args.Add(Arg3);
        actionData.Args.Add(Arg4);
        actionData.Args.Add(Arg5);
        actionData.Args.Add(Arg6);
        actionData.Args.Add(Arg7);
        return actionData;
    }
}

public class ActionKey
{
    public const string ACTION = "Action";
    public const string SHOW_ROLE = "ShowRole";
    public const string SHOW_BACKGROUND = "ShowBackGround";
    public const string TALK = "Talk";
    public const string OPTION = "Option";
    public const string SET_SCREEN = "SetScreen";
    public const string SHAKE = "Shake";
    public const string PLAY_MUSIC = "PlayMusic";
    public const string PLAY_SOUND = "PlaySound";
    public const string DELAY = "Delay";
}

public class OperationKey
{
    public const string ShowInterViewResult = "showInterViewResult";
    public const string Open = "open";
}

public enum TalkArgument
{
    Owner = 0,
    Name,
    Content,
    WaitMode,
}

public enum ShowBackGroundArgument
{
    Name = 0,
    Duration,
    Color,
    Alpha,
}

public enum ShowRoleArgument
{
    Pos = 0,
    ActionMode,
    Name,
    AnimName,
    LoopMode,
    WaitMode,
}

public enum OptionArgument
{
    Btn1text = 0,
    Btn1index,
    Btn2text,
    Btn2index,
    Owner,
    Name,
    Content,
}

public enum PlaySoundArgument
{
    CategoryName = 0,
    SoundName,
}

public enum PlayMusicArgument
{
    MusicName = 0,
}

public enum SetScreenArgument
{
    Mode = 0,
    Color,
    Duration,
}

public enum DelayArgument
{
    Duration = 0,
}

public enum PropertyArgument
{
    propertyKey = 0,
    propertyValue,
}

public enum OperationArgument
{
    operationKey = 0,
}

public enum OpenArgument
{
    openMapName = 1,
}

public enum UIButtonArgument
{
    buttonName = 0,
    buttonValue
}

public enum PlaceArgument
{
    placeName = 0,
}

public enum KarmaRangeArgument
{
    minValue = 0,
    maxValue
}

public enum CharacterPosArgument
{
    duringTime = 0,
    delayWhileDone,
    character,
    pos,
    behind
}

public enum ShakeArgument
{
    duringTime = 0,
    delayWhileDone,
    character,
    amplitude,
    zRotationAmplitude,
}

public enum ZoomArgument
{
    duringTime = 0,
    delayWhileDone,
    target,
    scaleDelta,
}

public enum FavorabilityArgument
{
    characterName = 0,
    value
}

public enum BoardArgument
{
    duringTime = 0,
    delayWhileDone,
    target,
    position,
    alpha
}

public enum GetItemArgument
{
    itemName = 0
}

public enum HighLightArgument
{
    buttonName = 0
}

public enum FadeArgument
{
    fadeInTime = 0,
    fadeOutTime,
    stayTime,
    bgImageName = 6
}

public class CharacterData
{
    public string code;
    public string Charactername;
}
