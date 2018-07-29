using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour {

    public static CursorManager manager;
    private Vector3 offset = new Vector3(20.0f, 0f, 0f);
    GameObject cursor;
    public bool selectable;

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

    public bool SetCursorImage(string name) {
        cursor = CursorPrefabs.cursors.cursorSprites[name];
        if (cursor == null) {
            selectable = false;
            transform.gameObject.SetActive(false);
            return false;
        } else {
            transform.gameObject.SetActive(true);
            transform.GetComponent<Image>().sprite = cursor.GetComponent<SpriteRenderer>().sprite;
            selectable = cursor.GetComponent<SelectorID>().selectable;
            return cursor.GetComponent<SelectorID>().placeable;
        }
    }
}
