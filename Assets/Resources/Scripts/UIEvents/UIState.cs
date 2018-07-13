using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIState : EventTrigger {

    public bool isActive;

    public override void OnPointerEnter(PointerEventData eventData) {
        isActive = true;
    }

    public override void OnPointerExit(PointerEventData eventData) {
        isActive = false;
    }
}
