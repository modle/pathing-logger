using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour {

    public static BuildingManager manager;

    public Dictionary<string, Dictionary<string, int>> productionCost;
    public Dictionary<string, Dictionary<string, int>> buildCost;
    public Dictionary<string, GameObject> buildingSelectors = new Dictionary<string, GameObject>();

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
        productionCost = new Dictionary<string, Dictionary<string, int>>();
        productionCost.Add("sawyer", new Dictionary<string, int>() {{"wood", 1}});
        buildCost = new Dictionary<string, Dictionary<string, int>>();
        buildCost.Add("sawyer", new Dictionary<string, int>() {{"wood", 1}, {"rock", 1}});
        GetBuildingSelectors();
        HideBuildings();
    }

    private void GetBuildingSelectors() {
        Transform selectorsTransform = GameObject.Find("BuildingSelectors").transform;
        foreach (Transform t in selectorsTransform) {
            buildingSelectors.Add(t.gameObject.name, t.gameObject);
        }
    }

    private void HideBuildings() {
        foreach (string key in buildingSelectors.Keys) {
            buildingSelectors[key].SetActive(false);
        }
    }

    public void EnableBuilding(string building) {
        if (buildingSelectors.ContainsKey(building)) {
            buildingSelectors[building].SetActive(true);
        }
    }

    public void PlaceBuilding(string targetType) {
        Object targetPrefab = BuildingPrefabs.buildings.inProgressSprites[targetType];
        if (targetPrefab == null) {
            print("no prefab found for " + targetType);
        }
        if (Input.GetMouseButtonDown(0)) {
            InstantiateBuildingObject(targetType, targetPrefab);
        }
    }

    bool CanConstruct(Dictionary<string, int> materialsNeeded) {
        foreach (KeyValuePair<string, int> entry in materialsNeeded) {
            if (entry.Value > ResourceCounter.counter.counts[entry.Key]) {
                return false;
            }
        }
        return true;
    }

    string GetMaterialsRepr(Dictionary<string, int> materialsNeeded) {
        string repr = "";
        foreach (KeyValuePair<string, int> entry in materialsNeeded) {
            repr += entry.Key + ": " + entry.Value + ", ";
        }
        return repr;
    }

    void InstantiateBuildingObject(string targetType, Object targetPrefab) {
        Vector3 placementLocation = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 actualPlacement = new Vector3(placementLocation.x, placementLocation.y, 0);
        GameObject theObject = Instantiate(targetPrefab, actualPlacement, Quaternion.identity) as GameObject;
        theObject.GetComponent<Building>().SetConsumes(buildCost[targetType]);
        theObject.GetComponent<Building>().SetName(targetType);
        TargetBucket.bucket.targets.Add(theObject);
        theObject.transform.SetParent(transform);
        theObject.name = targetType;
    }
}
