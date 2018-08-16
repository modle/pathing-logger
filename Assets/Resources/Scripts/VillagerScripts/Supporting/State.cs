using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class State : MonoBehaviour {

    private Animations animations;
    private Job job;
    private Targets targets;
    private Work work;
    private Properties props;

    public void Start() {
        animations = GetComponent<Animations>();
        job = GetComponent<Job>();
        targets = GetComponent<Targets>();
        work = GetComponent<Work>();
    }

    void Update () {
        if (targets.collided) {
            Properties props = targets.target.GetComponent<Properties>();
            string currentState = DetermineState(props, targets.collisionObject);
            ExecuteStateAction(props, targets.collisionObject, currentState);
            targets.collided = false;
            return;
        }
        if (targets.recollide) {
            targets.ProcessCollision(targets.collisionObject);
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
}
