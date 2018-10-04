using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HotKeys : MonoBehaviour {

    public static HotKeys hotKeys;
    public Dictionary<string, string> keys = new Dictionary<string, string>();

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

    // void Update() {
    //     CheckInput();
    // }

    void SetHotKeys() {
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
    }

}
