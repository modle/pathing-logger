using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Work : MonoBehaviour {

    private Targets targets;
    public Building building;
    public Animations animations;
    private Job job;

    public bool working;
    public float workStart = 0f;
    public float workDone = 2f;
    public AudioSource audioSource;
    public AudioClip workClip;
    public AudioClip storageClip;

    public string material;
    public bool haveMaterials = false;

    void Start() {
        animations = GetComponent<Animations>();
        job = GetComponent<Job>();
        targets = GetComponent<Targets>();

        audioSource = GetComponent<AudioSource>();
    }

    public void ProcessWorking() {
        if (targets.target.GetComponent<Properties>().type == "building") {
            building = targets.target.GetComponent<Building>();
        }
        // this will only be true if target is a resource
        if (IsStillWorking() && building == null) {
            return;
        }
        if (IsBuilding()) {
            return;
        }
        FinishWorking();
    }

    public bool IsStillWorking() {
        return (Time.time - workStart) < workDone;
    }

    public bool IsBuilding() {
        if (building == null) {
            return false;
        }
        // order matters here; counting on short-circuit boolean
        if (NeedMaterials() || IsStillWorking() || IsBuilder()) {
            return true;
        }
        DoBuildingThings();
        return true;
    }

    public bool NeedMaterials() {
        if (!building.ReadyToProduce() && !building.Producing() && material == "") {
            GoGetTheThing();
            return true;
        }
        return false;
    }

    public bool IsBuilder() {
        if (GetComponent<Properties>().job != "builder") {
            return false;
        }
        if (!building.constructionStarted && !building.built) {
            // restart the work timer to 'build' the building if not built and not constructing
            building.constructionStarted = true;
            workStart = Time.time;
        } else {
            building.BuildIt();
            StopWorking();
        }
        return true;
    }

    public void PlayChopSound() {
        if ((int)((Time.time - workStart) * 50) % 20 == 0) {
            audioSource.PlayOneShot(workClip, 0.7F);
        }
    }

    public void PerformWorkActions() {
        animations.SetAnimation("side-attack", true);
        animations.anim.speed = 1;
    }

    void DoBuildingThings() {
        if (building.Producing()) {
            building.Produce();
            if (GetComponent<Properties>().job == "builder") {
                // why stop working?
                // because otherwise builder will act as the producer of the target building
                StopWorking();
            }
        }
    }

    // gets materials for building and for producing
    void GoGetTheThing() {
        material = building.NextStockToGet();
        targets.target = GameObject.Find("storage");
        return;
    }

    void FinishWorking() {
        Properties props = targets.target.GetComponent<Properties>();
        props.Haulify();
        props.ChangeSprite();
        animations.SetDefaultDirections();
        animations.SetAnimation("side", true);
        StopWorking();
    }

    public void StopWorking() {
        working = false;
        targets.target = null;
        if (building != null) {
            building.GetComponent<Properties>().SetDefaults();
        }
        building = null;
        targets.collided = false;
        targets.collisionObject = null;
    }

    public void DropMaterial() {
        if (haveMaterials && material != "") {
            TargetBucket.bucket.InstantiateResource(transform.position, ResourcePrefabs.resources.gatherableResourceSprites[material]);
        }
        haveMaterials = false;
        material = "";
    }

    public void CollectTarget(Properties props) {
        material = props.produces;
        targets.DecomposeTarget();
        haveMaterials = true;
    }

    public void PutInStorage() {
        targets.target = null;
        haveMaterials = false;
        audioSource.PlayOneShot(storageClip, 0.7F);
        ResourceCounter.counter.AddToStorage(material);
        material = "";

        // use messaging here to remove Targets dependency on Job
        job.TriggerCheckJob();
    }

    public void GetFromStorage(GameObject other) {
        // TODO use methods in ResourceCounter instead
        if (ResourceCounter.counter.counts[material] > 0) {
            targets.target = building.transform.gameObject;
            haveMaterials = true;
            ResourceCounter.counter.counts[material]--;
            targets.collided = false;
            targets.collisionObject = null;
        } else {
            targets.collided = true;
            targets.collisionObject = other;
        }
    }

    public void AddStock() {
        building.AddStock(material);
        haveMaterials = false;
        material = "";
        if (!building.built) {
            return;
        }
        workStart = Time.time;
    }

    public void StartWork(Properties props) {
        animations.SetDefaultDirections();
        working = true;
        PerformWorkActions();
        props.engaged = true;
        if (props.type != "building" || haveMaterials) {
            workStart = Time.time;
        }
    }

    public void AbandonTarget() {
        targets.target = null;
        building = null;
    }

    public void Execute(Properties props, GameObject other, string state) {
        if (state == "ResetTarget") {
            AbandonTarget();
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
}
