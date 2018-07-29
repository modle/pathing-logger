using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailsPrefabs : MonoBehaviour {

    public static DetailsPrefabs details;
    public Dictionary<string, Object> objects = new Dictionary<string, Object>();

    void Awake() {
        // singleton pattern
        if (details == null) {
            DontDestroyOnLoad(gameObject);
            details = this;
        } else if (details != this) {
            Destroy(gameObject);
        }
        LoadPrefabs();
    }

    void LoadPrefabs() {
        objects = new Dictionary<string, Object>();
        objects.Add("building", Resources.Load("Prefabs/details-panel", typeof(GameObject)));
    }
}
