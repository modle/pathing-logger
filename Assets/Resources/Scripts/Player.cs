using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player player;
    private SpriteRenderer spriteRenderer;
    public Transform transform;
    private int speedMod = 120;
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
        {"0,0", "idle"}
    };

    private BoxCollider2D boxCollider;
    public LayerMask blockingLayer;
    Animator anim;
    bool idleFlipX = false;

    void Awake() {
        // singleton pattern
        if (player == null) {
            DontDestroyOnLoad(gameObject);
            player = this;
        } else if (player != this) {
            Destroy(gameObject);
        }
    }

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
        GetManualCoordinates();
        if (theX == 0 && theY == 0) {
            GetAutoCoordinates();
        }
        if (theX == 0 && theY == 0) {
            return false;
        }
        SetDirections();
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

    void GetManualCoordinates() {
        // be sure to set Body Type to Kinematic on the Box Collider 2D, else physics moves sprite downward
        Vector2 touchPosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        if (Input.GetMouseButton(0)) {
            theX = touchPosition.x;
            theY = touchPosition.y;
            horizontal = theX < Screen.width / 2 ? -1 : 1;
            vertical = theY > Screen.height / 2 ? -1 : 1;
            if (Mathf.Abs((Mathf.Abs(theX) - Screen.width / 2)) < Mathf.Abs((Mathf.Abs(theY) - Screen.height / 2))) {
                horizontal = 0;
            } else {
                vertical = 0;
            }
        }
    }

    void GetAutoCoordinates() {
        if (tasks == null || tasks.Length <= 0) {
            tasks = GameObject.FindGameObjectsWithTag("task");
        }
        if (tasks.Length <= 0) {
            return;
        }
        target = tasks[0];
        theX = transform.position.x - target.transform.position.x;
        theY = transform.position.y - target.transform.position.y;
        horizontal = theX < 0 ? 1 : -1;
        vertical = theY < 0 ? 1 : -1;
        if (Mathf.Abs(theX) < Mathf.Abs(theY)) {
            horizontal = 0;
        } else {
            vertical = 0;
        }
    }

    void SetDirections() {
        string direction = directions[(int)horizontal + "," + (int)vertical];
        bool flipX = ((int)horizontal == 1 && (int)vertical == 0) ? false : true;
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

    //OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
    //target object needs a box collider 2D, Is Trigger enabled, and layer set to crop
    private void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "crop") {
            GameObject boardHolder = GameObject.Find("Food");
            Object toInstantiate = Resources.Load("Prefabs/crop-patch", typeof(GameObject));
            GameObject instance = Instantiate(
                    toInstantiate, 
                    new Vector3 (
                        other.transform.position.x,
                        other.transform.position.y, 
                        0
                    ),
                    Quaternion.identity) as GameObject;
            instance.transform.SetParent(boardHolder.transform);
            Destroy(other.gameObject);
        }
    }

    void OnGUI() {
        GUIStyle font = new GUIStyle();
        font.normal.textColor = Color.black;
        font.fontSize = (int)(xUnit * 5);
        GUI.Label(
            new Rect(Screen.width / 2 - xUnit * 10, Screen.height / 2 - xUnit * 20, xUnit * 20, xUnit * 20),
            GetText(horizontal, vertical),
            font
        );
        GUI.Label(
            new Rect(Screen.width / 2 - xUnit * 10, Screen.height / 2 - xUnit * 25, xUnit * 20, xUnit * 20),
            GetText(transform.position.x, transform.position.y),
            font
        );
    }

    string GetText(float x, float y) {
        return string.Format("x: {0:#,##0.00}, y: {1:#,##0.00}", x, y);
    }
}
