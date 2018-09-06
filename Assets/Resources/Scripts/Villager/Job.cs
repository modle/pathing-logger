using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class Job : MonoBehaviour {

    private Properties properties;
    private Targets targets;
    private Work work;

    void Start() {
        properties = GetComponent<Properties>();
        targets = GetComponent<Targets>();
        work = GetComponent<Work>();
        StartCoroutine("CheckJob");
    }

    void Update() {
        transform.Find("villager-label(Clone)").GetComponent<TextMeshPro>().text = properties.id + " - " + properties.job + "/" + properties.baseJob;
    }

    IEnumerator CheckJob() {
        while (true) {
            if (targets.target == null) {
                properties.job = "hauler";
                foreach (GameObject go in TargetBucket.bucket.targets) {
                    if (go == null) {
                        continue;
                    }
                    Properties checkProps = go.GetComponent<Properties>();
                    if (!checkProps.selected || checkProps.targeted) {
                        continue;
                    }
                    // reassigns villagers to baseJob if job is needed
                    if (checkProps.job == properties.baseJob && properties.baseJob != "hauler") {
                        properties.job = properties.baseJob;
                        break;
                    }
                    // reassigns job to builder if building in progress needs materials
                    if (checkProps.job == "builder") {
                        properties.job = "builder";
                    }
                }
            }
            yield return new WaitForSeconds(2.0f);
        }
    }

    public void ChangeJob(string newJob) {
        properties.SetJob(newJob);
        work.DropMaterial();
        if (targets.target != null) {
            targets.target.GetComponent<Properties>().AbandonTask();
            work.StopWorking();
        }
    }

    public void TriggerCheckJob() {
        // allows villager to pivot roles on hauler task completion
        StopCoroutine("CheckJob");
        StartCoroutine("CheckJob");
    }
}
