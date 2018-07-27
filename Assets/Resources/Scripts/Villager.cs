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
    public Building building;
    public bool working;
    private float workStart = 0f;
    private float workDone = 2f;
    private AudioSource audioSource;
    public AudioClip workClip;
    public AudioClip storageClip;
    public string job;
    public int id;
    private Rect idRect;
    public string material;
    private bool retrigger;
    GameObject triggerObject;

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
        if (retrigger && triggerObject != null) {
            ProcessTrigger(triggerObject);
            return;
        }
        Move();
    }

    void Move() {
        SetDefaults();
        if (working && IsStillWorking()) {
            PerformWorkActions();
        }
        if (working && job != "hauler" && target != null && material == "" && !IsStillWorking()) {
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

    void PerformWorkActions() {
        anim.SetBool("side-attack", true);
        anim.speed = 1;
        if ((int)((Time.time - workStart) * 100) % 30 == 0) {
            audioSource.PlayOneShot(workClip, 0.7F);
        }
    }

    void ProcessWorking() {
        if (target.GetComponent<Properties>().type == "building") {
            building = target.GetComponent<Building>();
        }

        if (IsStillWorking() && building == null) {
            return;
        }

        if (building != null) {
            if (!building.ReadyToProduce() && !building.Producing() && material == "") {
                GoGetTheThing();
                return;
            }
            DoBuildingThings();
            return;
        }

        FinishWorking();
    }

    bool IsStillWorking() {
        return (Time.time - workStart) < workDone;
    }

    void DoBuildingThings() {
        if (building.Producing()) {
            building.Produce();
        }
    }

    void GoGetTheThing() {
        material = building.NextStockToGet();
        target = GameObject.Find("Storage");
        return;
    }

    void FinishWorking() {
        Properties props = target.GetComponent<Properties>();
        props.Haulify();
        props.ChangeSprite();
        StopWorking();
    }

    void StopWorking() {
        anim.SetBool("side", true);
        working = false;
        target = null;
        building = null;
        retrigger = false;
        triggerObject = null;
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
        if (target.GetComponent<Properties>().type == "tree") {
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
        // TODO: subfunction, closest = CompareToTargets();
        foreach (GameObject go in TargetBucket.bucket.targets) {
            if (go == null) {
                continue;
            }
            Properties props = go.GetComponent<Properties>();
            if (!props.selected || props.targeted || props.job != job) {
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
            target.GetComponent<Properties>().targeted = true;
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
            target.GetComponent<Properties>().AbandonTask();
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
        if (target == null || other.GetComponent<Properties>() == null) {
            // nothing to do
            return;
        }
        Properties props = target.GetComponent<Properties>();
        string state = DetermineState(props, other);
        ExecuteStateAction(props, other, state);
    }

    string DetermineState(Properties props, GameObject other) {
        string state = "";
        state = state == "" && !props.targeted && props.type != "storage" ? "ResetTarget" : state;
        if (other.gameObject.GetInstanceID() == target.GetInstanceID() && props.targeted) {
            state = state == "" && !props.workable ? "CollectTarget" : state;
            state = state == "" && haveMaterials && building != null ? "AddStock" : state;
            state = state == "" ? "StartWork" : state;
            return state;
        }
        if (props.type == "storage" && other.GetComponent<Properties>().type == "storage") {
            state = state == "" && haveMaterials && ResourceCounter.counter.resources.Contains(material) ? "PutInStorage" : state;
            state = state == "" && building != null && !haveMaterials ? "GetFromStorage" : state;
        }
        return state;
    }

    void ExecuteStateAction(Properties props, GameObject other, string state) {
        if (state == "ResetTarget") {
            target = null;
            return;
        }
        if (state == "AddStock") {
            AddStock();
            return;
        }
        if (state == "StartWork") {
            StartWork(props);
            return;
        }
        if (state == "CollectTarget") {
            CollectTarget(props);
            return;
        }
        if (state == "PutInStorage") {
            PutInStorage();
            return;
        }
        if (state == "GetFromStorage") {
            GetFromStorage(other);
            return;
        }
    }

    void AddStock() {
        building.AddStock(material);
        haveMaterials = false;
        material = "";
        workStart = Time.time;
    }

    void StartWork(Properties props) {
        working = true;
        props.engaged = true;
        if (props.type != "building" || haveMaterials) {
            workStart = Time.time;
        }
    }

    void CollectTarget(Properties props) {
        material = target.GetComponent<Properties>().produces;
        if (props.destructable) {
            Destroy(target);
        }
        target = GameObject.Find("Storage");
        haveMaterials = true;

    }

    void PutInStorage() {
        target = null;
        haveMaterials = false;
        audioSource.PlayOneShot(storageClip, 0.7F);
        ResourceCounter.counter.counts[material]++;
        material = "";
    }

    void GetFromStorage(GameObject other) {
        if (ResourceCounter.counter.counts[material] > 0) {
            target = building.transform.gameObject;
            haveMaterials = true;
            ResourceCounter.counter.counts[material]--;
            retrigger = false;
            triggerObject = null;
        } else {
            retrigger = true;
            triggerObject = other;
        }
    }

    string GetRepr() {
        return id.ToString() + " - " + job;
        // return id + " | " + job + (target != null ? "\n" + target.name + " | " + target.tag : "");
    }
}
