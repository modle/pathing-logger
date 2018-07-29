using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Selector : EventTrigger {

    string type;

    public override void OnPointerClick(PointerEventData eventData) {
        // somehow indicate that this is placeable and not selectable
        if (name != "stop") {
            type = name;
        }
        TargetManager.manager.SetTarget(name);
    }
}
