using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotKeyWatcher : MonoBehaviour {

    public Dictionary<string, string> hotKeys = new Dictionary<string, string>();
    public Dictionary<string, GameObject> toggleables = new Dictionary<string, GameObject>();
    GameObject techTree;

    void Start() {
        SetHotKeys();
        SetObjects();
    }

    void SetHotKeys() {
        hotKeys.Add("i", "techTree");
    }

    void SetObjects() {
        techTree = GameObject.Find("TechTree");
        techTree.SetActive(false);
        toggleables["techTree"] = techTree;
    }

	void Update() {
        CheckInput();
	}

    void CheckInput() {
        foreach (string key in hotKeys.Keys) {
            if (Input.GetKeyDown(key)) {
                toggleables[hotKeys[key]].SetActive(!techTree.activeSelf);
                return;
            }
        }
    }
}
