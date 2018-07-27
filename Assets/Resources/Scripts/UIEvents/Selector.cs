using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Selector : EventTrigger {

    GameObject cursor;
    Transform cursorImage;
    string type;

    public void Awake() {
        cursorImage = GameObject.Find("CursorImage").transform;
    }

    public void Start() {
        cursor = CursorPrefabs.cursors.cursorSprites[name];
    }

    public override void OnPointerClick(PointerEventData eventData) {
        // somehow indicate that this is placeable and not selectable
        if (name != "stop") {
            type = name;
        }
        TargetManager.manager.targetType = type;

        if (cursor == null) {
            cursorImage.gameObject.SetActive(false);
            TargetManager.manager.placeable = false;
        } else {
            cursorImage.gameObject.SetActive(true);
            cursorImage.GetComponent<Image>().sprite = cursor.GetComponent<SpriteRenderer>().sprite;
            TargetManager.manager.placeable = cursor.GetComponent<SelectorID>().placeable;
        }
    }
}
