using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {

    GameObject borderTree;
    Vector3 hitDown;
    Vector3 hitUp;
    float selectionAngle = 0f;
    Vector2 selectionDirection = new Vector2(0, 0);

    void Start() {
        Object borderTreePrefab = Resources.Load("Prefabs/tree-orange-highlighted", typeof(GameObject));
        borderTree = Instantiate(borderTreePrefab, new Vector2(-10000, -10000), Quaternion.identity) as GameObject;
    }

	void Update() {
        Vector2 touchPosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        if (Input.GetMouseButtonDown(0)) {
            hitDown = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0)) {
            hitUp = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 center = (hitDown + hitUp) * 0.5f;
            Vector2 size = new Vector2(Mathf.Abs(hitUp.x - hitDown.x), Mathf.Abs(hitDown.y - hitUp.y));
            RaycastHit2D[] check = Physics2D.BoxCastAll(center, size, selectionAngle, selectionDirection);

            foreach (RaycastHit2D hit in check) {
                if (hit.collider != null && hit.collider.tag == "task") {
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite = borderTree.GetComponent<SpriteRenderer>().sprite;
                    TreeBucket.treeBucket.targetTrees.Add(hit.collider.gameObject);
                }
            }
        }
	}
}
