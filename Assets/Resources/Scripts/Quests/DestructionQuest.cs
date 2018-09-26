using System.Collections.Generic;

public class DestructionQuest : Quest {
    public string text;
    public string toCount;
    public int start;
    public int current;

    public DestructionQuest(string text, string toCount) {
        this.text = text;
        this.toCount = toCount;
    }

    public override void Init() {
        start = TargetBucket.bucket.CountAll(toCount);
        MessageLog.log.Publish(string.Format("New Quest: {0}", GetStartMessage()));
    }

    public override bool IsComplete() {
        UpdateCount();
        if (current <= 0) {
            Complete();
        }
        return complete;
    }

    private void UpdateCount() {
        current = TargetBucket.bucket.CountAll(toCount);
    }

    private string GetStartMessage() {
        return string.Format("{0}: {1}", text, start - current);
    }

    public override string GetRepr() {
        return string.Format("{0}: {1}", text, current);
    }
}
