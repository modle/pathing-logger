using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DetailsDisplay : EventTrigger {

    bool shown;
    GameObject display;

    void Awake() {
        string type = GetComponent<Properties>().type;
        display = Instantiate(DetailsPrefabs.details.objects[type],Input.mousePosition, Quaternion.identity) as GameObject;
        display.transform.SetParent(GameObject.Find("Canvas").transform);
        display.SetActive(false);
    }

    public void OnMouseDown() {
        if (CursorManager.manager.transform.gameObject.activeSelf) {
            return;
        }
        shown = !shown;
        display.SetActive(shown);
    }
}
