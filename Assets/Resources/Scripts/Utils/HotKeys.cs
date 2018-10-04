using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HotKeys : MonoBehaviour {

    public static HotKeys hotKeys;
    public Dictionary<string, string> keys = new Dictionary<string, string>();
    private string help = "press:\n" +
        "    h for help\n" +
        "    t to toggle tech tree\n" +
        "    l to toggle message log\n" +
        "    q to toggle quest log\n" +
        "    1 to select tree harvester\n" +
        "    2 to select rock harvester\n" +
        "    0 to select storage builder";

    void Awake() {
        // singleton pattern
        if (hotKeys == null) {
            DontDestroyOnLoad(gameObject);
            hotKeys = this;
        } else if (hotKeys != this) {
            Destroy(gameObject);
        }
        SetHotKeys();
    }

    void Start() {
        MessageLog.log.Publish(help);
    }

    void Update() {
        CheckInput();
    }

    void SetHotKeys() {
        keys.Add("h", "ShowHelp");
    }

    void CheckInput() {
        foreach (string key in keys.Keys) {
            if (Input.GetKeyDown(key)) {
                ExecuteCommand(keys[key]);
                return;
            }
        }
    }

    void ExecuteCommand(string command) {
        if (command == "ShowHelp") {
            MessageLog.log.Publish(help);
        }
    }

}
