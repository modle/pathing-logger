using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPrefabs : MonoBehaviour {

    public static BuildingPrefabs buildings;
    public Dictionary<string, Object> buildingSprites;

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
        buildingSprites = new Dictionary<string, Object>();
        buildingSprites.Add("woodcutter", Resources.Load("Prefabs/woodcutter-building", typeof(GameObject)));
    }
}
