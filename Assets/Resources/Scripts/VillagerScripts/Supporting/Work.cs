using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Work : MonoBehaviour {

    private Targets targets;
    public Building building;
    public Animations animations;
    public Job job;

    public bool working;
    public float workStart = 0f;
    public float workDone = 2f;
    public AudioClip workClip;

    public string material;
    public bool haveMaterials = false;

    void Start() {
        targets = GetComponent<Targets>();
        animations = GetComponent<Animations>();
        job = GetComponent<Job>();
    }

    public void ProcessWorking() {
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

    public bool IsStillWorking() {
        return (Time.time - workStart) < workDone;
    }

    public void PerformWorkActions() {
        animations.anim.SetBool("side-attack", true);
        animations.anim.speed = 1;
        if ((int)((Time.time - workStart) * 100) % 30 == 0) {
            targets.audioSource.PlayOneShot(workClip, 0.7F);
        }
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
        targets.recollide = false;
        targets.collisionObject = null;
    }

    public void DropMaterial() {
        if (haveMaterials && material != "") {
            TargetBucket.bucket.InstantiateResource(transform.position, ResourcePrefabs.resources.gatherableResourceSprites[material]);
        }
        haveMaterials = false;
        material = "";
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
        working = true;
        props.engaged = true;
        if (props.type != "building" || haveMaterials) {
            workStart = Time.time;
        }
    }
}
