using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuToggler : MonoBehaviour {

    public Dictionary<string, string> hotKeys = new Dictionary<string, string>();
    public Dictionary<string, GameObject> toggleables = new Dictionary<string, GameObject>();
    GameObject techTree;
    GameObject messageLog;

    void Start() {
        SetHotKeys();
        SetObjects();
    }

    void SetHotKeys() {
        hotKeys.Add("i", "techTree");
        hotKeys.Add("l", "messageLog");
    }

    void SetObjects() {
        techTree = GameObject.Find("TechTree");
        techTree.SetActive(false);
        toggleables["techTree"] = techTree;
        messageLog = GameObject.Find("MessageLog");
        messageLog.SetActive(false);
        toggleables["messageLog"] = messageLog;
    }

	void Update() {
        CheckInput();
	}

    void CheckInput() {
        foreach (string key in hotKeys.Keys) {
            if (Input.GetKeyDown(key)) {
                GameObject obj = toggleables[hotKeys[key]];
                obj.SetActive(!obj.activeSelf);
                return;
            }
        }
    }
}
