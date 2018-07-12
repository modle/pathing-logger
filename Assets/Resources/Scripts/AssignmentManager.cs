using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignmentManager : MonoBehaviour {

    public static AssignmentManager manager;

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
    }

	void Update() {
	}

    void OnGUI() {
        if (Input.GetMouseButton(0)) {
            Vector3 currentPos = Input.mousePosition;
        }
    }
}
