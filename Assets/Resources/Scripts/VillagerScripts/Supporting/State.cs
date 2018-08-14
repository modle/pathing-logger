using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class State : MonoBehaviour {

    private Targets targets;
    private Work work;
 
    public void Start() {
        work = GetComponent<Work>();
        targets = GetComponent<Targets>();
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
