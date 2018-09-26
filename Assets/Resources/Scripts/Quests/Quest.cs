public abstract class Quest {
    // protected allows subclass to access
    protected bool complete;
    public abstract void Init();
    public abstract bool IsComplete();
    public void Complete() {
        complete = true;
        MessageLog.log.Publish(string.Format("Completed Quest: {0}", GetCompletedRepr()));
    }
    public abstract string GetRepr();
    public virtual string GetCompletedRepr() {
        return GetRepr();
    }
}
