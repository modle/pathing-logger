using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager : MonoBehaviour {

    public static QuestManager manager;

    private List<string> quests = new List<string>(){"harvest a tree", "harvest a rock"};
    private int currentQuestIndex = 0;
    private GameObject questContainer;

    void Awake() {
        // singleton pattern
        if (manager == null) {
            manager = this;
        } else if (manager != this) {
            Destroy(gameObject);
        }
        questContainer = transform.Find("QuestContainer").gameObject;
        SetQuest();
    }

    void SetQuest() {
        questContainer.GetComponent<TextMeshProUGUI>().text = quests[currentQuestIndex];
    }
}
