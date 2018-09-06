using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AssignmentCounter : MonoBehaviour {
    public static AssignmentCounter counter;
    public Dictionary<string, int> jobs;
    public Dictionary<string, TextMeshProUGUI> counters;
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
        // a lot going on here
        availableJobs = new List<string>() {"hauler", "harvester", "excavator", "builder", "sawyer"};
        jobs = new Dictionary<string, int>();
        counters = new Dictionary<string, TextMeshProUGUI>();
        Object textPrefab = Resources.Load("Prefabs/text-default", typeof(GameObject));

        Dictionary<string, Vector3> vectors = MakeVectors();

        foreach (string job in availableJobs) {
            jobs.Add(job, 0);

            GameObject jobContainer = ConstructJobContainer(job, vectors);
            GameObject jobText = ConstructJobText(job, vectors, jobContainer, textPrefab);
            GameObject jobNameText = ConstructJobNameText(job, vectors, jobContainer, textPrefab);

            SetAnchor(jobContainer, vectors["leftAlignVector"]);
            SetAnchor(jobText, vectors["leftAlignVector"]);
            SetAnchor(jobNameText, vectors["leftAlignVector"]);

            if (job != "hauler") {
                SetAdjustArrows(job, jobContainer.transform);
            }

            counters.Add(job, jobText.GetComponent<TextMeshProUGUI>());
        }
    }

    Dictionary<string, Vector3> MakeVectors() {
        Dictionary<string, Vector3> vectors = new Dictionary<string, Vector3>();
        vectors.Add("baseVector", new Vector3(110, 60, 0));
        vectors.Add("addRowVector", new Vector3(0, -25, 0));
        vectors.Add("counterVector", new Vector3(80, -10, 0));
        vectors.Add("nameVector", new Vector3(-40, 5, 0));
        vectors.Add("leftAlignVector", new Vector3(0, 0.5f, 0));
        vectors.Add("sizeDelta", new Vector3(100, 20, 0));
        return vectors;
    }

    GameObject ConstructJobContainer(string job, Dictionary<string, Vector3> vectors) {
        GameObject jobContainer = new GameObject(job, typeof(RectTransform));
        jobContainer.transform.SetParent(transform);
        jobContainer.GetComponent<RectTransform>().sizeDelta = vectors["sizeDelta"];
        jobContainer.GetComponent<RectTransform>().localPosition = vectors["baseVector"] + (vectors["addRowVector"] * counters.Count);
        jobContainer.layer = 5;
        return jobContainer;
    }

    GameObject ConstructJobText(string job, Dictionary<string, Vector3> vectors, GameObject jobContainer, Object textPrefab) {
        GameObject jobText = Instantiate(textPrefab, jobContainer.transform) as GameObject;
        jobText.name = "counter";
        SetAnchor(jobText, vectors["leftAlignVector"]);
        jobText.GetComponent<RectTransform>().localPosition = vectors["counterVector"];
        return jobText;
    }

    GameObject ConstructJobNameText(string job, Dictionary<string, Vector3> vectors, GameObject jobContainer, Object textPrefab) {
        GameObject jobNameText = Instantiate(textPrefab, jobContainer.transform) as GameObject;
        jobNameText.GetComponent<TextMeshProUGUI>().text = Representation.repr.CapitalizeFirstLetter(job);
        jobNameText.name = "label";
        SetAnchor(jobNameText, vectors["leftAlignVector"]);
        jobNameText.GetComponent<RectTransform>().sizeDelta = vectors["sizeDelta"];
        jobNameText.GetComponent<RectTransform>().localPosition = vectors["nameVector"];
        return jobNameText;
    }

    void SetAnchor(GameObject obj, Vector2 alignVector) {
        obj.GetComponent<RectTransform>().anchorMin = alignVector;
        obj.GetComponent<RectTransform>().anchorMax = alignVector;
    }

    void SetAdjustArrows(string job, Transform transform) {
        Object assignPrefab = Resources.Load("Prefabs/villager-assign", typeof(GameObject));
        Object unassignPrefab = Resources.Load("Prefabs/villager-unassign", typeof(GameObject));
        GameObject assign = Instantiate(assignPrefab, transform) as GameObject;
        assign.GetComponent<AddVillager>().job = job;
        GameObject unassign = Instantiate(unassignPrefab, transform) as GameObject;
        unassign.GetComponent<RemoveVillager>().job = job;
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
}
