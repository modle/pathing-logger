using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

    private float speed = 0.1f;

    void Update () {
        Vector3 myVector = new Vector3(transform.position.x, transform.position.y, -3);
        myVector.x += Input.GetAxisRaw("Horizontal") * speed;
        myVector.y += Input.GetAxisRaw("Vertical") * speed;
        transform.position = myVector;
    }
}
