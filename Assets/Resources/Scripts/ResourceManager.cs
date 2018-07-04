using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {

    GameObject borderTree;

    void Start() {
        Object borderTreePrefab = Resources.Load("Prefabs/tree-orange-highlighted", typeof(GameObject));
        borderTree = Instantiate(borderTreePrefab, new Vector2(-10000, -10000), Quaternion.identity) as GameObject;
    }

	// Update is called once per frame
	void Update() {
        Vector2 touchPosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        if (Input.GetMouseButton(0)) {

            RaycastHit2D hit = Physics2D.Raycast(UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.tag == "task") {
                hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite = borderTree.GetComponent<SpriteRenderer>().sprite;
                TreeBucket.treeBucket.targetTrees.Add(hit.collider.gameObject);
            }

        }
	}
}
