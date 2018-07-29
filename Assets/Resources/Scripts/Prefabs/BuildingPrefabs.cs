using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPrefabs : MonoBehaviour {

    public static BuildingPrefabs buildings;
    public Dictionary<string, Object> buildingSprites;
    public Dictionary<string, Object> inProgressSprites;
    public Dictionary<string, GameObject> templateSprites = new Dictionary<string, GameObject>();

    void Awake() {
        // singleton pattern
        if (buildings == null) {
            DontDestroyOnLoad(gameObject);
            buildings = this;
        } else if (buildings != this) {
            Destroy(gameObject);
        }
        LoadPrefabs();
    }

    void LoadPrefabs() {
        inProgressSprites = new Dictionary<string, Object>();
        inProgressSprites.Add("sawyer", Resources.Load("Prefabs/sawyer-building-incomplete", typeof(GameObject)));

        buildingSprites = new Dictionary<string, Object>();
        buildingSprites.Add("sawyer", Resources.Load("Prefabs/sawyer-building", typeof(GameObject)));

        foreach (KeyValuePair<string, Object> entry in buildingSprites) {
            GameObject theObject = Instantiate(entry.Value, new Vector2(-10000, -10000), Quaternion.identity) as GameObject;
            theObject.SetActive(false);
            templateSprites.Add(entry.Key, theObject);
        }
    }
}
