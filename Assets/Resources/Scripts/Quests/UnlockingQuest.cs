using System.Collections.Generic;
using UnityEngine;

public class UnlockingQuest : Quest {
    public string text;
    public string target;
    public Dictionary<string, GameObject> unlockTargets = new Dictionary<string, GameObject>();

    public UnlockingQuest(string text, string target, Dictionary<string, GameObject> unlockTargets) {
        this.text = text;
        this.target = target;
        this.unlockTargets = unlockTargets;
    }

    public override void Init() {
        MessageLog.log.Publish(string.Format("New Quest: {0}", GetRepr()));
    }

    public override bool IsComplete() {
        if (unlockTargets[target].activeSelf) {
            Complete();
        }
        return complete;
    }

    public override string GetRepr() {
        return string.Format("{0}", text);
    }
}
