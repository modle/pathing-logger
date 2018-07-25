using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

    TargetID id;
    Vector3 productionOffset = new Vector3(0f, 0f, 0f);
    Vector3 baseOffset = new Vector3(-0.5f, -0.5f, 0f);
    int numProduced;
    Dictionary<string, int> consumes;
    bool hasConsumed;

    public void Start() {
        id = GetComponent<TargetID>();
    }

    public void Produce() {
        productionOffset.x = (numProduced % 10) * 0.1f;
        Vector3 position = transform.position;
        TargetBucket.bucket.InstantiateResource(position + baseOffset + productionOffset, ResourcePrefabs.resources.gatherableResourceSprites[id.produces]);
        numProduced++;
        hasConsumed = false;
    }

    public void SetConsumes(Dictionary<string, int> materials) {
        consumes = materials;
    }

    public bool Consume() {
        if (!hasConsumed && CanConsume()) {
            foreach (KeyValuePair<string, int> entry in consumes) {
                // set this as a reference parameter of Building?
                ResourceCounter.counter.counts[entry.Key] -= entry.Value;
            }
            hasConsumed = true;
            return true;
        };
        return false;
    }

    private bool CanConsume() {
        foreach (KeyValuePair<string, int> entry in consumes) {
            if (ResourceCounter.counter.counts[entry.Key] < entry.Value) {
                return false;
            }
        }
        return true;
    }

    public bool CanProduce() {
        print ("hasConsumed val at CanProduce is " + hasConsumed);
        return hasConsumed;
    }
}
