using UnityEngine;

public class Properties : MonoBehaviour {
    public string type;
    public string produces;
    public string job;
    public bool targeted;
    public bool engaged;
    public int targetedBy = 0;
    public bool selected;
    public bool destructable;
    public bool workable;

    public void Harvestify() {
        job = "harvester";
        Workify();
    }

    public void Workify() {
        workable = true;
        destructable = false;
        SetDefaults();
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

}
