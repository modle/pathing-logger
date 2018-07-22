using UnityEngine;

public class Building : MonoBehaviour {

    TargetID id;

    public void Start() {
        id = GetComponent<TargetID>();
        print ("look, a " + id.type + " that makes " + id.produces);
    }

    public void Produce() {
        print ("making a thing " + Time.time);
    }

}
