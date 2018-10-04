using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class HelpPane : MonoBehaviour {

    public static HelpPane pane;

    private GameObject helpContainer;
    private string help = "\nhotkeys:\n" +
        "    H : help\n" +
        "    T : toggle tech tree\n" +
        "    L : toggle message log\n" +
        "    Q : toggle quest log\n" +
        "    1 : select tree harvester\n" +
        "    2 : select rock harvester\n" +
        "    0 : select storage builder";

    void Awake() {
        // singleton pattern
        if (pane == null) {
            DontDestroyOnLoad(gameObject);
            pane = this;
        } else if (pane != this) {
            Destroy(gameObject);
        }
        helpContainer = transform.Find("HelpContainer").gameObject;
    }

    void Start() {
        helpContainer.GetComponent<TextMeshProUGUI>().text = help;
    }
}
