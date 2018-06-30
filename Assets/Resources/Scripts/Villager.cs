using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    public Transform transform;
    private int speedMod = 50;
    float horizontal = 0f;
    float vertical = 0f;
    float theX = 0f;
    float theY = 0f;
    public float xUnit;
    public float yUnit;
    private GameObject[] tasks;
    private GameObject target;
    public Dictionary<string, string> directions = new Dictionary<string, string>() {
        {"-1,0", "side"},
        {"1,0", "side"},
        {"0,1", "up"},
        {"0,-1", "down"},
        {"1,1", "side"},
        {"-1,1", "side"},
        {"1,-1", "side"},
        {"-1,-1", "side"},
        {"0,0", "idle"}
    };

    private BoxCollider2D boxCollider;
    public LayerMask blockingLayer;
    Animator anim;
    bool idleFlipX = false;

    void Start () {
        xUnit = Screen.width * 0.005f;
        yUnit = Screen.height * 0.005f;
        SetInitialReferences();
    }

    void SetInitialReferences() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform = GetComponent<Transform>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    void Update () {
        Move();
    }

    bool Move() {
        SetDefaults();
        GetCoordinates();
        SetDirections();
        if (horizontal == 0 && vertical == 0) {
            return false;
        }
        return (MoveSprite());
    }

    void SetDefaults() {
        // add parameters with the same names to the Animator
        // on each transition:
            // uncheck "Has Exit Time"
            // Expand "Settings"
            // uncheck "Fixed Duration"
            // set "Transition Duration" to 0
            // Add appropriate condition (up -> side, side=true, etc.)
        anim.SetBool("side", false);
        anim.SetBool("up", false);
        anim.SetBool("down", false);
        theX = 0f;
        theY = 0f;
        horizontal = 0;
        vertical = 0;
    }

    void GetCoordinates() {
        if (tasks == null || tasks.Length <= 0) {
            tasks = GameObject.FindGameObjectsWithTag("task");
        }
        if (tasks.Length <= 0) {
            return;
        }
        target = tasks[0];
        theX = transform.position.x - target.transform.position.x;
        theY = transform.position.y - target.transform.position.y;
        if (theX < -0.01f) {
            horizontal = 1;
        } else if (theX > 0.01f) {
            horizontal = -1;
        }
        if (theY < -0.01f) {
            vertical = 1;
        } else if (theY > 0.01f) {
            vertical = -1;
        }
        Debug.Log(string.Format("theX is {0:f}; theY is {1:f}; horizontal is {2:f}; vertical is {3:f}", theX, theY, horizontal, vertical));
    }

    void SetDirections() {
        string direction = directions[horizontal + "," + vertical];
        bool flipX = (horizontal != 0 && horizontal == 1) ? false : true;
        spriteRenderer.flipX = flipX;
        idleFlipX = direction == "idle" ? idleFlipX : flipX;
        if (direction == "idle") {
            anim.speed = 0;
            spriteRenderer.flipX = idleFlipX;
        } else {
            anim.speed = 1;
            anim.SetBool(direction, true);
        }
    }

    bool MoveSprite() {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(horizontal / speedMod, vertical / speedMod);

        boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast (start, end, blockingLayer);
        boxCollider.enabled = true;

        if ((horizontal != 0 || vertical != 0) && hit.transform == null) {
            transform.position = end;
            return true;
        }
        return false;
    }

}
