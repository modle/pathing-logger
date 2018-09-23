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
        quests.Add(new UnlockingQuest("unlock the Sawyer", "sawyer", BuildingManager.manager.buildingSelectors));
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
}
