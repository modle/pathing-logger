using UnityEngine;

public class BuildingQuest : Quest {
    public string text;
    public string target;
    public GameObject parentObject;
    public int amount;
    public int start;

    public BuildingQuest(string text, string target, GameObject parentObject, int amount) {
        this.text = text;
        this.target = target;
        this.parentObject = parentObject;
        this.amount = amount;
        this.start = 0;
    }

    public override void Init() {
        MessageLog.log.Publish(string.Format("New Quest: {0}", GetRepr()));
        start = CountTargets();
    }

    private int CountTargets() {
        int count = 0;
        foreach (Transform child in parentObject.transform) {
            if (child.name == target) {
                count += 1;
            }
        }
        return count;
    }

    public override bool IsComplete() {
        if (CountTargets() - start >= amount) {
            Complete();
        }
        return complete;
    }

    public override string GetRepr() {
        return string.Format("{0}", text);
    }
}
