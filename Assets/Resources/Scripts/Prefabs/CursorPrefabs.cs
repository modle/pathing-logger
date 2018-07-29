using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorPrefabs : MonoBehaviour {
    public static CursorPrefabs cursors;
    public Dictionary<string, GameObject> cursorSprites = new Dictionary<string, GameObject>();

    void Awake() {
        // singleton pattern
        if (cursors == null) {
            DontDestroyOnLoad(gameObject);
            cursors = this;
        } else if (cursors != this) {
            Destroy(gameObject);
        }
        LoadPrefabs();
    }

    void LoadPrefabs() {
        Dictionary<string, Object> instantiables = new Dictionary<string, Object>();
        instantiables.Add("tree", Resources.Load("Prefabs/tree-orange-cursor", typeof(GameObject)));
        instantiables.Add("rock", Resources.Load("Prefabs/rock-cursor", typeof(GameObject)));
        instantiables.Add("sawyer", Resources.Load("Prefabs/sawyer-cursor", typeof(GameObject)));

        cursorSprites = PrefabUtils.utils.Load(instantiables);
        cursorSprites.Add("stop", null);
    }
}
