using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VillagerBucket : MonoBehaviour {
    public static VillagerBucket bucket;
    public Transform villagers;
    private int villagerCount;
    private int maxVillagers = 20;

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
        SpawnVillagers();
    }

    void SpawnVillagers() {
        villagers = GameObject.Find("VillagerBucket").transform;
        Object toInstantiateSprite = Resources.Load("Prefabs/villager", typeof(GameObject));
        Object toInstantiateLabel = Resources.Load("Prefabs/villager-label", typeof(GameObject));
        Vector3 labelOffset = new Vector3(1.15f, -1.0f, 0);
        while (villagerCount < maxVillagers) {
            villagerCount++;
            Vector3 spritePosition = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
            Vector3 labelPosition = spritePosition + labelOffset;
            GameObject sprite = Instantiate(toInstantiateSprite, spritePosition, Quaternion.identity) as GameObject;
            GameObject label = Instantiate(toInstantiateLabel, labelPosition, Quaternion.identity) as GameObject;
            label.transform.SetParent(sprite.transform);
            label.GetComponent<TextMeshPro>().text = villagerCount.ToString();
            sprite.GetComponent<Properties>().id = villagerCount;
            sprite.GetComponent<Properties>().SetJob(AssignmentCounter.counter.availableJobs[Random.Range(0, AssignmentCounter.counter.availableJobs.Count)]);
            AssignmentCounter.counter.jobs[sprite.GetComponent<Properties>().job]++;
            sprite.transform.SetParent(villagers);
        }
    }

    public void ReassignVillager(string from, string to) {
        foreach(Transform villager in villagers) {
            if (villager.gameObject.GetComponent<Properties>().baseJob == from) {
                villager.gameObject.GetComponent<Job>().ChangeJob(to);
                return;
            }
        }
    }

}
