using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignmentCounter : MonoBehaviour {
    public static AssignmentCounter counter;
    public Dictionary<string, int> jobs;
    public Dictionary<string, Text> counters;
    [HideInInspector]
    public List<string> availableJobs;

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
        availableJobs = new List<string>() {"hauler", "harvester", "sawyer", "builder"};
        jobs = new Dictionary<string, int>();
        counters = new Dictionary<string, Text>();

        Vector3 baseVector = new Vector3(25, 60, 0);
        Vector3 addRowVector = new Vector3(0, -25, 0);
        Vector3 counterVector = new Vector3(20, 5, 0);
        Vector3 nameVector = new Vector3(-40, 5, 0);
        Vector2 leftAlignVector = new Vector2(0, 0.5f);
        foreach (string job in availableJobs) {
            jobs.Add(job, 0);

            GameObject jobContainer = new GameObject(job, typeof(RectTransform));
            jobContainer.transform.SetParent(transform);
            jobContainer.GetComponent<RectTransform>().anchorMin = leftAlignVector;
            jobContainer.GetComponent<RectTransform>().anchorMax = leftAlignVector;
            jobContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 20);
            jobContainer.GetComponent<RectTransform>().localPosition = baseVector + (addRowVector * counters.Count);
            jobContainer.layer = 5;

            Object textPrefab = Resources.Load("Prefabs/text-default", typeof(GameObject));
            GameObject jobText = Instantiate(textPrefab, jobContainer.transform) as GameObject;
            jobText.name = "counter";
            jobText.GetComponent<RectTransform>().anchorMin = leftAlignVector;
            jobText.GetComponent<RectTransform>().anchorMax = leftAlignVector;
            jobText.GetComponent<RectTransform>().localPosition = counterVector;

            GameObject jobNameText = Instantiate(textPrefab, jobContainer.transform) as GameObject;
            jobNameText.GetComponent<Text>().text = CapitalizeFirstLetter(job);
            jobNameText.name = "label";
            jobNameText.GetComponent<RectTransform>().anchorMin = leftAlignVector;
            jobNameText.GetComponent<RectTransform>().anchorMax = leftAlignVector;
            jobNameText.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 20);
            jobNameText.GetComponent<RectTransform>().localPosition = nameVector;

            if (job != "hauler") {
                Object assignPrefab = Resources.Load("Prefabs/villager-assign", typeof(GameObject));
                GameObject assign = Instantiate(assignPrefab, jobContainer.transform) as GameObject;
                assign.GetComponent<AddVillager>().job = job;
                Object unassignPrefab = Resources.Load("Prefabs/villager-unassign", typeof(GameObject));
                GameObject unassign = Instantiate(unassignPrefab, jobContainer.transform) as GameObject;
                unassign.GetComponent<RemoveVillager>().job = job;
            }

            counters.Add(job, jobText.GetComponent<Text>());
        }
    }

    string CapitalizeFirstLetter(string s) {
        char[] a = s.ToCharArray();
        a[0] = char.ToUpper(a[0]);
        return new string(a);
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
            theJobs[villager.gameObject.GetComponent<Villager>().job]++;
        }
        jobs = theJobs;
    }

    void UpdateTexts() {
        foreach (string job in availableJobs) {
            counters[job].text = jobs[job].ToString();
        }
    }
}
