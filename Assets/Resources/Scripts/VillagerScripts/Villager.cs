using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Villager : MonoBehaviour {

    private Transform transform;

    public int id;
    private Rect idRect;
    private BoxCollider2D boxCollider;
    public LayerMask blockingLayer;

    private Actions actions;
    private Animations animations;
    private Job job;
    private State stateObj;
    private Targets targets;
    private Work work;

    void Start () {
        SetInitialReferences();
    }

    void SetInitialReferences() {
        transform = GetComponent<Transform>();
        boxCollider = GetComponent<BoxCollider2D>();

        actions = GetComponent<Actions>();
        animations = GetComponent<Animations>();
        job = GetComponent<Job>();
        stateObj = GetComponent<State>();
        targets = GetComponent<Targets>();
        work = GetComponent<Work>();
    }

    void Update () {
        transform.Find("villager-label(Clone)").GetComponent<TextMesh>().text = id + " - " + job.GetCurrentJob() + "/" + job.GetJob();
        if (targets.ProcessCollision(targets.collisionObject)) {
            return;
        }
        if (work.working && work.IsStillWorking()) {
            work.PerformWorkActions();
            return;
        }
        if (work.working && job.GetCurrentJob() != "hauler" && targets.target != null && work.material == "" && !work.IsStillWorking()) {
            work.ProcessWorking();
            return;
        }
        if (!targets.HasTarget()) {
            return;
        }
        animations.Move(targets.target);
        // every 5 seconds
    }

    public string GetRepr() {
        return CapitalizeFirstLetter(job.GetCurrentJob()) + "/" + CapitalizeFirstLetter(job.GetJob()) + " (" + id.ToString() + ")" +
            "\n" + (work.working ? "working" : "idle") + " " +
            string.Format("{0:0.0}", (Time.time - work.workStart < 2.0f ? Time.time - work.workStart : 0f)) +
            "\ntarget: " + (targets.target == null ? "" : CapitalizeFirstLetter(targets.target.name)) +
            "\nbuilding: " + (work.building == null ? "" : CapitalizeFirstLetter(work.building.name)) +
            "\n" + (work.haveMaterials ? "carrying: " : "finding: ") + work.material;
    }

    string CapitalizeFirstLetter(string s) {
        char[] a = s.ToCharArray();
        a[0] = char.ToUpper(a[0]);
        return new string(a);
    }

}
