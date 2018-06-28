using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

    // Update is called once per frame
    void Update () {
        Vector3 myVector = new Vector3(Player.player.transform.position.x, Player.player.transform.position.y, -3);
        transform.position = myVector;
    }
}
