using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class State : MonoBehaviour {

    private Animations animations;
    private Targets targets;
    private Work work;
    private Properties properties;

    public bool move;

    public void Start() {
        animations = GetComponent<Animations>();
        properties = GetComponent<Properties>();
        targets = GetComponent<Targets>();
        work = GetComponent<Work>();
    }

    void Update () {
        if (work.working && work.IsStillWorking()) {
            work.PlayChopSound();
            return;
        }
        if (targets.collided) {
            Properties targetProps = targets.target.GetComponent<Properties>();
            properties.currentState = DetermineState(targetProps, targets.collisionObject);
            work.Execute(targetProps, targets.collisionObject, properties.currentState);
            targets.collided = false;
            return;
        }
        if (work.working && properties.job != "hauler" && targets.target != null && work.material == "" && !work.IsStillWorking()) {
            work.ProcessWorking();
            return;
        }
        if (!targets.HasTarget()) {
            return;
        }

        targets.CheckForRecollision();
        animations.Move(targets.target);
    }

    public string DetermineState(Properties targetProps, GameObject other) {
        string state = "";
        state = state == "" && !targetProps.targeted && targetProps.type != "storage" ? "ResetTarget" : state;
        if (other.gameObject.GetInstanceID() == targets.target.GetInstanceID() && targetProps.targeted) {
            state = state == "" && !targetProps.workable ? "CollectTarget" : state;
            state = state == "" && work.haveMaterials && work.building != null ? "AddStock" : state;
            state = state == "" ? "StartWork" : state;
            return state;
        }
        if (targetProps.type == "storage" && other.GetComponent<Properties>().type == "storage") {
            state = state == "" && work.haveMaterials && ResourceCounter.counter.resources.Contains(work.material) ? "PutInStorage" : state;
            state = state == "" && work.building != null && !work.haveMaterials ? "GetFromStorage" : state;
        }
        return state;
    }

    public string GetRepr() {
        return Representation.repr.CapitalizeFirstLetter(properties.job) + "/" + Representation.repr.CapitalizeFirstLetter(properties.baseJob) + " (" + properties.id.ToString() + ")" +
            "\n" + (work.working ? "working" : "idle") + " " +
            string.Format("{0:0.0}", (Time.time - work.workStart < 2.0f ? Time.time - work.workStart : 0f)) +
            "\ntarget: " + (targets.target == null ? "" : Representation.repr.CapitalizeFirstLetter(targets.target.name)) +
            "\nbuilding: " + (work.building == null ? "" : Representation.repr.CapitalizeFirstLetter(work.building.name)) +
            "\n" + (work.haveMaterials ? "carrying: " : "finding: ") + work.material +
            "\nstate: " + properties.currentState;
    }
}
