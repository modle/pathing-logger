using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuToggler : MonoBehaviour {

    Transform menus;
    List<string> doNotHideThese = new List<string>() {"MessageLog", "QuestPane"};

    void Start() {
        SetObjects();
    }

    void SetObjects() {
        menus = GameObject.Find("Menus").transform;
        foreach (Transform t in menus) {
            if (doNotHideThese.Contains(t.name)) {
                continue;
            }
            t.gameObject.SetActive(false);
        }
    }

	void Update() {
        CheckInput();
	}

    void CheckInput() {
        foreach (Transform t in menus) {
            if (Input.GetKeyDown("escape") && !doNotHideThese.Contains(t.name)) {
                t.gameObject.SetActive(false);
                continue;
            }
            if (Input.GetKeyDown(t.gameObject.GetComponent<MenuProps>().hotKey)) {
                t.gameObject.SetActive(!t.gameObject.activeSelf);
                break;
            }
        }
    }
}
