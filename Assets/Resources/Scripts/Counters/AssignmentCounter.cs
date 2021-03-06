using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AssignmentCounter : MonoBehaviour {
    // TODO rename to AssignmentManager
    public static AssignmentCounter counter;
    public Dictionary<string, int> jobs = new Dictionary<string, int>();
    public Dictionary<string, TextMeshProUGUI> counters = new Dictionary<string, TextMeshProUGUI>();

    // making this public causes it to not be set at load time, even with [HideInInspector]
    private List<string> availableJobs = new List<string>() {"hauler", "harvester", "builder", "sawyer", "excavator"};
    private List<string> disableTheseJobs = new List<string>() {"sawyer", "excavator"};

    void Awake() {
        // singleton pattern
        if (counter == null) {
            counter = this;
        } else if (counter != this) {
            Destroy(gameObject);
        }

        BuildAssignments();
    }

    void BuildAssignments() {
        Object assignmentPrefab = Resources.Load("Prefabs/assignment-base", typeof(GameObject));
        Vector3 baseVector = new Vector3(30, 60, 0);

        foreach (string job in availableJobs) {
            Vector3 offsetVector = new Vector3(0, -25 * jobs.Count, 0);
            GameObject theJob = Instantiate(assignmentPrefab) as GameObject;

            theJob.name = job;
            theJob.transform.Find("label").GetComponent<TextMeshProUGUI>().text = Representation.repr.CapitalizeFirstLetter(job);
            theJob.transform.Find("assign").GetComponent<AddVillager>().job = job;
            theJob.transform.Find("unassign").GetComponent<RemoveVillager>().job = job;
            theJob.GetComponent<RectTransform>().localPosition = transform.position + baseVector + offsetVector;
            if (job == "hauler") {
                Destroy(theJob.transform.Find("assign").gameObject);
                Destroy(theJob.transform.Find("unassign").gameObject);
            }

            jobs.Add(job, 0);
            counters.Add(job, theJob.transform.Find("counter").GetComponent<TextMeshProUGUI>());
            theJob.transform.SetParent(transform);
            if (disableTheseJobs.Contains(job)) {
                theJob.SetActive(false);
            }
        }
    }

    void Update() {
        CountVillagers();
        UpdateTexts();
    }

    void CountVillagers() {
        Dictionary<string, int> theJobs = new Dictionary<string, int>();
        foreach (string job in availableJobs) {
            theJobs.Add(job, 0);
        }
        foreach(Transform villager in VillagerBucket.bucket.villagers) {
            theJobs[villager.gameObject.GetComponent<Properties>().baseJob]++;
        }
        jobs = theJobs;
    }

    void UpdateTexts() {
        foreach (string job in availableJobs) {
            counters[job].text = jobs[job].ToString();
        }
    }

    public string AssignJob() {
        List<string> activeJobs = GetActiveJobs();
        string job = activeJobs[Random.Range(0, activeJobs.Count)];
        jobs[job]++;
        return job;
    }

    private List<string> GetActiveJobs() {
        List<string> activeJobs = new List<string>();
        foreach (Transform child in transform) {
            if (child.gameObject.activeSelf) {
                activeJobs.Add(child.gameObject.name);
            }
        }
        return activeJobs;
    }

    public void EnableJob(string job) {
        foreach (Transform child in transform) {
            if (child.gameObject.name == job) {
                child.gameObject.SetActive(true);
                break;
            }
        }
    }
}
