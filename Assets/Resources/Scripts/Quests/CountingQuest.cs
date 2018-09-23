using System.Collections.Generic;

public class CountingQuest : Quest {
    public string text;
    public Dictionary<string, int> things;
    public string toCount;
    public int amount;
    public int start;

    public CountingQuest(string text, Dictionary<string, int> things, string toCount, int amount) {
        this.text = text;
        this.things = things;
        this.toCount = toCount;
        this.amount = amount;
        start = things[toCount];
    }

    public override void Init() {
        start = things[toCount];
        MessageLog.log.Publish(string.Format("New Quest: {0}", GetRepr()));
    }

    public override bool IsComplete() {
        if (things[toCount] >= start + amount) {
            Complete();
        }
        return complete;
    }

    public override string GetRepr() {
        return string.Format("{0}: {1}/{2}", text, things[toCount] - start, amount);
    }
}
