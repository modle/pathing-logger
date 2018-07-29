using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DetailsDisplay : EventTrigger {

    string type;
    int count = 0;

    public void OnMouseDown() {
        count++;
        print ("clicked " + name + " " + count + " times");

    }
}
