using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

    Properties props;
    public string name;
    Vector3 productionOffset = new Vector3(0f, 0f, 0f);
    Vector3 baseOffset = new Vector3(-0.5f, -0.5f, 0f);
    int numProduced;
    Dictionary<string, int> consumes;
    Dictionary<string, int> rawStock;
    bool producing;
    public bool built;

    public void Start() {
        props = GetComponent<Properties>();
    }

    public void Produce() {
        producing = false;
        if (!built) {
            built = true;
            SetConsumes(BuildingManager.manager.productionCost[name]);
            ChangeSprite();
            producing = false;
            return;
        }
        productionOffset.x = (numProduced % 10) * 0.1f;
        Vector3 position = transform.position;
        TargetBucket.bucket.InstantiateResource(
            position + baseOffset + productionOffset, ResourcePrefabs.resources.gatherableResourceSprites[props.produces]
        );
        numProduced++;
        InitializeStock();
    }

    public void SetConsumes(Dictionary<string, int> materials) {
        consumes = materials;
        InitializeStock();
    }

    public void SetName(string _name) {
        name = _name;
    }

    void InitializeStock() {
        rawStock = new Dictionary<string, int>();
        foreach (KeyValuePair<string, int> entry in consumes) {
            rawStock.Add(entry.Key, 0);
        }
    }

    public bool ReadyToProduce() {
        if (NextStockToGet() != "") {
            return false;
        }
        producing = true;
        return true;
    }

    public bool Producing() {
        return producing;
    }

    public string NextStockToGet() {
        foreach (KeyValuePair<string, int> entry in rawStock) {
            if (consumes[entry.Key] > entry.Value) {
                return entry.Key;
            }
        }
        return "";
    }

    public void AddStock(string material) {
        rawStock[material]++;
    }

    void ChangeSprite() {
        GetComponent<SpriteRenderer>().sprite = BuildingPrefabs.buildings.templateSprites[name].GetComponent<SpriteRenderer>().sprite;
        props.job = name;
    }

    public string GetRepr() {
        string materialsNeeded = "";
        foreach (KeyValuePair<string, int> entry in consumes) {
            materialsNeeded += entry.Key + ": " + entry.Value;
        }

        string baseString = CapitalizeFirstLetter(name);

        if (!built) {
            return baseString + "\nConstructing...";
        } else {
            return baseString + "\nActive" +
                "\nconsumes: " + materialsNeeded +
                "\nproduces: " + props.produces +
                "\nassignee: " + (props.targetedBy > 0 ? props.targetedBy.ToString() : "nobody");
        }
    }

    string CapitalizeFirstLetter(string s) {
        char[] a = s.ToCharArray();
        a[0] = char.ToUpper(a[0]);
        return new string(a);
    }
}
