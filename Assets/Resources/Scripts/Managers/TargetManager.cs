using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour {

    public static TargetManager manager;

    public Vector3 hitDown;
    Vector3 hitUp;
    public Vector3 downMousePos;
    float selectionAngle = 0f;
    Vector2 selectionDirection = new Vector2(0, 0);
    public GUISkin skin;
    private bool select;
    public string targetType;
    Dictionary<string, GameObject> selectedSprites = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> harvestedSprites = new Dictionary<string, GameObject>();
    public bool placeable;
    public bool targetMode;
    public Dictionary<string, string> hotKeys = new Dictionary<string, string>();

    void Awake() {
        // singleton pattern
        if (manager == null) {
            DontDestroyOnLoad(gameObject);
            manager = this;
        } else if (manager != this) {
            Destroy(gameObject);
        }
        SetHotKeys();
    }

    void Start() {

        // use PrefabUtils here
        Object borderTreePrefab = Resources.Load("Prefabs/tree-highlighted", typeof(GameObject));
        GameObject theTree = Instantiate(borderTreePrefab, new Vector2(-10000, -10000), Quaternion.identity) as GameObject;
        theTree.SetActive(false);
        selectedSprites.Add("tree", theTree);

        Object highlightedRocksPrefab = Resources.Load("Prefabs/rock-highlighted", typeof(GameObject));
        GameObject theRock = Instantiate(highlightedRocksPrefab, new Vector2(-10000, -10000), Quaternion.identity) as GameObject;
        theRock.SetActive(false);
        selectedSprites.Add("rock", theRock);

        Object logPrefab = Resources.Load("Prefabs/logs", typeof(GameObject));
        GameObject theLogs = Instantiate(logPrefab, new Vector2(-10000, -10000), Quaternion.identity) as GameObject;
        theLogs.SetActive(false);
        harvestedSprites.Add("tree", theLogs);

        Object rubblePrefab = Resources.Load("Prefabs/rubble", typeof(GameObject));
        GameObject theRubble = Instantiate(rubblePrefab, new Vector2(-10000, -10000), Quaternion.identity) as GameObject;
        theRubble.SetActive(false);
        harvestedSprites.Add("rock", theRubble);
    }

    void SetHotKeys() {
        hotKeys.Add("1", "tree");
        hotKeys.Add("2", "rock");
        hotKeys.Add("9", "sawyer");
        hotKeys.Add("0", "storage");
    }

	void Update() {
        CheckInput();
        select = false;
        if (UIActive()) {
            return;
        }
        if (placeable) {
            BuildingManager.manager.PlaceBuilding(targetType);
            return;
        }
        if (targetType == "" || !CursorManager.manager.selectable) {
            return;
        }
        select = true;
        if (Input.GetMouseButtonDown(0)) {
            StartSelection();
        }
        if (Input.GetMouseButtonUp(0)) {
            EndSelection();
        }
	}

    void CheckInput() {
        foreach (string key in hotKeys.Keys) {
            if (Input.GetKeyDown(key)) {
                string target = hotKeys[key];
                SetTarget(target);
                return;
            }
        }
    }

    bool UIActive() {
        bool active = false;
        foreach (GameObject uiElement in GameObject.FindGameObjectsWithTag("ui-container")) {
            if (active) {
                break;
            }
            active = uiElement.GetComponent<UIState>().isActive;
        }
        return active;
    }

    void StartSelection() {
        downMousePos = Input.mousePosition;
        // downMousePos = WorldToScreenPoint?
        // does it make sense to calculate hitDown here?
        hitDown = UnityEngine.Camera.main.ScreenToWorldPoint(downMousePos);
    }

    void EndSelection() {
        // hitDown is changing; need to track camera movement if hitDown
        // hitDown could be calculated here from downMousePos;
        hitUp = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 center = (hitDown + hitUp) * 0.5f;
        Vector2 size = new Vector2(Mathf.Abs(hitUp.x - hitDown.x), Mathf.Abs(hitDown.y - hitUp.y));
        RaycastHit2D[] check = Physics2D.BoxCastAll(center, size, selectionAngle, selectionDirection);
        AddSelectedObjectsToQueue(check);
    }

    void AddSelectedObjectsToQueue(RaycastHit2D[] objects) {
        foreach (RaycastHit2D hit in objects) {
            Properties props = hit.collider.gameObject.GetComponent<Properties>();
            if (props == null) {
                continue;
            }
            if (hit.collider != null && hit.collider.tag == "task" && props.type == targetType && !props.selected) {
                hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite = selectedSprites[targetType].GetComponent<SpriteRenderer>().sprite;
                props.selected = true;
            }
        }
    }

    void OnGUI() {
        DrawSelectionBox();
    }

    private void DrawSelectionBox() {
        if (Input.GetMouseButton(0) && select && targetType != "" && !placeable) {
            Vector3 currentPos = Input.mousePosition;
            Rect boxRect = new Rect(downMousePos.x, Screen.height - downMousePos.y, currentPos.x - downMousePos.x, downMousePos.y - currentPos.y);
            GUI.Box(boxRect, "", skin.box);
        }
    }

    public void SetTarget(string name) {
        targetType = name;
        placeable = CursorManager.manager.SetCursorImage(name);
    }
}
