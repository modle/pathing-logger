using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private Transform transform;
    private int speedMod = 50;
    private float horizontal = 0f;
    private float vertical = 0f;
    private float theX = 0f;
    private float theY = 0f;
    private float xUnit;
    private float yUnit;
    private GameObject target;
    private bool chopping;
    private float chopStart = 0f;
    private float chopDone = 1f;
    private AudioSource audioSource;
    public AudioClip chopClip;
    public AudioClip storageClip;

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
    private bool haveMaterials = false;

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
        audioSource = GetComponent<AudioSource>();
    }

    void Update () {
        Move();
    }

    void Move() {
        SetDefaults();
        if (chopping) {
            ProcessChopping();
            return;
        }
        GetTargetCoordinates();
        SetDirections();
        if (horizontal == 0 && vertical == 0) {
            return;
        }
        MoveSprite();
    }

    void SetDefaults() {
        // add parameters with the same names to the Animator
        // on each transition from "Any State" to target state:
            // uncheck "Has Exit Time"
            // Expand "Settings"
            // uncheck "Fixed Duration"
            // set "Transition Duration" to 0
            // uncheck "Can Transition to Self" if using "Any State" as the from
            // Add appropriate condition (up -> side, side=true, etc.)
        anim.SetBool("side", false);
        anim.SetBool("up", false);
        anim.SetBool("down", false);
        anim.SetBool("side-attack", false);
        theX = 0f;
        theY = 0f;
        horizontal = 0;
        vertical = 0;
    }

    void ProcessChopping() {
        if (chopping && Time.time - chopStart < chopDone) {
            anim.SetBool("side-attack", true);
            anim.speed = 1;
            if ((int)((Time.time - chopStart) * 100) % 30 == 0) {
                audioSource.PlayOneShot(chopClip, 0.7F);
            }
            return;
        }
        chopping = false;
        Destroy(target.gameObject);
        target = GameObject.Find("Storage");
        haveMaterials = true;
    }

    void GetTargetCoordinates() {
        if (target == null) {
            GetClosest();
        }
        if (target == null) {
            return;
        }
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
    }

    private void GetClosest() {
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        if (target != null) {
            return;
        }
        foreach (GameObject go in TreeBucket.treeBucket.targetTrees) {
            if (go == null || go.tag != "task" || target != null) {
                continue;
            }
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance) {
                closest = go;
                distance = curDistance;
            }
        }
        if (closest != null) {
            target = closest;
            TreeBucket.treeBucket.targetTrees.Remove(closest);
            target.tag = "engaged";
        }
    }

    void SetDirections() {
        string direction = directions[horizontal + "," + vertical];
        bool flipX = (horizontal != 0 && horizontal == 1) ? false : true;
        spriteRenderer.flipX = flipX;
        idleFlipX = direction == "idle" ? idleFlipX : flipX;
        if (direction == "idle") {
            spriteRenderer.flipX = idleFlipX;
        } else {
            anim.speed = 1;
            anim.SetBool(direction, true);
        }
    }

    bool MoveSprite() {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(horizontal / speedMod, vertical / speedMod);

        if (horizontal != 0 || vertical != 0) {
            transform.position = end;
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D (Collider2D other) {
        if (target == null || chopping == true) {
            return;
        }
        if (other.gameObject.GetInstanceID() == target.GetInstanceID() && target.tag == "engaged") {
            chopping = true;
            chopStart = Time.time;
        } else if (target.tag == "storage" && other.tag == "storage" && haveMaterials) {
            target = null;
            haveMaterials = false;
            audioSource.PlayOneShot(storageClip, 0.7F);
            WoodCounter.counter.count++;
        }
    }
}
