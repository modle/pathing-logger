using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBucket : MonoBehaviour {
    public static TreeBucket treeBucket;
    private Transform trees;
    private int treeCount;
    private int maxTrees = 1000;

    void Awake() {
        // singleton pattern
        if (treeBucket == null) {
            DontDestroyOnLoad(gameObject);
            treeBucket = this;
        } else if (treeBucket != this) {
            Destroy(gameObject);
        }

        SpawnTrees();
    }

    void SpawnTrees() {
        trees = GameObject.Find("TreeBucket").transform;
        Object toInstantiate = Resources.Load("Prefabs/tree-orange", typeof(GameObject));
        while (treeCount < maxTrees) {
            treeCount++;
            GameObject instance = Instantiate(toInstantiate, new Vector3 (Random.Range(-9.0f, 9.0f), Random.Range(-5.0f, 5.0f), 0), Quaternion.identity) as GameObject;
            instance.transform.SetParent(trees);
        }
    }

}
