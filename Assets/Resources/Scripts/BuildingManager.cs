using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour {

    public static BuildingManager manager;

    public Dictionary<string, Dictionary<string, int>> buildings;

    void Awake() {
        // singleton pattern
        if (manager == null) {
            DontDestroyOnLoad(gameObject);
            manager = this;
        } else if (manager != this) {
            Destroy(gameObject);
        }
        GenerateBuildings();
    }

    private void GenerateBuildings() {
        buildings = new Dictionary<string, Dictionary<string, int>>();
        buildings.Add("sawyer", new Dictionary<string, int>() {{"wood", 1}});
    }

    public void PlaceBuilding(string targetType) {
        Object targetPrefab = BuildingPrefabs.buildings.buildingSprites[targetType];
        if (targetPrefab == null) {
            print("no prefab found for " + targetType);
        }
        if (Input.GetMouseButtonDown(0)) {
            Vector3 placementLocation = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 actualPlacement = new Vector3(placementLocation.x, placementLocation.y, 0);
            GameObject theObject = Instantiate(targetPrefab, actualPlacement, Quaternion.identity) as GameObject;
            theObject.GetComponent<Building>().SetConsumes(buildings[targetType]);
            TargetBucket.bucket.targets.Add(theObject);
            theObject.transform.SetParent(transform);
        }
    }
}
