using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager : MonoBehaviour {

    public static QuestManager manager;

    private List<CountingQuest> quests = new List<CountingQuest>();
    private int currentQuestIndex = 0;
    private CountingQuest currentQuest;
    private GameObject questContainer;
    private Dictionary<string, int> resources;

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
        resources = ResourceCounter.counter.counts;
        DefineQuests();
        SetQuest();
    }

    void Update() {
        questContainer.GetComponent<TextMeshProUGUI>().text = currentQuest.GetRepr();
        if (currentQuest.things[currentQuest.toCount] >= currentQuest.start + currentQuest.amount) {
            currentQuest.state = false;
            currentQuestIndex += 1;
            SetQuest();
        }
    }

    void DefineQuests() {
        quests.Add(new CountingQuest("harvest trees", resources, "wood", 2));
        quests.Add(new CountingQuest("harvest rocks", resources, "rock", 2));
    }

    void SetQuest() {
        string text = "";
        if (quests.Count - 1 >= currentQuestIndex) {
            currentQuest = quests[currentQuestIndex];
            text = currentQuest.GetRepr();
            currentQuest.start = currentQuest.things[currentQuest.toCount];
        }
        questContainer.GetComponent<TextMeshProUGUI>().text = text;
    }

    private class CountingQuest {
        public string text;
        public Dictionary<string, int> things;
        public string toCount;
        public int amount;

        public int start;
        public bool state;

        public CountingQuest(string text, Dictionary<string, int> things, string toCount, int amount) {
            this.text = text;
            this.things = things;
            this.toCount = toCount;
            this.amount = amount;
            start = things[toCount];
        }

        public string GetRepr() {
            return string.Format("{0}: {1}/{2}", text, things[toCount] - start, amount);
        }
    }
}
