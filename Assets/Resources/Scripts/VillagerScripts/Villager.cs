using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Villager : MonoBehaviour {

    private Transform transform;
    public Building building;
    public bool working;
    private float workStart = 0f;
    private float workDone = 2f;
    public AudioClip workClip;

    public int id;
    private Rect idRect;
    public string material;
    public bool recollide;
    public GameObject collisionObject;
    private BoxCollider2D boxCollider;
    public LayerMask blockingLayer;
    public bool haveMaterials = false;

    private Actions actions;
    private Animations animations;
    private Job job;
    private State stateObj;
    private Targets targets;
    private Work work;

    void Start () {
        SetInitialReferences();
    }

    void SetInitialReferences() {
        transform = GetComponent<Transform>();
        boxCollider = GetComponent<BoxCollider2D>();

        actions = GetComponent<Actions>();
        animations = GetComponent<Animations>();
        job = GetComponent<Job>();
        stateObj = GetComponent<State>();
        targets = GetComponent<Targets>();
        work = GetComponent<Work>();
    }

    void Update () {
        transform.Find("villager-label(Clone)").GetComponent<TextMesh>().text = id + " - " + job.GetCurrentJob() + "/" + job.GetJob();
        if (recollide && collisionObject != null) {
            targets.ProcessCollision(collisionObject);
            return;
        }
        if (working && IsStillWorking()) {
            PerformWorkActions();
            return;
        }
        if (working && job.GetCurrentJob() != "hauler" && targets.target != null && material == "" && !IsStillWorking()) {
            ProcessWorking();
            return;
        }
        if (!targets.HasTarget()) {
            return;
        }
        animations.Move(targets.target);
        // every 5 seconds
    }

    void PerformWorkActions() {
        animations.anim.SetBool("side-attack", true);
        animations.anim.speed = 1;
        if ((int)((Time.time - workStart) * 100) % 30 == 0) {
            targets.audioSource.PlayOneShot(workClip, 0.7F);
        }
    }

    void ProcessWorking() {
        if (targets.target.GetComponent<Properties>().type == "building") {
            building = targets.target.GetComponent<Building>();
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
            if (job.GetCurrentJob() == "builder") {
                StopWorking();
            }
        }
    }

    void GoGetTheThing() {
        material = building.NextStockToGet();
        targets.target = GameObject.Find("Storage");
        return;
    }

    void FinishWorking() {
        Properties props = targets.target.GetComponent<Properties>();
        props.Haulify();
        props.ChangeSprite();
        StopWorking();
    }

    public void StopWorking() {
        animations.anim.SetBool("side", true);
        working = false;
        targets.target = null;
        if (building != null) {
            building.GetComponent<Properties>().SetDefaults();
        }
        building = null;
        recollide = false;
        collisionObject = null;
    }

    public void DropMaterial() {
        if (haveMaterials && material != "") {
            TargetBucket.bucket.InstantiateResource(transform.position, ResourcePrefabs.resources.gatherableResourceSprites[material]);
        }
        haveMaterials = false;
        material = "";
    }

    public string DetermineState(Properties props, GameObject other) {
        string state = "";
        state = state == "" && !props.targeted && props.type != "storage" ? "ResetTarget" : state;
        if (other.gameObject.GetInstanceID() == targets.target.GetInstanceID() && props.targeted) {
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

    public void ExecuteStateAction(Properties props, GameObject other, string state) {
        if (state == "ResetTarget") {
            targets.target = null;
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
            targets.CollectTarget(props);
            return;
        }
        if (state == "PutInStorage") {
            targets.PutInStorage();
            return;
        }
        if (state == "GetFromStorage") {
            targets.GetFromStorage(other);
            return;
        }
    }

    void AddStock() {
        building.AddStock(material);
        haveMaterials = false;
        material = "";
        if (!building.built) {
            return;
        }
        workStart = Time.time;
    }

    void StartWork(Properties props) {
        working = true;
        props.engaged = true;
        if (props.type != "building" || haveMaterials) {
            workStart = Time.time;
        }
    }

    public string GetRepr() {
        return CapitalizeFirstLetter(job.GetCurrentJob()) + "/" + CapitalizeFirstLetter(job.GetJob()) + " (" + id.ToString() + ")" +
            "\n" + (working ? "working" : "idle") + " " +
            string.Format("{0:0.0}", (Time.time - workStart < 2.0f ? Time.time - workStart : 0f)) +
            "\ntarget: " + (targets.target == null ? "" : CapitalizeFirstLetter(targets.target.name)) +
            "\nbuilding: " + (building == null ? "" : CapitalizeFirstLetter(building.name)) +
            "\n" + (haveMaterials ? "carrying: " : "finding: ") + material;
    }

    string CapitalizeFirstLetter(string s) {
        char[] a = s.ToCharArray();
        a[0] = char.ToUpper(a[0]);
        return new string(a);
    }

}
