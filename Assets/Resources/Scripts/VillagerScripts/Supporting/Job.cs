using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Job : MonoBehaviour {

    public string job;
    public string baseJob;
    private Targets targets;
    private Villager villager;

    void Start() {
        targets = GetComponent<Targets>();
        villager = GetComponent<Villager>();
        StartCoroutine("CheckJob");
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
        villager.DropMaterial();
        if (targets.target != null) {
            targets.target.GetComponent<Properties>().AbandonTask();
            villager.StopWorking();
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
