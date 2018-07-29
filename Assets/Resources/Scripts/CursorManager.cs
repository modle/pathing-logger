using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour {

    public static CursorManager manager;
    private Vector3 offset = new Vector3(20.0f, 0f, 0f);

    void Awake() {
        // singleton pattern
        if (manager == null) {
            manager = this;
        } else if (manager != this) {
            Destroy(gameObject);
        }
    }

    void Start() {
        transform.gameObject.SetActive(false);
    }

    void Update() {
        transform.position = Input.mousePosition + offset;
    }
}
