using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignmentCounter : MonoBehaviour {
    public static AssignmentCounter counter;
    public Text chopperText;
    public Text haulerText;
    public Text idleText;
    public Dictionary<string, int> jobs;
    [HideInInspector]
    public List<string> availableJobs;

    void Awake() {
        // singleton pattern
        if (counter == null) {
            DontDestroyOnLoad(gameObject);
            counter = this;
        } else if (counter != this) {
            Destroy(gameObject);
        }
        availableJobs = new List<string>() {"chopper", "hauler", "idle"};
        jobs = new Dictionary<string, int>();
        foreach (string job in availableJobs) {
            jobs.Add(job, 0);
        }
    }

    void Update() {
        ShowCurrentAssignments();
        chopperText.text = jobs["chopper"].ToString();
        haulerText.text = jobs["hauler"].ToString();
        idleText.text = jobs["idle"].ToString();
    }

    void ShowCurrentAssignments() {
        string currentAssignments = "";
        foreach (KeyValuePair<string, int> entry in jobs) {
            currentAssignments += entry.Key + ":" + entry.Value + "; ";
        };
        // Debug.Log(currentAssignments);
    }
}
