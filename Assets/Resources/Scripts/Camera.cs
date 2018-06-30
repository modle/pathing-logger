using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

    // Update is called once per frame
    void Update () {
        Vector3 myVector = new Vector3(transform.position.x, transform.position.y, -3);
        Vector2 touchPosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        if (Input.GetMouseButton(0)) {
            myVector.x += (touchPosition.x < Screen.width / 2 ? -0.1f : 0.1f);
            myVector.y += (touchPosition.y > Screen.height / 2 ? -0.1f : 0.1f);
        }
        transform.position = myVector;
    }
}
