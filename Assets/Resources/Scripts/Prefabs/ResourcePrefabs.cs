using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePrefabs : MonoBehaviour {

    public static ResourcePrefabs resources;
    public Dictionary<string, Object> rawResourceSprites;
    public Dictionary<string, Object> gatherableResourceSprites;

    void Awake() {
        // singleton pattern
        if (resources == null) {
            DontDestroyOnLoad(gameObject);
            resources = this;
        } else if (resources != this) {
            Destroy(gameObject);
        }
        LoadPrefabs();
    }

    void LoadPrefabs() {
        rawResourceSprites = new Dictionary<string, Object>();
        rawResourceSprites.Add("wood", Resources.Load("Prefabs/tree", typeof(GameObject)));
        rawResourceSprites.Add("rock", Resources.Load("Prefabs/rock", typeof(GameObject)));

        gatherableResourceSprites = new Dictionary<string, Object>();
        gatherableResourceSprites.Add("wood", Resources.Load("Prefabs/logs", typeof(GameObject)));
        gatherableResourceSprites.Add("rock", Resources.Load("Prefabs/rubble", typeof(GameObject)));
        gatherableResourceSprites.Add("plank", Resources.Load("Prefabs/plank", typeof(GameObject)));
    }

    public Object GetRandom(string type) {
        if (type == "raw") {
            return GetRandomFromDict(rawResourceSprites);
        } else if (type == "gatherable") {
            // we'll use this later for randomly spawned gatherable resources, maybe
            return GetRandomFromDict(gatherableResourceSprites);
        } else {
            // yep. deal with it.
            return null;
        }
    }

    private Object GetRandomFromDict(Dictionary<string, Object> dict) {
        List<string> keyList = new List<string>(dict.Keys);
        return dict[
            keyList[Random.Range(0, keyList.Count)]
        ];
    }
}
