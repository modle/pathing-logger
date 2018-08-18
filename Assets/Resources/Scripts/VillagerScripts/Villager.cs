using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Villager : MonoBehaviour {

    public int id;

    private Properties properties;
    private Targets targets;
    private Work work;

    void Start () {
        SetInitialReferences();
    }

    void SetInitialReferences() {
        properties = GetComponent<Properties>();
        targets = GetComponent<Targets>();
        work = GetComponent<Work>();
    }

    public string GetRepr() {
        return CapitalizeFirstLetter(properties.job) + "/" + CapitalizeFirstLetter(properties.baseJob) + " (" + properties.id.ToString() + ")" +
            "\n" + (work.working ? "working" : "idle") + " " +
            string.Format("{0:0.0}", (Time.time - work.workStart < 2.0f ? Time.time - work.workStart : 0f)) +
            "\ntarget: " + (targets.target == null ? "" : CapitalizeFirstLetter(targets.target.name)) +
            "\nbuilding: " + (work.building == null ? "" : CapitalizeFirstLetter(work.building.name)) +
            "\n" + (work.haveMaterials ? "carrying: " : "finding: ") + work.material +
            "\nstate: " + properties.currentState;
    }

    string CapitalizeFirstLetter(string s) {
        char[] a = s.ToCharArray();
        a[0] = char.ToUpper(a[0]);
        return new string(a);
    }

}
