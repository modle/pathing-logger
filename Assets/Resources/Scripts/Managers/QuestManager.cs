using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager : MonoBehaviour {

    public static QuestManager manager;

    private List<Quest> quests = new List<Quest>();
    private int currentQuestIndex = 0;
    private Quest currentQuest;
    private GameObject questContainer;

    void Awake() {
        // singleton pattern
        if (manager == null) {
            manager = this;
        } else if (manager != this) {
            Destroy(gameObject);
        }
        questContainer = GameObject.Find("QuestContainer");
    }

    void Start() {
        DefineQuests();
        SetQuest();
    }

    void Update() {
        SetText();
        UpdateQuest();
    }

    void DefineQuests() {
        Dictionary<string, int> resources = ResourceCounter.counter.counts;
        quests.Add(new CountingQuest("harvest trees", resources, "wood", 2));
        quests.Add(new CountingQuest("harvest rocks", resources, "rock", 2));
        quests.Add(new UnlockingQuest("unlock the Sawyer"));
    }

    void SetQuest() {
        if (quests.Count - 1 >= currentQuestIndex) {
            currentQuest = quests[currentQuestIndex];
            currentQuest.Init();
        }
    }

    void SetText() {
        questContainer.GetComponent<TextMeshProUGUI>().text = "Quest: " +
            (currentQuest == null ? "" : currentQuest.GetRepr());
    }

    void UpdateQuest() {
        if (currentQuest == null) {
            return;
        }
        if (currentQuest.IsComplete()) {
            currentQuest = null;
            currentQuestIndex += 1;
            SetQuest();
        }
    }

    private abstract class Quest {
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

    private class CountingQuest : Quest {
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

    private class UnlockingQuest : Quest {
        public string text;
        public int start;

        public UnlockingQuest(string text) {
            this.text = text;
            this.start = 0;
        }

        public override void Init() {
            // implement me
        }

        public override bool IsComplete() {
            return false;
        }

        public override string GetRepr() {
            return string.Format("{0}", text);
        }
    }
}
