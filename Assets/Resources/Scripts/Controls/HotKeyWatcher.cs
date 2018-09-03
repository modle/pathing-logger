using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotKeyWatcher : MonoBehaviour {

    public Dictionary<string, string> hotKeys = new Dictionary<string, string>();

    void Start() {
        SetHotKeys();
    }

    void SetHotKeys() {
        hotKeys.Add("i", "techTree");
    }

	void Update() {
        CheckInput();
	}

    void CheckInput() {
        foreach (string key in hotKeys.Keys) {
            if (Input.GetKeyDown(key)) {
                string target = hotKeys[key];
                print ("pressed " + target);
                return;
            }
        }
    }
}
