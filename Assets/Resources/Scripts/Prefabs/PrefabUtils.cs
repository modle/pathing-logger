using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabUtils : MonoBehaviour {
    public static PrefabUtils utils;

    void Awake() {
        // singleton pattern
        if (utils == null) {
            DontDestroyOnLoad(gameObject);
            utils = this;
        } else if (utils != this) {
            Destroy(gameObject);
        }
    }

    public Dictionary<string, GameObject> Load(Dictionary<string, Object> targetPrefabs) {
        Dictionary<string, GameObject> prefabBucket = new Dictionary<string, GameObject>();
        foreach (KeyValuePair<string, Object> entry in targetPrefabs) {
            GameObject theObject = Instantiate(entry.Value, new Vector2(-10000, -10000), Quaternion.identity) as GameObject;
            theObject.SetActive(false);
            prefabBucket.Add(entry.Key, theObject);
        }
        return prefabBucket;
    }
}
