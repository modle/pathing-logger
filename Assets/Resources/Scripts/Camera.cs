using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

    private float speed = 0.1f;

    void Update () {
        Vector3 myVector = new Vector3(transform.position.x, transform.position.y, -3);
        Vector3 moveVector = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        moveVector = moveVector * speed;
        // myVector.x += Input.GetAxisRaw("Horizontal") * speed;
        // myVector.y += Input.GetAxisRaw("Vertical") * speed;
        transform.position = myVector + moveVector;
        TargetManager.manager.hitDown = TargetManager.manager.hitDown + moveVector;
        TargetManager.manager.downMousePos = UnityEngine.Camera.main.WorldToScreenPoint(TargetManager.manager.hitDown);
    }
}
