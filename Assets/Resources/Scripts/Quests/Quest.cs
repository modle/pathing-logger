public abstract class Quest {
    // protected allows subclass to access
    protected bool complete;
    public abstract void Init();
    public abstract bool IsComplete();
    public void Complete() {
        complete = true;
        MessageLog.log.Publish(string.Format("Quest: {0} COMPLETED", GetRepr()));
    }
    public abstract string GetRepr();
}
