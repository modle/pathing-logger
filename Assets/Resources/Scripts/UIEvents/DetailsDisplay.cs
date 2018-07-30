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
        Object uninstantiated = DetailsPrefabs.details.objects[type];
        Vector3 position = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        display = Instantiate(uninstantiated, position, Quaternion.identity) as GameObject;
        display.transform.SetParent(GameObject.Find("Canvas").transform);
        display.SetActive(false);
    }

    public void OnMouseDown() {
        if (CursorManager.manager.transform.gameObject.activeSelf) {
            return;
        }
        shown = true;
        display.SetActive(shown);
        display.GetComponent<DisplayUpdater>().building = transform.gameObject.GetComponent<Building>();
        display.GetComponent<DisplayUpdater>().villager = transform.gameObject.GetComponent<Villager>();
    }

    public void DeactivateDisplay() {
        display.SetActive(false);
    }
}
