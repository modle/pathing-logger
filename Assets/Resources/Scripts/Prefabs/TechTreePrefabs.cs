using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechTreePrefabs : MonoBehaviour {
    public static TechTreePrefabs techTreeIcons;
    public Dictionary<string, GameObject> techTreeSprites = new Dictionary<string, GameObject>();

    void Awake() {
        // singleton pattern
        if (techTreeIcons == null) {
            DontDestroyOnLoad(gameObject);
            techTreeIcons = this;
        } else if (techTreeIcons != this) {
            Destroy(gameObject);
        }
    }

    void Start() {
        LoadPrefabs();
    }

    void LoadPrefabs() {
        Dictionary<string, Object> instantiables = new Dictionary<string, Object>();
        instantiables.Add("sawyer", Resources.Load("Prefabs/sawyer-tech-tree-checked", typeof(GameObject)));
        techTreeSprites = PrefabUtils.utils.Load(instantiables);
    }
}
