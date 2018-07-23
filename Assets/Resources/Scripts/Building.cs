using UnityEngine;

public class Building : MonoBehaviour {

    TargetID id;
    Vector3 productionOffset = new Vector3(0f, 0f, 0f);
    Vector3 baseOffset = new Vector3(-0.5f, -0.5f, 0f);
    int numProduced;

    public void Start() {
        id = GetComponent<TargetID>();
    }

    public void Produce() {
        productionOffset.x = (numProduced % 10) * 0.1f;
        Vector3 position = transform.position;
        TargetBucket.bucket.InstantiateResource(position + baseOffset + productionOffset, ResourcePrefabs.resources.gatherableResourceSprites[id.produces]);
        numProduced++;
    }
}
