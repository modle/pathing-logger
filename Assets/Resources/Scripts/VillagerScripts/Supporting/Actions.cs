using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Actions : MonoBehaviour {

    private Targets targets;
    private Work work;

    public void Start() {
        targets = GetComponent<Targets>();
        work = GetComponent<Work>();
    }

    public void Execute(Properties props, GameObject other, string state) {
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
