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


    public string DetermineState(Properties props, GameObject other) {
        string state = "";
        state = state == "" && !props.targeted && props.type != "storage" ? "ResetTarget" : state;
        if (other.gameObject.GetInstanceID() == targets.target.GetInstanceID() && props.targeted) {
            state = state == "" && !props.workable ? "CollectTarget" : state;
            state = state == "" && work.haveMaterials && work.building != null ? "AddStock" : state;
            state = state == "" ? "StartWork" : state;
            return state;
        }
        if (props.type == "storage" && other.GetComponent<Properties>().type == "storage") {
            state = state == "" && work.haveMaterials && ResourceCounter.counter.resources.Contains(work.material) ? "PutInStorage" : state;
            state = state == "" && work.building != null && !work.haveMaterials ? "GetFromStorage" : state;
        }
        return state;
    }

    public void ExecuteStateAction(Properties props, GameObject other, string state) {
        if (state == "ResetTarget") {
            targets.target = null;
            return;
        }
        if (state == "AddStock") {
            work.AddStock();
            return;
        }
        if (state == "StartWork") {
            work.StartWork(props);
            return;
        }
        if (state == "CollectTarget") {
            targets.CollectTarget(props);
            return;
        }
        if (state == "PutInStorage") {
            targets.PutInStorage();
            return;
        }
        if (state == "GetFromStorage") {
            targets.GetFromStorage(other);
            return;
        }
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
