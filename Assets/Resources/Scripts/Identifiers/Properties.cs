using UnityEngine;

// this class is being used for both villager and building properties
// this violates SRP
// use a base abstract class?
public class Properties : MonoBehaviour {
    public string type;
    public string produces;
    public string job;
    public string baseJob;
    public bool targeted;
    public bool engaged;
    public int targetedBy = 0;
    public bool selected;
    public bool destructable;
    public bool workable;
    public int id;
    public string currentState;

    public void SetJob(string newJob) {
        job = newJob;
        baseJob = newJob;
    }

    public void Haulify() {
        job = "hauler";
        workable = false;
        destructable = true;
        SetDefaults();
    }

    public void SetDefaults() {
        targeted = false;
        engaged = false;
        targetedBy = -1;
    }

    public void AbandonTask() {
        SetDefaults();
    }

    public void ChangeSprite() {
        name = produces;
        GetComponent<SpriteRenderer>().sprite =
            TargetManager.manager.harvestedSprites[type].GetComponent<SpriteRenderer>().sprite;
    }

    public void SetTargeted(int id) {
        targetedBy = id;
        targeted = true;
    }

    public void MatchProps(Properties props) {
        type = props.type;
        produces = props.produces;
        job = props.job;
        baseJob = props.baseJob;
        targeted = props.targeted;
        engaged = props.engaged;
        targetedBy = props.targetedBy;
        selected = props.selected;
        destructable = props.destructable;
        workable = props.workable;
        id = props.id;
        currentState = props.currentState;
    }

}
