using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {

    GameObject borderTree;
    RaycastHit2D hitDown;

    void Start() {
        Object borderTreePrefab = Resources.Load("Prefabs/tree-orange-highlighted", typeof(GameObject));
        borderTree = Instantiate(borderTreePrefab, new Vector2(-10000, -10000), Quaternion.identity) as GameObject;
    }

	// Update is called once per frame
	void Update() {
        Vector2 touchPosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        if (Input.GetMouseButtonDown(0)) {
            hitDown = Physics2D.Raycast(UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        }
        if (Input.GetMouseButtonUp(0)) {
            RaycastHit2D hitUp = Physics2D.Raycast(UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            Rect selectionBox = new Rect(hitDown.point.x, hitUp.point.y, hitUp.point.x - hitDown.point.x, hitDown.point.y - hitUp.point.y);
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("task")) {
                if (selectionBox.Contains(go.transform.position)) {
                    go.GetComponent<SpriteRenderer>().sprite = borderTree.GetComponent<SpriteRenderer>().sprite;
                    TreeBucket.treeBucket.targetTrees.Add(go);
                }
            }

            // RaycastHit2D hitUp = Physics2D.Raycast(UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
        }
	}
}
