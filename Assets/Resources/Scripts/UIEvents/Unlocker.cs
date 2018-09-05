using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Unlocker : EventTrigger {

    bool unlocked = false;
    GameObject tooltip;

    void Awake() {
        tooltip = transform.Find("tooltip").gameObject;
        tooltip.SetActive(false);
    }

    public override void OnPointerClick(PointerEventData eventData) {
        if (unlocked) {
            return;
        }
        BuildingManager.manager.EnableBuilding(name);
        TechTreeManager.manager.MarkEntryComplete(name);
        unlocked = true;
    }

    public override void OnPointerEnter(PointerEventData eventData) {
        float xPos = transform.position.x + GetComponent<RectTransform>().rect.width * 2;
        float yPos = transform.position.y; // - tooltip.GetComponent<RectTransform>().rect.height / 2;
        Vector3 position = new Vector3(xPos, yPos, 0);
        tooltip.transform.position = position;
        tooltip.SetActive(true);
    }

    public override void OnPointerExit(PointerEventData eventData) {
        tooltip.SetActive(false);
    }
}
