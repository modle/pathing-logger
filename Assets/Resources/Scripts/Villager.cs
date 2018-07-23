using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Villager : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private Transform transform;
    private int speedMod = 50;
    private float horizontal = 0f;
    private float vertical = 0f;
    private float theX = 0f;
    private float theY = 0f;
    public GameObject target;
    public bool working;
    private float workStart = 0f;
    private float workDone = 1f;
    private AudioSource audioSource;
    public AudioClip workClip;
    public AudioClip storageClip;
    public string job;
    public int id;
    private Rect idRect;
    public string material;

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
        transform.Find("villager-label(Clone)").GetComponent<TextMesh>().text = GetRepr();
        Move();
    }

    void Move() {
        SetDefaults();
        if (working && job != "hauler" && target != null) {
            ProcessWorking();
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

    void ProcessWorking() {
        if (working && (Time.time - workStart) < workDone) {
            PerformWorkActions();
            return;
        }
        if (target.GetComponent<TargetID>().type == "building") {
            target.GetComponent<Building>().Produce();
            workStart = Time.time;
            return;
        }
        FinishWorking();
    }

    void PerformWorkActions() {
        anim.SetBool("side-attack", true);
        anim.speed = 1;
        if ((int)((Time.time - workStart) * 100) % 30 == 0) {
            audioSource.PlayOneShot(workClip, 0.7F);
        }
    }

    void FinishWorking() {
        TargetID id = target.GetComponent<TargetID>();
        id.Haulify();
        id.ChangeSprite();
        StopWorking();
    }

    void StopWorking() {
        anim.SetBool("side", true);
        working = false;
        target = null;
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
        if (target.GetComponent<TargetID>().type == "tree") {
            theX = transform.position.x - (target.transform.position.x + target.transform.gameObject.GetComponent<SpriteRenderer>().bounds.size.x * 0.5f);
            theY = transform.position.y - (target.transform.position.y - target.transform.gameObject.GetComponent<SpriteRenderer>().bounds.size.y * 0.3f);
        }
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
        foreach (GameObject go in TargetBucket.bucket.targets) {
            if (go == null) {
                continue;
            }
            TargetID id = go.GetComponent<TargetID>();
            if (!id.selected || id.targeted || id.job != job) {
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
            target.GetComponent<TargetID>().targeted = true;
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

    public string GetJob() {
        return job;
    }

    public void ChangeJob(string newJob) {
        job = newJob;
        if (target != null) {
            target.GetComponent<TargetID>().AbandonTask();
            if (material != "") {
                TargetBucket.bucket.InstantiateResource(transform.position, ResourcePrefabs.resources.gatherableResourceSprites[material]);
            }
            StopWorking();
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (target != null && !working && other.gameObject.GetInstanceID() == target.GetInstanceID()) {
            ProcessTrigger(other.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        ProcessTrigger(other.gameObject);
    }

    void ProcessTrigger(GameObject other) {
        // TODO this is a bit of a mess; not clear what purpose is
        // it's handling what villager does when colliding with
        // an interactable object
        if (target == null || other.GetComponent<TargetID>() == null) {
            return;
        }
        TargetID id = target.GetComponent<TargetID>();
        if (!id.targeted && id.type != "storage") {
            target = null;
            return;
        }
        if (other.gameObject.GetInstanceID() == target.GetInstanceID() && id.targeted) {
            if (id.workable) {
                working = true;
                id.engaged = true;
                workStart = Time.time;
            } else {
                CollectTarget(id);
            }
        } else if (id.type == "storage" && other.GetComponent<TargetID>().type == "storage" && haveMaterials && ResourceCounter.counter.resources.Contains(material)) {
            target = null;
            haveMaterials = false;
            audioSource.PlayOneShot(storageClip, 0.7F);
            ResourceCounter.counter.counts[material]++;
            material = "";
        }
    }

    void CollectTarget(TargetID id) {
        material = target.GetComponent<TargetID>().produces;
        if (id.destructable) {
            Destroy(target);
        }
        target = GameObject.Find("Storage");
        haveMaterials = true;

    }

    string GetRepr() {
        return id.ToString();
        // return id + " | " + job + (target != null ? "\n" + target.name + " | " + target.tag : "");
    }
}
