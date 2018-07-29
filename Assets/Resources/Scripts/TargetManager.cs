﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour {

    public static TargetManager manager;

    Vector3 hitDown;
    Vector3 hitUp;
    Vector3 downMousePos;
    float selectionAngle = 0f;
    Vector2 selectionDirection = new Vector2(0, 0);
    public GUISkin skin;
    private bool select;
    public string targetType;
    private Transform selectors;
    Dictionary<string, GameObject> selectedSprites = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> harvestedSprites = new Dictionary<string, GameObject>();
    public bool placeable;
    public bool targetMode;

    void Awake() {
        // singleton pattern
        if (manager == null) {
            DontDestroyOnLoad(gameObject);
            manager = this;
        } else if (manager != this) {
            Destroy(gameObject);
        }
        selectors = GameObject.Find("Selectors").transform;
    }

    void Start() {

        // use PrefabUtils here
        Object borderTreePrefab = Resources.Load("Prefabs/tree-orange-highlighted", typeof(GameObject));
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

	void Update() {
        select = false;
        if (UIActive()) {
            return;
        }
        if (placeable) {
            BuildingManager.manager.PlaceBuilding(targetType);
            return;
        }
        if (targetType == "") {
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
        hitDown = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void EndSelection() {
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
