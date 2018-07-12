using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignmentCounter : MonoBehaviour {
    public static AssignmentCounter counter;
    public int chopper;
    public int hauler;
    public Text chopperText;
    public Text haulerText;
    public Dictionary<string, int> jobs;
    public List<string> availableJobs = new List<string>() {
        "chopper", "hauler"
    };

    void Awake() {
        // singleton pattern
        if (counter == null) {
            DontDestroyOnLoad(gameObject);
            counter = this;
        } else if (counter != this) {
            Destroy(gameObject);
        }
        jobs = new Dictionary<string, int>();
        foreach (string job in availableJobs) {
            jobs.Add(job, 0);
        }
    }

    void Update() {
        ShowCurrentAssignments();
        chopper = jobs["chopper"];
        hauler = jobs["hauler"];
        chopperText.text = chopper.ToString();
        haulerText.text = hauler.ToString();
    }

    void ShowCurrentAssignments() {
        string currentAssignments = "";
        foreach (KeyValuePair<string, int> entry in jobs) {
            currentAssignments += entry.Key + ":" + entry.Value + "; ";
        };
        // Debug.Log(currentAssignments);
    }
}
