using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {

    public static ResourceManager manager;

    GameObject borderTree;
    public GameObject logs;
    Vector3 hitDown;
    Vector3 hitUp;
    Vector3 downMousePos;
    float selectionAngle = 0f;
    Vector2 selectionDirection = new Vector2(0, 0);
    public GUISkin skin;

    void Awake() {
        // singleton pattern
        if (manager == null) {
            DontDestroyOnLoad(gameObject);
            manager = this;
        } else if (manager != this) {
            Destroy(gameObject);
        }
    }

    void Start() {
        Object borderTreePrefab = Resources.Load("Prefabs/tree-orange-highlighted", typeof(GameObject));
        borderTree = Instantiate(borderTreePrefab, new Vector2(-10000, -10000), Quaternion.identity) as GameObject;
        borderTree.SetActive(false);
        Object logPrefab = Resources.Load("Prefabs/logs", typeof(GameObject));
        logs = Instantiate(logPrefab, new Vector2(-10000, -10000), Quaternion.identity) as GameObject;
        logs.SetActive(false);
    }

	void Update() {
        if (Input.GetMouseButtonDown(0)) {
            ProcessMouseDown();
        }
        if (Input.GetMouseButtonUp(0)) {
            ProcessMouseUp();
        }
	}

    void ProcessMouseDown() {
        downMousePos = Input.mousePosition;
        hitDown = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void ProcessMouseUp() {
        hitUp = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 center = (hitDown + hitUp) * 0.5f;
        Vector2 size = new Vector2(Mathf.Abs(hitUp.x - hitDown.x), Mathf.Abs(hitDown.y - hitUp.y));
        RaycastHit2D[] check = Physics2D.BoxCastAll(center, size, selectionAngle, selectionDirection);
        AddSelectedObjectsToQueue(check);
    }

    void AddSelectedObjectsToQueue(RaycastHit2D[] objects) {
        foreach (RaycastHit2D hit in objects) {
            if (hit.collider != null && hit.collider.tag == "task") {
                hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite = borderTree.GetComponent<SpriteRenderer>().sprite;
                TreeBucket.treeBucket.toChop.Add(hit.collider.gameObject);
            }
        }
    }

    void OnGUI() {
        if (Input.GetMouseButton(0)) {
            Vector3 currentPos = Input.mousePosition;
            Rect boxRect = new Rect(downMousePos.x, Screen.height - downMousePos.y, currentPos.x - downMousePos.x, downMousePos.y - currentPos.y);
            GUI.Box(boxRect, "", skin.box);
        }
    }
}
