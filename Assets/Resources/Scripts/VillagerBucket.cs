using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerBucket : MonoBehaviour {
    public static VillagerBucket villagerBucket;
    private Transform villagers;
    private int villagerCount;
    private int maxVillagers = 15;
    public Dictionary<string, int> jobs;
    public List<string> availableJobs = new List<string>() {
        "chopper", "hauler"
    };

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

    void Update() {
        ResetJobs();
        foreach (Transform child in villagers.transform) {
            jobs[child.gameObject.GetComponent<Villager>().job]++;
        }
        string currentAssignments = "";
        foreach (KeyValuePair<string, int> entry in jobs) {
            currentAssignments += entry.Key + ":" + entry.Value + "; ";
        };
        Debug.Log(currentAssignments);
    }

    void ResetJobs() {
        jobs = new Dictionary<string, int>();
        foreach (string job in availableJobs) {
            jobs.Add(job, 0);
        }
    }

    void SpawnVillagers() {
        villagers = GameObject.Find("VillagerBucket").transform;
        Object toInstantiate = Resources.Load("Prefabs/villager", typeof(GameObject));
        while (villagerCount < maxVillagers) {
            villagerCount++;
            GameObject instance = Instantiate(toInstantiate, new Vector3 (Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0), Quaternion.identity) as GameObject;
            instance.GetComponent<Villager>().job = availableJobs[Random.Range(0, availableJobs.Count)];
            instance.transform.SetParent(villagers);
        }
    }

}
