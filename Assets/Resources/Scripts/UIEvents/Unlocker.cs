using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Unlocker : EventTrigger {

    bool unlocked = false;

    public override void OnPointerClick(PointerEventData eventData) {
        if (unlocked) {
            return;
        }
        BuildingManager.manager.EnableBuilding(name);
        TechTreeManager.manager.MarkEntryComplete(name);
        unlocked = true;
    }
}
