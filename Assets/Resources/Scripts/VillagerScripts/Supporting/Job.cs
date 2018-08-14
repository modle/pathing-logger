using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Job : MonoBehaviour {

    public string job;
    public string baseJob;
    private Properties properties;
    private Targets targets;
    private Villager villager;
    private Work work;

    void Start() {
        properties = GetComponent<Properties>();
        targets = GetComponent<Targets>();
        villager = GetComponent<Villager>();
        work = GetComponent<Work>();
        StartCoroutine("CheckJob");
    }

    void Update() {
        transform.Find("villager-label(Clone)").GetComponent<TextMesh>().text = properties.id + " - " + job + "/" + baseJob;
    }

    public string GetJob() {
        return baseJob;
    }

    public string GetCurrentJob() {
        return job;
    }

    IEnumerator CheckJob() {
        while (true) {
            if (targets.target == null) {
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
        work.DropMaterial();
        if (targets.target != null) {
            targets.target.GetComponent<Properties>().AbandonTask();
            work.StopWorking();
        }
    }

    public void SetJob(string newJob) {
        job = newJob;
        baseJob = newJob;
    }

    public void TriggerCheckJob() {
        // allows villager to pivot roles on hauler task completion
        StopCoroutine("CheckJob");
        StartCoroutine("CheckJob");
    }

}
