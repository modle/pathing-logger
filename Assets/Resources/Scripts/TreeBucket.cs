using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBucket : MonoBehaviour {
    public static TreeBucket bucket;
    private int treeCount;
    private int maxTrees = 1000;
    private HashSet<string> treePositions = new HashSet<string>();
    public HashSet<GameObject> toChop = new HashSet<GameObject>();
    public HashSet<GameObject> toHaul = new HashSet<GameObject>();
    public HashSet<GameObject> toDestroy = new HashSet<GameObject>();
    public List<GameObject> trees = new List<GameObject>();

    void Awake() {
        // singleton pattern
        if (bucket == null) {
            DontDestroyOnLoad(gameObject);
            bucket = this;
        } else if (bucket != this) {
            Destroy(gameObject);
        }
        SpawnTrees();
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

    void SpawnTrees() {
        Object toInstantiate = Resources.Load("Prefabs/tree-orange", typeof(GameObject));
        while (treeCount < maxTrees) {
            Vector3 theVector = new Vector3(
                Random.Range(-9.0f, 9.0f),
                Random.Range(-5.0f, 5.0f),
                0);
            if ((new Rect(-1.5f, -1.5f, 3.0f, 3.0f).Contains(new Vector2(theVector.x, theVector.y)))) {
                continue;
            }
            treeCount++;

            // keeps them from being placed too close together
            string positionString = Mathf.RoundToInt(theVector.x * 1.5f) + "," + Mathf.RoundToInt(theVector.y * 1.5f);
            if (!treePositions.Add(positionString)) {
                continue;
            }

            GameObject instance = Instantiate(toInstantiate, theVector, Quaternion.identity) as GameObject;

            // sort in reverse vertical order
            instance.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(theVector.y * 100f) * -1;
            instance.transform.SetParent(transform);
        }

        foreach (Transform child in transform) {
            trees.Add(child.gameObject);
        }
    }
}
