using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBucket : MonoBehaviour {
    public static TargetBucket bucket;
    private int resourceCount;
    private int maxResources = 100;
    private HashSet<string> resourcePositions = new HashSet<string>();
    public List<GameObject> targets = new List<GameObject>();
    private Dictionary<string, float> colliderWidths = new Dictionary<string, float>();
    private List<Object> instantiables;
    private Rect noResourceZone = new Rect(-1.5f, -1.5f, 3.0f, 3.0f);

    void Awake() {
        // singleton pattern
        if (bucket == null) {
            DontDestroyOnLoad(gameObject);
            bucket = this;
        } else if (bucket != this) {
            Destroy(gameObject);
        }
    }

    void Start() {
        LoadColliders();
        SpawnResources();
    }

    void LoadColliders() {
        colliderWidths.Add("tree", 0.5f);
        colliderWidths.Add("rock", 1.0f);
    }

    void SpawnResources() {
        while (resourceCount < maxResources) {
            Vector3 theVector = GetValidPosition();

            resourceCount++;
            if (TooCloseToOthers(theVector)) {
                continue;
            }

            InstantiateResource(theVector, ResourcePrefabs.resources.GetRandom("raw"));
        }
        print ("made " + targets.Count + " targets");
    }

    Vector3 GetValidPosition() {
        Vector3 thePosition = new Vector3(
            Random.Range(-9.0f, 9.0f),
            Random.Range(-5.0f, 5.0f),
            0
        );
        if (noResourceZone.Contains(new Vector2(thePosition.x, thePosition.y))) {
            return GetValidPosition();
        } else {
            return thePosition;
        }
    }

    bool TooCloseToOthers(Vector3 theVector) {
        string testPositionString = Mathf.RoundToInt(theVector.x * 1.5f) + "," + Mathf.RoundToInt(theVector.y * 1.5f);
        return !resourcePositions.Add(testPositionString);
    }

    public GameObject InstantiateResource(Vector3 theVector, Object theObject) {
        GameObject instance = Instantiate(theObject, theVector, Quaternion.identity) as GameObject;

        SetColliderWidth(instance, instance.GetComponent<TargetID>().type);

        // sort in reverse vertical order
        instance.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(theVector.y * 100f) * -1;
        instance.transform.SetParent(transform);
        targets.Add(instance);
        return instance;
    }

    private void SetColliderWidth(GameObject instance, string type) {
        List<string> customColliders = new List<string>(colliderWidths.Keys);
        if (!customColliders.Contains(type)) {
            return;
        }
        Vector2 instanceSize = instance.GetComponent<SpriteRenderer>().bounds.size;
        instanceSize.x *= colliderWidths[type];
        ((BoxCollider2D)instance.GetComponent<BoxCollider2D>()).size = instanceSize;
    }
}
