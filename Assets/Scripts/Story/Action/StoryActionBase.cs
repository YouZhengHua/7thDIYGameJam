

public abstract class StoryActionBase
{
    protected static string actionKey = string.Empty;
    protected IStoryProcessor storyProcessor;
    public virtual string GetActionKey()
    {
        return actionKey;
    }

    public void SetStoryProcessor(IStoryProcessor istoryProcessor)
    {
        storyProcessor = istoryProcessor;
    }

    public abstract void StartAction(ActionData data, bool canSkip = false);
}
