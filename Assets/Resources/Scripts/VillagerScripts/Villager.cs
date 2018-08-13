using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Villager : MonoBehaviour {

    private Transform transform;
    public GameObject target;
    public Building building;
    public bool working;
    private float workStart = 0f;
    private float workDone = 2f;
    private AudioSource audioSource;
    public AudioClip workClip;
    public AudioClip storageClip;
    public string job;
    public string baseJob;
    public int id;
    private Rect idRect;
    public string material;
    private bool recollide;
    GameObject collisionObject;
    private BoxCollider2D boxCollider;
    public LayerMask blockingLayer;
    public bool haveMaterials = false;

    private Actions actions;
    private Animations animations;
    private Job jobObj;
    private State stateObj;
    private Targets targets;
    private Work work;

    void Start () {
        SetInitialReferences();
        StartCoroutine("CheckJob");
    }

    void SetInitialReferences() {
        transform = GetComponent<Transform>();
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();

        actions = GetComponent<Actions>();
        animations = GetComponent<Animations>();
        jobObj = GetComponent<Job>();
        stateObj = GetComponent<State>();
        targets = GetComponent<Targets>();
        work = GetComponent<Work>();
    }

    void Update () {
        transform.Find("villager-label(Clone)").GetComponent<TextMesh>().text = id + " - " + job + "/" + baseJob;
        if (recollide && collisionObject != null) {
            ProcessCollision(collisionObject);
            return;
        }
        if (working && IsStillWorking()) {
            PerformWorkActions();
            return;
        }
        if (working && job != "hauler" && target != null && material == "" && !IsStillWorking()) {
            ProcessWorking();
            return;
        }
        if (target == null) {
            GetClosest();
        }
        if (target == null) {
            return;
        }
        animations.Move(target);
        // every 5 seconds
    }

    void PerformWorkActions() {
        animations.anim.SetBool("side-attack", true);
        animations.anim.speed = 1;
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
            if (job == "builder") {
                StopWorking();
            }
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
        animations.anim.SetBool("side", true);
        working = false;
        target = null;
        if (building != null) {
            building.GetComponent<Properties>().SetDefaults();
        }
        building = null;
        recollide = false;
        collisionObject = null;
    }

    private void GetClosest() {
        if (target != null) {
            return;
        }
        GameObject closest = CompareToTargets();
        if (closest != null) {
            target = closest;
            target.GetComponent<Properties>().SetTargeted(id);
            if (target.GetComponent<Properties>().type == "building") {
                ProcessCollision(target);
            }
        }
    }

    private GameObject CompareToTargets() {
        Vector3 position = transform.position;
        float distance = Mathf.Infinity;
        GameObject match = null;
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
                match = go;
                distance = curDistance;
            }
        }
        return match;
    }


    public string GetJob() {
        return baseJob;
    }

    IEnumerator CheckJob() {
        while (true) {
            if (target == null) {
                job = "hauler";
                foreach (GameObject go in TargetBucket.bucket.targets) {
                    if (go == null) {
                        continue;
                    }
                    Properties props = go.GetComponent<Properties>();
                    if (!props.selected || props.targeted) {
                        continue;
                    }
                    // reassigns villagers to baseJob if job is needed
                    if (props.job == baseJob && baseJob != "hauler") {
                        job = baseJob;
                        break;
                    }
                    // reassigns job to builder if building in progress needs materials
                    if (props.job == "builder") {
                        job = "builder";
                    }
                }
            }
            yield return new WaitForSeconds(2.0f);
        }
    }

    public void ChangeJob(string newJob) {
        SetJob(newJob);
        DropMaterial();
        if (target != null) {
            target.GetComponent<Properties>().AbandonTask();
            StopWorking();
        }
    }

    public void SetJob(string newJob) {
        job = newJob;
        baseJob = newJob;
    }

    private void DropMaterial() {
        if (haveMaterials && material != "") {
            TargetBucket.bucket.InstantiateResource(transform.position, ResourcePrefabs.resources.gatherableResourceSprites[material]);
        }
        haveMaterials = false;
        material = "";
    }

    private void OnCollisionStay2D(Collision2D other) {
        if (target != null && !working && other.gameObject.GetInstanceID() == target.GetInstanceID()) {
            ProcessCollision(other.gameObject);
            return;
        }
        // prevents villager from getting stuck when inside storage
        if (target != null && target.name == "Storage") {
            ProcessCollision(other.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        ProcessCollision(other.gameObject);
    }

    void ProcessCollision(GameObject other) {
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

        // allows villager to pivot roles on hauler task completion
        StopCoroutine("CheckJob");
        StartCoroutine("CheckJob");
    }

    void GetFromStorage(GameObject other) {
        if (ResourceCounter.counter.counts[material] > 0) {
            target = building.transform.gameObject;
            haveMaterials = true;
            ResourceCounter.counter.counts[material]--;
            recollide = false;
            collisionObject = null;
        } else {
            recollide = true;
            collisionObject = other;
        }
    }

    public string GetRepr() {
        return CapitalizeFirstLetter(job) + "/" + CapitalizeFirstLetter(baseJob) + " (" + id.ToString() + ")" +
            "\n" + (working ? "working" : "idle") + " " +
            string.Format("{0:0.0}", (Time.time - workStart < 2.0f ? Time.time - workStart : 0f)) +
            "\ntarget: " + (target == null ? "" : CapitalizeFirstLetter(target.name)) +
            "\nbuilding: " + (building == null ? "" : CapitalizeFirstLetter(building.name)) +
            "\n" + (haveMaterials ? "carrying: " : "finding: ") + material;
    }

    string CapitalizeFirstLetter(string s) {
        char[] a = s.ToCharArray();
        a[0] = char.ToUpper(a[0]);
        return new string(a);
    }
}
