using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

    Properties props;
    string name;
    Vector3 productionOffset = new Vector3(0f, 0f, 0f);
    Vector3 baseOffset = new Vector3(-0.5f, -0.5f, 0f);
    int numProduced;
    Dictionary<string, int> consumes;
    Dictionary<string, int> rawStock;
    bool producing;
    float startTime;

    public void Start() {
        props = GetComponent<Properties>();
        Invoke("ChangeSprite", 2);
    }

    public void Produce() {
        productionOffset.x = (numProduced % 10) * 0.1f;
        Vector3 position = transform.position;
        TargetBucket.bucket.InstantiateResource(
            position + baseOffset + productionOffset, ResourcePrefabs.resources.gatherableResourceSprites[props.produces]
        );
        numProduced++;
        producing = false;
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
        print ("Producing?: " + producing);
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
}
