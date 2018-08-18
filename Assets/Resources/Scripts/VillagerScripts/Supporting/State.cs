using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class State : MonoBehaviour {

    private Actions actions;
    private Animations animations;
    private Targets targets;
    private Work work;
    private Properties properties;

    public bool move;

    public void Start() {
        actions = GetComponent<Actions>();
        animations = GetComponent<Animations>();
        properties = GetComponent<Properties>();
        targets = GetComponent<Targets>();
        work = GetComponent<Work>();
    }

    void Update () {
        if (targets.collided) {
            Properties targetProps = targets.target.GetComponent<Properties>();
            properties.currentState = DetermineState(targetProps, targets.collisionObject);
            actions.Execute(targetProps, targets.collisionObject, properties.currentState);
            targets.collided = false;
            return;
        }
        if (work.working && work.IsStillWorking()) {
            work.PerformWorkActions();
            return;
        }
        if (work.working && properties.job != "hauler" && targets.target != null && work.material == "" && !work.IsStillWorking()) {
            work.ProcessWorking();
            return;
        }
        if (!targets.HasTarget()) {
            return;
        }
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
}
