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
        instantiables.Add("woodcutter", Resources.Load("Prefabs/woodcutter-cursor", typeof(GameObject)));

        foreach (KeyValuePair<string, Object> entry in instantiables) {
            GameObject theObject = Instantiate(entry.Value, new Vector2(-10000, -10000), Quaternion.identity) as GameObject;
            theObject.SetActive(false);
            cursorSprites.Add(entry.Key, theObject);
        }

        cursorSprites.Add("stop", null);
    }

}
