using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DisplayUpdater : EventTrigger {

    public Transform target;
    private bool dragging;

    public void Update() {
        // TODO: this is ugly. Generics?
        if (target.GetComponent<Building>() != null) {
            transform.Find("text").GetComponent<TextMeshProUGUI>().text = target.GetComponent<Building>().GetRepr();
        } else if (target.GetComponent<CollisionState>() != null) {
            transform.Find("text").GetComponent<TextMeshProUGUI>().text = target.GetComponent<CollisionState>().GetRepr();
        }
        if (dragging) {
            Drag();
        }
    }

    public override void OnPointerDown(PointerEventData eventData) {
        dragging = true;
    }

    public override void OnPointerUp(PointerEventData eventData) {
        dragging = false;
    }

    private void Drag() {
        Vector3 touchPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        transform.position = touchPosition;
    }
}
