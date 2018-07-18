using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Selector : EventTrigger {

    GameObject cursor;
    Transform cursorImage;

    public void Awake() {
        cursor = CursorPrefabs.cursors.cursorSprites[name];
        cursorImage = GameObject.Find("CursorImage").transform;
    }

    public override void OnPointerClick(PointerEventData eventData) {
        string type = "";
        if (name != "stop") {
            type = name;
        }
        ResourceManager.manager.targetType = type;
        print("clicked thing: " + name + "; targetType is: " + ResourceManager.manager.targetType + "; cursor is: " + cursor);

        if (cursor == null) {
            cursorImage.gameObject.SetActive(false);
        } else {
            cursorImage.gameObject.SetActive(true);
            cursorImage.GetComponent<Image>().sprite = cursor.GetComponent<SpriteRenderer>().sprite;
        }
    }
}
