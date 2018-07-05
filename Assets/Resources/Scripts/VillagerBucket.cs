using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerBucket : MonoBehaviour {
    public static VillagerBucket villagerBucket;
    private Transform villagers;
    private int villagerCount;
    private int maxVillagers = 15;

    void Awake() {
        // singleton pattern
        if (villagerBucket == null) {
            DontDestroyOnLoad(gameObject);
            villagerBucket = this;
        } else if (villagerBucket != this) {
            Destroy(gameObject);
        }

        SpawnVillagers();
    }

    void SpawnVillagers() {
        villagers = GameObject.Find("VillagerBucket").transform;
        Object toInstantiate = Resources.Load("Prefabs/villager", typeof(GameObject));
        while (villagerCount < maxVillagers) {
            villagerCount++;
            GameObject instance = Instantiate(toInstantiate, new Vector3 (Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0), Quaternion.identity) as GameObject;
            instance.transform.SetParent(villagers);
        }
    }

}
