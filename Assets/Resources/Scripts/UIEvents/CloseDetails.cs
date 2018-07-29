using UnityEngine;
using UnityEngine.EventSystems;

public class CloseDetails : EventTrigger {

    public override void OnPointerClick(PointerEventData eventData) {
        transform.parent.transform.gameObject.SetActive(false);
    }
}
