using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuToggler : MonoBehaviour {

    public Dictionary<string, string> hotKeys = new Dictionary<string, string>();
    public Dictionary<string, GameObject> toggleables = new Dictionary<string, GameObject>();
    GameObject techTree;
    GameObject messageLog;

    // child object structure might make more sense here
    // each child object could be given a ScriptableObject containing a single property: hotkey

    private List<string> menus = new List<string>() {"TechTree", "MessageLog"};

    void Start() {
        SetHotKeys();
        SetObjects();
        DisableObjects();
    }

    void SetHotKeys() {
        hotKeys.Add("i", "TechTree");
        hotKeys.Add("l", "MessageLog");
    }

    void SetObjects() {
        foreach (string menu in menus) {
            toggleables[menu] = GameObject.Find(menu);
        }
    }

    void DisableObjects() {
        foreach (KeyValuePair<string, GameObject> entry in toggleables) {
            entry.Value.SetActive(false);
        }
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
