using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour {

    public static BuildingManager manager;

    public Dictionary<string, Dictionary<string, int>> productionCost;
    public Dictionary<string, Dictionary<string, int>> buildCost;

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
        buildCost.Add("sawyer", new Dictionary<string, int>() {{"wood", 5}, {"rock", 5}});
    }

    public void PlaceBuilding(string targetType) {
        Object targetPrefab = BuildingPrefabs.buildings.buildingSprites[targetType];
        if (targetPrefab == null) {
            print("no prefab found for " + targetType);
        }
        if (Input.GetMouseButtonDown(0)) {
            Dictionary<string, int> materialsNeeded = buildCost[targetType];
            if (!CanConstruct(materialsNeeded)) {
                print ("not enough materials, need " + GetMaterialsRepr(materialsNeeded));
                return;
            }
            ConsumeMaterials(materialsNeeded);
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

    void ConsumeMaterials(Dictionary<string, int> materialsNeeded) {
        foreach (KeyValuePair<string, int> entry in materialsNeeded) {
            ResourceCounter.counter.counts[entry.Key] -= entry.Value;
        }
    }

    void InstantiateBuildingObject(string targetType, Object targetPrefab) {
        Vector3 placementLocation = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 actualPlacement = new Vector3(placementLocation.x, placementLocation.y, 0);
        GameObject theObject = Instantiate(targetPrefab, actualPlacement, Quaternion.identity) as GameObject;
        theObject.GetComponent<Building>().SetConsumes(productionCost[targetType]);
        TargetBucket.bucket.targets.Add(theObject);
        theObject.transform.SetParent(transform);
    }
}
