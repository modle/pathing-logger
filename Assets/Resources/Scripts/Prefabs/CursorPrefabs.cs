using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorPrefabs : MonoBehaviour {
    public static CursorPrefabs cursors;
    public Dictionary<string, GameObject> cursorSprites = new Dictionary<string, GameObject>();
    public List<string> disableTargetsOnLoad = new List<string>() {"sawyer"};

    void Awake() {
        // singleton pattern
        if (cursors == null) {
            DontDestroyOnLoad(gameObject);
            cursors = this;
        } else if (cursors != this) {
            Destroy(gameObject);
        }
    }

    void Start() {
        LoadPrefabs();
    }

    void LoadPrefabs() {
        Dictionary<string, Object> instantiables = new Dictionary<string, Object>();
        instantiables.Add("tree", Resources.Load("Prefabs/tree-orange-cursor", typeof(GameObject)));
        instantiables.Add("rock", Resources.Load("Prefabs/rock-cursor", typeof(GameObject)));
        instantiables.Add("sawyer", Resources.Load("Prefabs/sawyer-cursor", typeof(GameObject)));

        cursorSprites = PrefabUtils.utils.Load(instantiables);
        cursorSprites.Add("stop", null);

        DisableTargetPrefabs();
    }

    void DisableTargetPrefabs() {
        foreach (string target in disableTargetsOnLoad) {
            if (cursorSprites.ContainsKey(target)) {
                cursorSprites[target].SetActive(false);
            }
        }
    }

    public void EnablePrefab(string name) {
        if (cursorSprites.ContainsKey(name)) {
            cursorSprites[name].SetActive(true);
        }
    }
}
