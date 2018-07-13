using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public bool isActive;

    public void OnPointerEnter(PointerEventData eventData) {
        isActive = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        isActive = false;
    }
}
