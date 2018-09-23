using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechTreeManager : MonoBehaviour {

    public static TechTreeManager manager;
    public Dictionary<string, GameObject> techTreeSelectors = new Dictionary<string, GameObject>();

    void Awake() {
        // singleton pattern
        if (manager == null) {
            DontDestroyOnLoad(gameObject);
            manager = this;
        } else if (manager != this) {
            Destroy(gameObject);
        }
        GetTechTreeSelectors();
    }

    private void GetTechTreeSelectors() {
        Transform selectorsTransform = GameObject.Find("TechTree").transform.Find("Entries").transform;
        foreach (Transform t in selectorsTransform) {
            techTreeSelectors.Add(t.gameObject.name, t.gameObject);
        }
    }

    public void MarkEntryComplete(string entry) {
        techTreeSelectors[entry].GetComponent<Image>().sprite =
            TechTreePrefabs.techTreeIcons.techTreeSprites[entry].GetComponent<SpriteRenderer>().sprite;
    }
}
