using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DetailsDisplay : EventTrigger {

    string type;
    int count = 0;
    bool shown;
    GameObject display;

    void Awake() {
        type = GetComponent<Properties>().type;
    }

    public void OnMouseDown() {
        if (shown) {
            Destroy(display);
            shown = false;
            return;
        }
        display = Instantiate(DetailsPrefabs.details.objects[type],Input.mousePosition, Quaternion.identity) as GameObject;
        display.transform.SetParent(GameObject.Find("Canvas").transform);
        shown = true;
    }
}
