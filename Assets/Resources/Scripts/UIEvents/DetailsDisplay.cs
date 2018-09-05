using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DetailsDisplay : EventTrigger {

    GameObject display;

    void Awake() {
        string type = GetComponent<Properties>().type;
        Object uninstantiated = DetailsPrefabs.details.objects[type];

        Vector3 position = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        display = Instantiate(uninstantiated, position, Quaternion.identity) as GameObject;
        display.transform.SetParent(GameObject.Find("DetailsPanels").transform);
        display.SetActive(false);
    }

    public void OnMouseDown() {
        if (CursorManager.manager.transform.gameObject.activeSelf) {
            return;
        }
        float xPos = Input.mousePosition.x;
        float yPos = Input.mousePosition.y - display.GetComponent<RectTransform>().rect.height / 2;
        Vector3 position = new Vector3(xPos, yPos, 0);
        display.transform.position = position;
        display.SetActive(true);
        display.GetComponent<DisplayUpdater>().target = transform;
    }

    public void DeactivateDisplay() {
        display.SetActive(false);
    }
}
