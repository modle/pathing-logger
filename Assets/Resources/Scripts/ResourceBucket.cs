using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBucket : MonoBehaviour {
    public static ResourceBucket bucket;
    private int resourceCount;
    private int maxResources = 1000;
    private HashSet<string> resourcePositions = new HashSet<string>();
    public HashSet<GameObject> toChop = new HashSet<GameObject>();
    public HashSet<GameObject> toHaul = new HashSet<GameObject>();
    public HashSet<GameObject> toDestroy = new HashSet<GameObject>();
    public List<GameObject> resources = new List<GameObject>();
    private Dictionary<string, float> colliderWidths = new Dictionary<string, float>();

    void Awake() {
        // singleton pattern
        if (bucket == null) {
            DontDestroyOnLoad(gameObject);
            bucket = this;
        } else if (bucket != this) {
            Destroy(gameObject);
        }
        colliderWidths.Add("tree", 0.5f);
        colliderWidths.Add("rock", 1.0f);
        SpawnResources();
    }

    void Update() {
        // Debug.Log(
        //     "toChop length:" + toChop.Count +
        //     "; toHaul length:" + toHaul.Count +
        //     "; toDestroy length:" + toDestroy.Count +
        //     "; trees length:" + GameObject.FindGameObjectsWithTag("task").Length
        // );
        foreach (GameObject go in toDestroy) {
            Destroy(go.gameObject);
        }
        if (toDestroy.Count > 50) {
            toDestroy = new HashSet<GameObject>();
        }
    }

    void SpawnResources() {
        Object tree = Resources.Load("Prefabs/tree-orange", typeof(GameObject));
        Object rock = Resources.Load("Prefabs/rock", typeof(GameObject));
        List<Object> instantiables = new List<Object>() {tree, rock};
        while (resourceCount < maxResources) {
            Vector3 theVector = new Vector3(
                Random.Range(-9.0f, 9.0f),
                Random.Range(-5.0f, 5.0f),
                0);
            if ((new Rect(-1.5f, -1.5f, 3.0f, 3.0f).Contains(new Vector2(theVector.x, theVector.y)))) {
                continue;
            }
            resourceCount++;

            // keeps them from being placed too close together
            string positionString = Mathf.RoundToInt(theVector.x * 1.5f) + "," + Mathf.RoundToInt(theVector.y * 1.5f);
            if (!resourcePositions.Add(positionString)) {
                continue;
            }

            GameObject instance = Instantiate(instantiables[Random.Range(0, instantiables.Count)], theVector, Quaternion.identity) as GameObject;
            Vector2 instanceSize = instance.GetComponent<SpriteRenderer>().bounds.size;
            instanceSize.x *= colliderWidths[instance.GetComponent<Identifier>().type];
            ((BoxCollider2D)instance.GetComponent<BoxCollider2D>()).size = instanceSize;

            // sort in reverse vertical order
            instance.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(theVector.y * 100f) * -1;
            instance.transform.SetParent(transform);
        }

        foreach (Transform child in transform) {
            resources.Add(child.gameObject);
        }
    }
}
