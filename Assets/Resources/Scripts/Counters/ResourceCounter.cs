using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceCounter : MonoBehaviour {
    public static ResourceCounter counter;
    public Dictionary<string, int> counts;
    public Dictionary<string, Text> counters;
    private int startingCount = 10;

    public List<string> resources;

    void Awake() {
        // singleton pattern
        if (counter == null) {
            counter = this;
        } else if (counter != this) {
            Destroy(gameObject);
        }

        counts = new Dictionary<string, int>();
        counters = new Dictionary<string, Text>();
        resources = new List<string>() {"wood", "rock", "plank"};

        foreach (Transform child in transform) {
            if (resources.Contains(child.gameObject.name)) {
                counters.Add(child.gameObject.name, child.Find("count").GetComponent<Text>());
                counts.Add(child.gameObject.name, startingCount);
            }
        }
    }

	void Update () {
        foreach (KeyValuePair<string, Text> entry in counters) {
            entry.Value.text = counts[entry.Key].ToString();
        }
	}
}
