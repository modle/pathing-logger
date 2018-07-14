using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selector : EventTrigger {

    public override void OnPointerClick(PointerEventData eventData) {
        string type = "";
        if (name != "stop") {
            type = name;
        }
        ResourceManager.manager.targetType = type;
        print("clicked thing: " + name + "; targetType is: " + ResourceManager.manager.targetType);
    }
}
