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
        if (!CanUnlock()) {
            return;
        }
        BuildingManager.manager.EnableBuilding(name);
        AssignmentCounter.counter.EnableJob(name);
        CursorPrefabs.cursors.EnablePrefab(name);
        TechTreeManager.manager.MarkEntryComplete(name);
        unlocked = true;
    }

    private bool CanUnlock() {
        // TODO use methods in ResourceCounter instead
        bool canConsume = true;
        foreach (ResearchCosting.Material material in GetComponent<ResearchCosting>().researchMaterials) {
            if (!ResourceCounter.counter.counts.ContainsKey(material.name) || ResourceCounter.counter.counts[material.name] < material.amount) {
                canConsume = false;
                break;
            }
        }
        if (canConsume) {
            foreach (ResearchCosting.Material material in GetComponent<ResearchCosting>().researchMaterials) {
                ResourceCounter.counter.counts[material.name] -= material.amount;
            }
        }
        return canConsume;
    }

    public override void OnPointerEnter(PointerEventData eventData) {
        float xPos = transform.position.x + GetComponent<RectTransform>().rect.width * 2;
        float yPos = transform.position.y;
        Vector3 position = new Vector3(xPos, yPos, 0);
        tooltip.transform.position = position;
        tooltip.SetActive(true);
    }

    public override void OnPointerExit(PointerEventData eventData) {
        tooltip.SetActive(false);
    }
}
