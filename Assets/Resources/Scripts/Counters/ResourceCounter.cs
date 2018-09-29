using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceCounter : MonoBehaviour {
    // TODO Rename to ResourceManager
    public static ResourceCounter counter;
    public Dictionary<string, int> counts;
    public Dictionary<string, int> resourceGains;
    public Dictionary<string, TextMeshProUGUI> counters;
    private int startingCount = 50;

    public List<string> resources;

    void Awake() {
        // singleton pattern
        if (counter == null) {
            counter = this;
        } else if (counter != this) {
            Destroy(gameObject);
        }

        counts = new Dictionary<string, int>();
        resourceGains = new Dictionary<string, int>();
        counters = new Dictionary<string, TextMeshProUGUI>();
        resources = new List<string>() {"wood", "rock", "plank"};

        foreach (Transform child in transform) {
            if (resources.Contains(child.gameObject.name)) {
                counters.Add(child.gameObject.name, child.Find("count").GetComponent<TextMeshProUGUI>());
                counts.Add(child.gameObject.name, startingCount);
                resourceGains.Add(child.gameObject.name, 0);
            }
        }
    }

	void Update () {
        foreach (KeyValuePair<string, TextMeshProUGUI> entry in counters) {
            entry.Value.text = counts[entry.Key].ToString();
        }
	}

    public void AddToStorage(string target) {
        counts[target]++;
        resourceGains[target]++;
    }
}
